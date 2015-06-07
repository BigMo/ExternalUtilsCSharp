using ExternalUtilsCSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            CHConfig config = new CHConfig();
            config.ReadSettingsFromFile("chconfig.cfg");
            bool clicker = true,
                drawing = true,
                randomize = false,
                firstRun = true,
                castSpells = true;
            Random random = new Random();
            Point[] trail = new Point[16];
            
            while (!keys.KeyIsDown(WinAPI.VirtualKeyShort.F10))
            {
                Console.Clear();
                Console.WriteLine("Controls:\n" +
                    "F10: Terminate\n" +
                    "F9: Toggle auto-clicker\n" +
                    "F8: Toggle drawing\n" +
                    "F7: Toggle randomization\n" + 
                    "Num9/Num6: Increase/decrease clicker-offset (x)\n" +
                    "Num8/Num5: Increase/decrease clicker-offset (y)\n" +
                    "F6: Save window-size and -position and clicker-offsets\n" +
                    "F5: Apply saved window-size and -position to game-window\n" +
                    "F4: Toggle spell-casting");

                firstRun = true;
                Console.WriteLine("Wait for ClickerHeroes to start...");
                while (!ProcUtils.ProcessIsRunning("Clicker Heroes")) { Thread.Sleep(500); }
                proc = new ProcUtils("Clicker Heroes", WinAPI.ProcessAccessFlags.VirtualMemoryOperation);

                Console.WriteLine("Wait for ClickerHeroes' window to show up...");
                while (proc.Process.MainWindowHandle == IntPtr.Zero) { Thread.Sleep(500); }
                while (ProcUtils.ProcessIsRunning("Clicker Heroes"))
                {
                    Thread.Sleep(8);

                    keys.Update();
                    if (keys.KeyWentUp(WinAPI.VirtualKeyShort.F9))
                        clicker = !clicker;
                    if (keys.KeyWentUp(WinAPI.VirtualKeyShort.F8))
                        drawing = !drawing;
                    if (keys.KeyWentUp(WinAPI.VirtualKeyShort.F7))
                        randomize = !randomize;
                    if (keys.KeyWentUp(WinAPI.VirtualKeyShort.F4))
                        castSpells = !castSpells;

                    if (keys.KeyWentUp(WinAPI.VirtualKeyShort.NUMPAD8))
                        config.SetValue("offsetY", config.GetValue<int>("offsetY") + 1);
                    if (keys.KeyWentUp(WinAPI.VirtualKeyShort.NUMPAD5))
                        config.SetValue("offsetY", config.GetValue<int>("offsetY") - 1);
                    if (keys.KeyWentUp(WinAPI.VirtualKeyShort.NUMPAD9))
                        config.SetValue("offsetX", config.GetValue<int>("offsetX") + 1);
                    if (keys.KeyWentUp(WinAPI.VirtualKeyShort.NUMPAD6))
                        config.SetValue("offsetY", config.GetValue<int>("offsetY") + 1);
                    if (keys.KeyWentUp(WinAPI.VirtualKeyShort.F5))
                    {
                        firstRun = true;
                        config.ReadSettingsFromFile("chconfig.cfg");
                    }

                    WinAPI.WINDOWINFO info = new WinAPI.WINDOWINFO();
                    if (!WinAPI.GetWindowInfo(proc.Process.MainWindowHandle, ref info))
                        continue;

                    if (firstRun)
                    {
                        if (config.GetValue<int>("windowHeight") != 0 || config.GetValue<int>("windowWidth") != 0 || config.GetValue<int>("windowX") != 0 || config.GetValue<int>("windowY") != 0)
                        {
                            WinAPI.SetWindowPos(proc.Process.MainWindowHandle, IntPtr.Zero, config.GetValue<int>("windowX"), config.GetValue<int>("windowY"), config.GetValue<int>("windowWidth"), config.GetValue<int>("windowHeight"), 0);
                        }

                        for (int i = 0; i < trail.Length; i++)
                            trail[i] = new Point(0, 0);

                        firstRun = false;
                    }
                    if (keys.KeyWentUp(WinAPI.VirtualKeyShort.F6))
                    {
                        config.SetValue("windowWidth", info.rcWindow.Right - info.rcWindow.Left);
                        config.SetValue("windowHeight", info.rcWindow.Bottom - info.rcWindow.Top);
                        config.SetValue("windowX", info.rcWindow.Left);
                        config.SetValue("windowY", info.rcWindow.Top);
                        config.SaveSettingsToFile("chconfig.cfg");
                    }

                    int width = info.rcClient.Right - info.rcClient.Left;
                    int height = info.rcClient.Bottom - info.rcClient.Top;
                    int sin = (int)(Math.Sin(DateTime.Now.TimeOfDay.TotalSeconds * 10) * width * 0.06);
                    int cos = (int)(Math.Cos(DateTime.Now.TimeOfDay.TotalSeconds * 10) * height * 0.07);
                    int randomD = (int)Math.Sqrt(Math.Sqrt(width * height));
                    int click_x = (int)(width * 0.725) + sin + config.GetValue<int>("offsetX");
                    int click_y = (int)(height * 0.55) + cos + config.GetValue<int>("offsetY");

                    if (randomize)
                    {
                        random = new Random(random.Next(0, (int)Environment.TickCount));
                        click_x += random.Next(0, randomD) * (random.Next(0, 2) == 1 ? 1 : -1);
                        click_y += random.Next(0, randomD) * (random.Next(0, 2) == 1 ? 1 : -1);
                    }

                    Point[] tmp = new Point[trail.Length];
                    Array.Copy(trail, 1, tmp, 0, trail.Length - 1);
                    trail = tmp;
                    trail[trail.Length - 1] = new Point(click_x, click_y);

                    if (clicker)
                    {

                        int lParam = MakeLParam(click_x, click_y);
                        int wParam = 0;
                        WinAPI.SendMessage(proc.Process.MainWindowHandle, (uint)WinAPI.WindowMessage.WM_LBUTTONDOWN, wParam, lParam);
                        WinAPI.SendMessage(proc.Process.MainWindowHandle, (uint)WinAPI.WindowMessage.WM_LBUTTONUP, wParam, lParam);
                    }

                    if (castSpells)
                    {
                        for (uint i = 0; i < 10; i++)
                        {
                            uint key = (uint)WinAPI.VirtualKeyShort.KEY_0 + i;
                            uint scanCode = WinAPI.MapVirtualKey(key, 0);
                            uint lParam = lParam = (0x00000001 | (scanCode << 16));

                            WinAPI.SendMessage(proc.Process.MainWindowHandle, (uint)WinAPI.WindowMessage.WM_KEYDOWN, (int)key, (int)lParam);
                            WinAPI.SendMessage(proc.Process.MainWindowHandle, (uint)WinAPI.WindowMessage.WM_KEYUP, (int)key, (int)lParam);
                        }
                    }

                    if (drawing)
                    {
                        try
                        {
                            using (Graphics g = Graphics.FromHwnd(proc.Process.MainWindowHandle))
                            {
                                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                                g.DrawRectangle(Pens.Black, width / 2f, 0, width / 2f, height);

                                StringBuilder builder = new StringBuilder();
                                builder.AppendLine("ClickerHeroes - ExternalUtilsCSharp");
                                builder.AppendFormat("Autoclicker: {0}\n", clicker.ToString());
                                builder.AppendFormat("Randomize: {0}\n", randomize.ToString());
                                builder.AppendFormat("Cast spells: {0}\n", castSpells.ToString());
                                builder.AppendFormat("Offsets (x,y): {0} {1}\n", config.GetValue<int>("offsetX").ToString(), config.GetValue<int>("offsetY").ToString());
                                builder.AppendFormat("Current Window size (w,h): {0} {1}\n", (info.rcWindow.Right - info.rcWindow.Left).ToString(), (info.rcWindow.Bottom - info.rcWindow.Top).ToString());
                                builder.AppendFormat("Current Window coords (x,y): {0} {1}\n", info.rcWindow.Left.ToString(), info.rcWindow.Top.ToString());
                                builder.AppendFormat("Saved Window size (w,h): {0} {1}\n", config.GetValue<int>("windowWidth").ToString(), config.GetValue<int>("windowHeight").ToString());
                                builder.AppendFormat("Saved Window coords (x,y): {0} {1}", config.GetValue<int>("windowX").ToString(), config.GetValue<int>("windowY").ToString());

                                using (Font fnt = new Font("Courier New", 8))
                                {
                                    string text = builder.ToString();
                                    SizeF size = g.MeasureString(text, fnt);
                                    g.FillRectangle(Brushes.Black, width / 2f, 0, size.Width + 8, size.Height + 8);
                                    g.DrawString(text, fnt, Brushes.Red, width / 2f + 4, 4);
                                }

                                for (int i = trail.Length - 1; i >= 1; i--)
                                {
                                    if (trail[i].X != 0 && trail[i].Y != 0 && trail[i - 1].X != 0 && trail[i - 1].Y != 0)
                                    {
                                        g.DrawLine(Pens.Red, trail[i], trail[i - 1]);
                                    }
                                }
                                g.FillEllipse(Brushes.Red, click_x - 8, click_y - 8, 16, 16);
                            }
                        }catch(Exception ex)
                        {
                            Console.WriteLine("Drawing failed: {0}", ex.Message);
                        }
                    }
                }
            }
        }
        public static int MakeLParam(int LoWord, int HiWord)
        {
            return ((HiWord << 16) | (LoWord & 0xffff));
        }
    }
}
