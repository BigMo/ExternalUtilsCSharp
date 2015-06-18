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
        #region PROPERTIES
        public CSLocalPlayer LocalPlayer { get; private set; }
        public BaseEntity Target { get; private set; }
        public Tuple<int, CSPlayer>[] Players { get; private set; }
        public Tuple<int, BaseEntity>[] Entities { get; private set; }
        public Tuple<int, Weapon>[] Weapons { get; private set; }
        public Matrix ViewMatrix { get; private set; }
        private int dwEntityList, dwViewMatrix, dwLocalPlayer, clientDllBase, engineDllBase;
        #endregion

        #region CONSTRUCTOR
        public Framework(ProcessModule clientDll, ProcessModule engineDll)
        {
            CSGOScanner.ScanOffsets(WithOverlay.MemUtils, clientDll, engineDll);
            clientDllBase = (int)clientDll.BaseAddress;
            engineDllBase = (int)engineDll.BaseAddress;
            dwEntityList = clientDllBase + CSGOOffsets.MiscEntityList;
            dwViewMatrix = clientDllBase + CSGOOffsets.MiscViewMatrix;
        }
        #endregion

        #region METHODS
        public void Update()
        {
            List<Tuple<int, CSPlayer>> players = new List<Tuple<int, CSPlayer>>();
            List<Tuple<int, BaseEntity>> entities = new List<Tuple<int, BaseEntity>>();
            List<Tuple<int, Weapon>> weapons = new List<Tuple<int, Weapon>>();

            dwLocalPlayer = WithOverlay.MemUtils.Read<int>((IntPtr)(clientDllBase + CSGOOffsets.MiscLocalPlayer));
            ViewMatrix = WithOverlay.MemUtils.ReadMatrix((IntPtr)dwViewMatrix, 4, 4);

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

            Players = players.ToArray();
            Entities = entities.ToArray();
            Weapons = weapons.ToArray();
        }
        #endregion
    }
}
