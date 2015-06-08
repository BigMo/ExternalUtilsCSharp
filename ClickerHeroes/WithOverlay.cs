using ClickerHeroes.UI;
using ExternalUtilsCSharp;
using ExternalUtilsCSharp.SharpDXRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ExternalUtilsCSharp.UI.Controls;
using SharpDX;
using SharpDX.DirectWrite;
using ExternalUtilsCSharp.UI;

namespace ClickerHeroes
{
    class WithOverlay
    {
        private static ProcUtils proc;
        private static KeyUtils keys;

        private static SharpDXOverlay overlay;
        private static CHCheckBox chbAdvanceLevel;
        private static CHCheckBox chbAutoClicker;
        private static CHCheckBox chbAutoSpells;
        private static Segments segments;
        
        [STAThread]
        public static void Main(string[] args)
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            chbAdvanceLevel = new CHCheckBox();
            chbAdvanceLevel.Text = "Auto level-advancing";
            chbAutoClicker = new CHCheckBox();
            chbAutoClicker.Text = "Auto clicker";
            chbAutoClicker.Y = 14f;
            chbAutoSpells = new CHCheckBox();
            chbAutoSpells.Text = "Auto spell-casting";
            chbAutoSpells.Y = 28f;
            segments = new Segments();

            while(true)
            {
                while (!ProcUtils.ProcessIsRunning("Clicker Heroes")) 
                    Thread.Sleep(250);

                proc = new ProcUtils("Clicker Heroes", WinAPI.ProcessAccessFlags.QueryLimitedInformation);
                keys = new KeyUtils();
                using(overlay = new SharpDXOverlay())
                {
                    overlay.Attach(proc.Process.MainWindowHandle);
                    overlay.TickEvent += overlay_TickEvent;
                    overlay.DrawOnlyWhenInForeground = false;

                    SharpDXRenderer renderer = overlay.Renderer;
                    renderer.CreateFont("font1", "Courier New", 10f);

                    chbAdvanceLevel.Font = renderer.GetFont("font1");
                    chbAutoClicker.Font = renderer.GetFont("font1");
                    chbAutoSpells.Font = renderer.GetFont("font1");
                    segments.Width = overlay.Width;
                    segments.Height = overlay.Height;

                    overlay.ChildControls.Add(chbAdvanceLevel);
                    overlay.ChildControls.Add(chbAutoClicker);
                    overlay.ChildControls.Add(chbAutoSpells);
                    overlay.ChildControls.Add(segments);

                    System.Windows.Forms.Application.Run(overlay);
                }
            }
        }

        static void overlay_DrawEvent(object sender, ExternalUtilsCSharp.UI.Overlay<SharpDXRenderer, SharpDX.Color, SharpDX.Vector2, SharpDX.DirectWrite.TextFormat>.OverlayEventArgs e)
        {

        }

        static void overlay_TickEvent(object sender, ExternalUtilsCSharp.UI.Overlay<SharpDXRenderer, SharpDX.Color, SharpDX.Vector2, SharpDX.DirectWrite.TextFormat>.DeltaEventArgs e)
        {
            keys.Update();
            overlay.UpdateControls(e.SecondsElapsed, keys);
            segments.Width = overlay.Width;
            segments.Height = overlay.Height;
            if (keys.KeyIsDown(WinAPI.VirtualKeyShort.INSERT))
                e.Overlay.Close();
        }
    }
}
