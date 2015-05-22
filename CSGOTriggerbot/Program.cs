using ExternalUtilsCSharp;
using ExternalUtilsCSharp.MathObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSGOTriggerbot
{
    class Program
    {
        #region OFFSETS
        private const int offsetEntityList = 0x049EE2E4;
        private const int offsetLocalPlayer = 0x00A4CA5C;
        private const int offsetJump = 0x04A7CE50;
        private const int offsetClientState = 0x5C71B4;
        private const int offsetSetViewAngles = 0x00004CE0;
        #endregion

        #region VARIABLES
        private static bool m_bWork;
        private static Vector3 vecPunch = Vector3.Zero;
        private static KeyUtils keyUtils;
        private static bool m_bRCSComplete, m_bRCSEnabled;
        private static bool m_bBunnyhop;
        #endregion

        static void Main(string[] args)
        {
            m_bWork = true;
            keyUtils = new KeyUtils();
            Thread thread = new Thread(new ThreadStart(Loop));
            thread.IsBackground = true;
            thread.Start();

            Console.WriteLine("Press ESC to exit");
            Console.WriteLine("Press NUMPAD0 to toggle RCS on/off");
            Console.WriteLine("Press NUMPAD1 to toggle RCS mode");
            Console.WriteLine("Press NUMPAD2 to toggle bunnyhop on/off");
            Console.WriteLine("Hold SPACE for bunnyhop");
            Console.WriteLine("Hold MOUSE3 for triggerbot");
            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                Thread.Sleep(100);
            }
            Console.WriteLine("Waiting for thread to exit...");
            m_bWork = false;
            thread.Join();

            Console.WriteLine("Bye.");
        }

        private static void Loop()
        {
            ProcUtils proc;
            ProcessModule clientDll = null;
            ProcessModule engineDll = null;
            byte[] data;
            CSGOPlayer[] players = new CSGOPlayer[64];
            int entityListAddress;
            int localPlayerAddress;
            int clientStateAddress;
            int setViewAnglesAddress;
            CSGOPlayer nullPlayer = new CSGOPlayer() { m_iID = 0, m_iHealth = 0, m_iTeam = 0 };
            CSGOLocalPlayer localPlayer;
            m_bRCSComplete = false;
            m_bRCSEnabled = true;
            m_bBunnyhop = true;

            //Wait for process to spawn
            while (!ProcUtils.ProcessIsRunning("csgo")) { Thread.Sleep(500); }
            proc = new ProcUtils("csgo", WinAPI.ProcessAccessFlags.All);
            MemUtils.Handle = proc.Handle;
            
            //Get client.dll & engine.dll
            while (clientDll == null) { clientDll = proc.GetModuleByName(@"bin\client.dll"); }
            while (engineDll == null) { engineDll = proc.GetModuleByName("engine.dll"); }

            int clientDllBase = clientDll.BaseAddress.ToInt32();
            int engineDllBase = engineDll.BaseAddress.ToInt32();
            //Run triggerbot
            while (proc.IsRunning && m_bWork)
            {
                if (WinAPI.GetForegroundWindow() != proc.Process.MainWindowHandle)
                    continue;
                keyUtils.Update();

                if (keyUtils.KeyWentUp(WinAPI.VirtualKeyShort.NUMPAD0))
                    m_bRCSEnabled = !m_bRCSEnabled;
                if (keyUtils.KeyWentUp(WinAPI.VirtualKeyShort.NUMPAD1))
                    m_bRCSComplete = !m_bRCSComplete;
                if (keyUtils.KeyWentUp(WinAPI.VirtualKeyShort.NUMPAD2))
                    m_bBunnyhop = !m_bBunnyhop;

                //Let's use int as datatype for addresses&pointers since we're in a 32bit process...
                entityListAddress = clientDll.BaseAddress.ToInt32() + offsetEntityList;
                localPlayerAddress = MemUtils.Read<int>((IntPtr)(offsetLocalPlayer + clientDllBase));
                localPlayer = MemUtils.Read<CSGOLocalPlayer>((IntPtr)(localPlayerAddress));
                clientStateAddress = MemUtils.Read<int>((IntPtr)(engineDllBase + offsetClientState));
                setViewAnglesAddress = clientStateAddress + offsetSetViewAngles;

                //Sanity checks
                if (!localPlayer.IsValid())
                    continue;

                //Read entitylist
                if (!MemUtils.Read((IntPtr)(clientDllBase + offsetEntityList), out data, 16 * 64))
                {
                    Console.WriteLine("ERROR: Failed to read entitylist!");
                    Thread.Sleep(1000);
                    continue;
                }

                //Read entities (players)
                for (int i = 0; i < data.Length / 64; i++)
                {
                    int address = BitConverter.ToInt32(data, i * 16);
                    if(address != 0)
                    {
                        players[i] = MemUtils.Read<CSGOPlayer>((IntPtr)(address), nullPlayer);
                    }
                    else 
                    {
                        players[i] = nullPlayer;
                    }
                }


                //Triggerbot
                if (keyUtils.KeyIsDown(WinAPI.VirtualKeyShort.MBUTTON))
                {
                    if (localPlayer.m_iCrosshairIdx > 0 && localPlayer.m_iCrosshairIdx <= players.Length)
                    {
                        CSGOPlayer target = players[localPlayer.m_iCrosshairIdx - 1];
                        if (target.IsValid())
                        {
                            if (target.m_iTeam != localPlayer.m_iTeam)
                            {
                                Console.WriteLine("*Shoot*");
                                WinAPI.mouse_event(WinAPI.MOUSEEVENTF.LEFTDOWN, 0, 0, 0, 0);
                                Thread.Sleep(10);
                                WinAPI.mouse_event(WinAPI.MOUSEEVENTF.LEFTUP, 0, 0, 0, 0);
                                Thread.Sleep(10);
                            }
                        }
                    }
                }

                //RCS
                if (m_bRCSEnabled)
                {
                    if (localPlayer.m_iShotsFired > 0)
                    {
                        Vector3 currentPunch = localPlayer.m_vecPunch - vecPunch;
                        Vector3 viewAngles = MemUtils.Read<Vector3>((IntPtr)(setViewAnglesAddress), Vector3.Zero);
                        Vector3 newViewAngles = viewAngles - (m_bRCSComplete ? currentPunch * 2f : currentPunch);
                        newViewAngles = MathUtils.ClampAngle(newViewAngles);
                        MemUtils.Write<Vector3>((IntPtr)(setViewAnglesAddress), newViewAngles);
                    }
                    vecPunch = localPlayer.m_vecPunch;
                }

                //Bunnyhop
                if(m_bBunnyhop)
                {
                    if(keyUtils.KeyIsDown(WinAPI.VirtualKeyShort.SPACE))
                    {
                        if ((localPlayer.m_iFlags & 1) == 1) //Stands (FL_ONGROUND)
                            MemUtils.Write<int>((IntPtr)(clientDllBase + offsetJump), 5);
                        else
                            MemUtils.Write<int>((IntPtr)(clientDllBase + offsetJump), 4);
                    }
                }
                Thread.Sleep((int)(1000f / 60f));
            }
        }

        #region HELPER-METHODS
        private static T GetStructure<T>(byte[] data)
        {
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            T structure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return structure;
        }
        private static T GetStructure<T>(byte[] data, int offset, int length)
        {
            byte[] dt = new byte[length];
            Array.Copy(data, offset, dt, 0, length);
            return GetStructure<T>(dt);
        }
        #endregion
    }
}
