using ClickerHeroes.UI;
using ExternalUtilsCSharp;
using ExternalUtilsCSharp.SharpDXRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClickerHeroes
{
    class WithOverlay
    {
        private static ProcUtils proc;
        private static KeyUtils keys;

        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            while(true)
            {
                while (!ProcUtils.ProcessIsRunning("Clicker Heroes")) 
                    Thread.Sleep(250);
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

                proc = new ProcUtils("Clicker Heroes", WinAPI.ProcessAccessFlags.QueryLimitedInformation);
                keys = new KeyUtils();
                using(SharpDXOverlay overlay = new SharpDXOverlay())
                {
                    overlay.Attach(proc.Process.MainWindowHandle);
                    overlay.TickEvent += overlay_TickEvent;
                    overlay.DrawOnlyWhenInForeground = false;

                    SharpDXRenderer renderer = (SharpDXRenderer)((SharpDXOverlay)overlay).Renderer;
                    renderer.CreateFont("font1", "Courier New", 10f);

                    CHCheckBox checkBox = new CHCheckBox(renderer.GetFont("font1"));
                    overlay.ChildControls.Add(checkBox);
                    Application.Run(overlay);
                }
            }
        }

        static void overlay_DrawEvent(object sender, ExternalUtilsCSharp.UI.Overlay<SharpDX.Color, SharpDX.Vector2, SharpDX.DirectWrite.TextFormat>.OverlayEventArgs e)
        {
            e.Overlay.Renderer.BeginDraw();
            e.Overlay.Renderer.Clear(SharpDX.Color.Transparent);
            e.Overlay.Renderer.FillRectangle(
                SharpDX.Color.Red,
                new SharpDX.Vector2(200, 200),
                new SharpDX.Vector2(200, 200));
            e.Overlay.Renderer.EndDraw();
        }

        static void overlay_TickEvent(object sender, ExternalUtilsCSharp.UI.Overlay<SharpDX.Color, SharpDX.Vector2, SharpDX.DirectWrite.TextFormat>.DeltaEventArgs e)
        {
            keys.Update();
            if (keys.KeyIsDown(WinAPI.VirtualKeyShort.INSERT))
                e.Overlay.Close();
        }
    }
}
