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
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            while(true)
            {
                while (!ProcUtils.ProcessIsRunning("Clicker Heroes")) 
                    Thread.Sleep(250);
                proc = new ProcUtils("Clicker Heroes", WinAPI.ProcessAccessFlags.QueryLimitedInformation);
                keys = new KeyUtils();
                using(SharpDXOverlay overlay = new SharpDXOverlay())
                {
                    overlay.Attach(proc.Process.MainWindowHandle);
                    overlay.TickEvent += overlay_TickEvent;
                    overlay.DrawEvent += overlay_DrawEvent;
                    overlay.DrawOnlyWhenInForeground = false;

                    Application.Run(overlay);
                }
            }
        }

        static void overlay_DrawEvent(object sender, ExternalUtilsCSharp.UI.Overlay<SharpDX.Color, SharpDX.Vector2, SharpDX.DirectWrite.TextFormat>.OverlayEventArgs e)
        {
            e.Overlay.Renderer.BeginDraw();
            e.Overlay.Renderer.Clear(SharpDX.Color.Transparent);
            e.Overlay.Renderer.DrawEllipse(SharpDX.Color.Red, new SharpDX.Vector2(0, 0), new SharpDX.Vector2(e.Overlay.Width, e.Overlay.Height));
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
