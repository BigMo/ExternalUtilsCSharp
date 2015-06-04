using ExternalUtilsCSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ClickerHeroes
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcUtils proc;
            KeyUtils keys = new KeyUtils();
            bool active = true;
            Random random = new Random();

            Console.WriteLine("Wait for ClickerHeroes to start...");
            while (!ProcUtils.ProcessIsRunning("Clicker Heroes")) { Thread.Sleep(500); }
            proc = new ProcUtils("Clicker Heroes", WinAPI.ProcessAccessFlags.VirtualMemoryOperation);

            Console.WriteLine("Wait for ClickerHeroes' window to show up...");
            while (proc.Process.MainWindowHandle == IntPtr.Zero) { Thread.Sleep(500); }

            Console.WriteLine("Press F10 to terminate, press F9 to toggle on/off");
            while(!keys.KeyIsDown(WinAPI.VirtualKeyShort.F10))
            {
                Thread.Sleep(8);
                //if (WinAPI.GetForegroundWindow() != proc.Process.MainWindowHandle)
                //    continue;

                keys.Update();
                if (keys.KeyWentUp(WinAPI.VirtualKeyShort.F9))
                    active = !active;

                if (!active)
                    continue;

                WinAPI.RECT rect = new WinAPI.RECT();
                if (!WinAPI.GetWindowRect(proc.Process.MainWindowHandle, out rect))
                    continue;

                int width = rect.Right - rect.Left;
                int height = rect.Bottom - rect.Top;
                int sin = (int)(Math.Sin(DateTime.Now.TimeOfDay.TotalSeconds * 10) * width * 0.06);
                int cos = (int)(Math.Cos(DateTime.Now.TimeOfDay.TotalSeconds * 10) * height * 0.07);

                random = new Random(random.Next(0, (int)Environment.TickCount));
                int randomD = (int)Math.Sqrt(Math.Sqrt(width * height));

                int x = /*rect.Left +*/ (int)(width * 0.75) + sin + random.Next(0, randomD) * (random.Next(0,2) == 1 ? 1 : -1);
                int y = /*rect.Top +*/ (int)(height * 0.6) + cos + random.Next(0, randomD) * (random.Next(0, 2) == 1 ? 1 : -1) -20;

                //WinAPI.SetCursorPos(x, y);
                //WinAPI.mouse_event(WinAPI.MOUSEEVENTF.LEFTDOWN, (uint)x, (uint)y, 0, 0);
                //Thread.Sleep(1);
                //WinAPI.mouse_event(WinAPI.MOUSEEVENTF.LEFTUP, (uint)x, (uint)y, 0, 0);

                int lParam = MakeLParam(x, y);
                int wParam = 0;
                WinAPI.SendMessage(proc.Process.MainWindowHandle, 0x201, wParam, lParam);
                Thread.Sleep(1);
                WinAPI.SendMessage(proc.Process.MainWindowHandle, 0x202, wParam, lParam);
            }
        }
        public static int MakeLParam(int LoWord, int HiWord)
        {
            return ((HiWord << 16) | (LoWord & 0xffff));
        }
    }
}
