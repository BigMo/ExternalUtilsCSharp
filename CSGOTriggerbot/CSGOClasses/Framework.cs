using ExternalUtilsCSharp;
using ExternalUtilsCSharp.MathObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOTriggerbot.CSGOClasses
{
    public class Framework
    {
        #region VARIABLES
        private int
            dwEntityList,
            dwViewMatrix, 
            dwLocalPlayer,
            dwClientState,
            clientDllBase, 
            engineDllBase,
            dwIGameResources;
        private bool mouseEnabled;
        #endregion
        #region PROPERTIES
        public CSLocalPlayer LocalPlayer { get; private set; }
        public BaseEntity Target { get; private set; }
        public Tuple<int, CSPlayer>[] Players { get; private set; }
        public Tuple<int, BaseEntity>[] Entities { get; private set; }
        public Tuple<int, Weapon>[] Weapons { get; private set; }
        public Matrix ViewMatrix { get; private set; }
        public Vector3 ViewAngles { get; private set; }
        public Vector3 NewViewAngles { get; private set; }
        public int[] Kills { get; private set; }
        public int[] Deaths { get; private set; }
        public int[] Assists { get; private set; }
        public int[] Armor { get; private set; }
        public int[] Score { get; private set; }
        public string[] Clantags { get; private set; }
        public string[] Names { get; private set; }
        public SignOnState State { get; set; }
        public bool MouseEnabled
        { 
            get { return mouseEnabled; }
            set
            {
                if(value != mouseEnabled)
                {
                    mouseEnabled = value;
                    WinAPI.SetCursorPos(WithOverlay.SHDXOverlay.Location.X + WithOverlay.SHDXOverlay.Width / 2, WithOverlay.SHDXOverlay.Location.Y + WithOverlay.SHDXOverlay.Height / 2);
                    WithOverlay.MemUtils.Write<byte>((IntPtr)(clientDllBase + CSGOOffsets.Misc.MouseEnable), value ? (byte)1 : (byte)0);
                }
            }
        }
        public bool AimbotActive { get; set; }
        public bool RCSHandled { get; set; }
        #endregion

        #region CONSTRUCTOR
        public Framework(ProcessModule clientDll, ProcessModule engineDll)
        {
            CSGOScanner.ScanOffsets(WithOverlay.MemUtils, clientDll, engineDll);
            clientDllBase = (int)clientDll.BaseAddress;
            engineDllBase = (int)engineDll.BaseAddress;
            dwEntityList = clientDllBase + CSGOOffsets.Misc.EntityList;
            dwViewMatrix = clientDllBase + CSGOOffsets.Misc.ViewMatrix;
            dwClientState = WithOverlay.MemUtils.Read<int>((IntPtr)(engineDllBase + CSGOOffsets.ClientState.Base));
            mouseEnabled = true;
            AimbotActive = false;
        }
        #endregion

        #region METHODS
        public void Update()
        {
            List<Tuple<int, CSPlayer>> players = new List<Tuple<int, CSPlayer>>();
            List<Tuple<int, BaseEntity>> entities = new List<Tuple<int, BaseEntity>>();
            List<Tuple<int, Weapon>> weapons = new List<Tuple<int, Weapon>>();

            dwLocalPlayer = WithOverlay.MemUtils.Read<int>((IntPtr)(clientDllBase + CSGOOffsets.Misc.LocalPlayer));
            dwIGameResources = WithOverlay.MemUtils.Read<int>((IntPtr)(clientDllBase + CSGOOffsets.GameResources.Base));
            
            State = (SignOnState)WithOverlay.MemUtils.Read<int>((IntPtr)(dwClientState + CSGOOffsets.ClientState.SignOnState));
            if (State != SignOnState.SIGNONSTATE_FULL)
                return;

            ViewMatrix = WithOverlay.MemUtils.ReadMatrix((IntPtr)dwViewMatrix, 4, 4);
            ViewAngles = WithOverlay.MemUtils.Read<Vector3>((IntPtr)(dwClientState + CSGOOffsets.ClientState.SetViewAngles));
            NewViewAngles = ViewAngles;
            RCSHandled = false;

            #region Read entities
            byte[] data = new byte[16 * 8192];
            WithOverlay.MemUtils.Read((IntPtr)(dwEntityList), out data, data.Length);

            for (int i = 0; i < data.Length / 16; i++)
            {
                int address = BitConverter.ToInt32(data, 16 * i);
                if (address != 0)
                {
                    BaseEntity ent = new BaseEntity(address);
                    if (!ent.IsValid())
                        continue;
                    if (ent.IsPlayer())
                        players.Add(new Tuple<int, CSPlayer>(i, new CSPlayer(ent)));
                    else if (ent.IsWeapon())
                        weapons.Add(new Tuple<int, Weapon>(i, new Weapon(ent)));
                    else
                        entities.Add(new Tuple<int, BaseEntity>(i, ent));
                }
            }
            #endregion

            #region LocalPlayer and Target
            if (players.Exists(x => x.Item2.Address == dwLocalPlayer))
                LocalPlayer = new CSLocalPlayer(players.First(x => x.Item2.Address == dwLocalPlayer).Item2);
            else
                LocalPlayer = null;

            if (LocalPlayer != null)
            {
                if (entities.Exists(x => x.Item1 == LocalPlayer.m_iCrosshairIdx - 1))
                    Target = entities.First(x => x.Item1 == LocalPlayer.m_iCrosshairIdx - 1).Item2;
                if (players.Exists(x => x.Item1 == LocalPlayer.m_iCrosshairIdx - 1))
                    Target = players.First(x => x.Item1 == LocalPlayer.m_iCrosshairIdx - 1).Item2;
                else
                    Target = null;
            }
            #endregion

            #region IGameResources
            if (dwIGameResources != 0)
            {
                Kills = WithOverlay.MemUtils.ReadArray<int>((IntPtr)(dwIGameResources + CSGOOffsets.GameResources.Kills), 65);
                Deaths = WithOverlay.MemUtils.ReadArray<int>((IntPtr)(dwIGameResources + CSGOOffsets.GameResources.Deaths), 65);
                Armor = WithOverlay.MemUtils.ReadArray<int>((IntPtr)(dwIGameResources + CSGOOffsets.GameResources.Armor), 65);
                Assists = WithOverlay.MemUtils.ReadArray<int>((IntPtr)(dwIGameResources + CSGOOffsets.GameResources.Assists), 65);
                Score = WithOverlay.MemUtils.ReadArray<int>((IntPtr)(dwIGameResources + CSGOOffsets.GameResources.Score), 65);

                byte[] clantagsData = new byte[16 * 65];
                WithOverlay.MemUtils.Read((IntPtr)(dwIGameResources + CSGOOffsets.GameResources.Clantag), out clantagsData, clantagsData.Length);
                string[] clantags = new string[65];
                for (int i = 0; i < 65; i++)
                    clantags[i] = Encoding.Unicode.GetString(clantagsData, i * 16, 16);
                Clantags = clantags;

                int[] namePtrs = WithOverlay.MemUtils.ReadArray<int>((IntPtr)(dwIGameResources + CSGOOffsets.GameResources.Names), 65);
                string[] names = new string[65];
                for (int i = 0; i < 65; i++)
                    names[i] = WithOverlay.MemUtils.ReadString((IntPtr)namePtrs[i], 32, Encoding.ASCII);
                Names = names;
            }
            #endregion

            Players = players.ToArray();
            Entities = entities.ToArray();
            Weapons = weapons.ToArray();

            #region Aimbot
            if (WithOverlay.ConfigUtils.GetValue<bool>("aimEnabled"))
            {
                if (WithOverlay.ConfigUtils.GetValue<bool>("aimToggle"))
                {
                    if (WithOverlay.KeyUtils.KeyWentUp(WithOverlay.ConfigUtils.GetValue<WinAPI.VirtualKeyShort>("aimKey")))
                        AimbotActive = !AimbotActive;
                }
                else if (WithOverlay.ConfigUtils.GetValue<bool>("aimHold"))
                {
                    AimbotActive = WithOverlay.KeyUtils.KeyIsDown(WithOverlay.ConfigUtils.GetValue<WinAPI.VirtualKeyShort>("aimKey"));
                }
                if (AimbotActive)
                    DoAimbot();
            }
            #endregion

            #region RCS
            if (WithOverlay.ConfigUtils.GetValue<bool>("rcsEnabled"))
            {
                if (!RCSHandled && LocalPlayer.m_iShotsFired > 0)
                {
                    NewViewAngles -= LocalPlayer.m_vecPunch * (2f / 100f * WithOverlay.ConfigUtils.GetValue<float>("rcsForce"));
                    RCSHandled = true;
                }
            }
            #endregion

            #region Set view angles
            if (NewViewAngles != ViewAngles)
                SetViewAngles(NewViewAngles);
            #endregion
        }

        public void SetViewAngles(Vector3 viewAngles, bool clamp = true)
        {
            if (clamp)
                viewAngles = MathUtils.ClampAngle(viewAngles);
            WithOverlay.MemUtils.Write<Vector3>((IntPtr)(dwClientState + CSGOOffsets.ClientState.SetViewAngles), viewAngles);
        }

        public bool IsPlaying()
        {
            return State == SignOnState.SIGNONSTATE_FULL;
        }

        public void DoAimbot()
        {
            if (LocalPlayer == null)
                return;
            var valid = Players.Where(x => x.Item2.IsValid() && x.Item2.m_iHealth != 0);
            if (WithOverlay.ConfigUtils.GetValue<bool>("aimFilterSpotted"))
                valid = valid.Where(x => x.Item2.SeenBy(LocalPlayer));
            if (WithOverlay.ConfigUtils.GetValue<bool>("aimFilterSpottedBy"))
                valid = valid.Where(x => LocalPlayer.SeenBy(x.Item2));
            if (WithOverlay.ConfigUtils.GetValue<bool>("aimFilterEnemies"))
                valid = valid.Where(x => x.Item2.m_iTeamNum != LocalPlayer.m_iTeamNum);
            if (WithOverlay.ConfigUtils.GetValue<bool>("aimFilterAllies"))
                valid = valid.Where(x => x.Item2.m_iTeamNum == LocalPlayer.m_iTeamNum);

            valid = valid.OrderBy(x => (x.Item2.m_vecOrigin - LocalPlayer.m_vecOrigin).Length());
            Vector3 viewAngles = ViewAngles;
            Vector3 closest = Vector3.Zero;
            float closestFov = float.MaxValue;
            foreach(Tuple<int, CSPlayer> tpl in valid)
            {
                CSPlayer plr = tpl.Item2;
                //float tps = 1f / WithOverlay.SHDXOverlay.LogicUpdater.FrameRate;
                //Vector3 newAngles = MathUtils.CalcAngle(LocalPlayer.m_vecOrigin + LocalPlayer.m_vecViewOffset + LocalPlayer.m_vecVelocity * tps, plr.Bones.Spine3 + plr.m_vecVelocity * tps) - viewAngles;
                Vector3 newAngles = MathUtils.CalcAngle(LocalPlayer.m_vecOrigin + LocalPlayer.m_vecViewOffset, plr.Bones.Neck) - viewAngles;
                newAngles = MathUtils.ClampAngle(newAngles);
                float fov = newAngles.Length() % 360f;
                if (fov < closestFov && fov < WithOverlay.ConfigUtils.GetValue<float>("aimFov"))
                {
                    closestFov = fov;
                    closest = newAngles;
                }
            }
            if (closest != Vector3.Zero)
            {
                if (WithOverlay.ConfigUtils.GetValue<bool>("rcsEnabled"))
                {
                    if (!RCSHandled && LocalPlayer.m_iShotsFired > 0)
                    {
                        closest -= LocalPlayer.m_vecPunch * (2f / 100f * WithOverlay.ConfigUtils.GetValue<float>("rcsForce"));
                        RCSHandled = true;
                    }
                }
                if (WithOverlay.ConfigUtils.GetValue<bool>("aimSmoothEnabled"))
                    viewAngles = MathUtils.SmoothAngle(viewAngles, viewAngles + closest, WithOverlay.ConfigUtils.GetValue<float>("aimSmoothValue"));
                else
                    viewAngles += closest;
                NewViewAngles = viewAngles;
            }
        }
        #endregion
    }
}
