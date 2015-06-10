using ClickerHeroes.UI;
using ExternalUtilsCSharp;
using ExternalUtilsCSharp.SharpDXRenderer;
using ExternalUtilsCSharp.SharpDXRenderer.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ExternalUtilsCSharp.UI;
using SharpDX;
using SharpDX.DirectWrite;

namespace ClickerHeroes
{
    class WithOverlay
    {
        private static ProcUtils proc;
        private static KeyUtils keys;

        private static SharpDXOverlay overlay;
        private static SharpDXPanel pnlPanel;
        private static SharpDXPanel pnlControlsPanel;
        private static SharpDXButton btnToggleMenu;
        private static SharpDXCheckBox chbAdvanceLevel;
        private static SharpDXCheckBox chbAutoClicker;
        private static SharpDXCheckBox chbAutoSpells;
        private static SharpDXLabel lblCaption;
        private static SharpDXLabel lblDescription;
        private static SharpDXLabel lblMenu;
        private static Segments segments;
        
        [STAThread]
        public static void Main(string[] args)
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            pnlPanel = new SharpDXPanel();
            pnlPanel.X = 2;
            pnlPanel.Y = 2;

            lblCaption = new SharpDXLabel();
            lblCaption.Text = "ClickerHeroes";
            lblDescription = new SharpDXLabel();
            lblDescription.Text = "A sample of ExternalUtilsCSharp";
            lblMenu = new SharpDXLabel();
            lblMenu.Text = "Options";

            btnToggleMenu = new SharpDXButton();
            btnToggleMenu.Text = "Toggle menu";
            btnToggleMenu.MouseClickEventUp += btnToggleMenu_MouseClickEventUp;

            pnlControlsPanel = new SharpDXPanel();
            pnlControlsPanel.Visible = false;
            pnlControlsPanel.FillParent = true;

            chbAdvanceLevel = new SharpDXCheckBox();
            chbAdvanceLevel.Text = "Auto level-advancing";
            chbAdvanceLevel.X = 2;
            chbAutoClicker = new SharpDXCheckBox();
            chbAutoClicker.Text = "Auto clicker";
            chbAutoClicker.X = 2;
            chbAutoSpells = new SharpDXCheckBox();
            chbAutoSpells.Text = "Auto spell-casting";
            chbAutoSpells.X = 2;
            segments = new Segments();

            while(true)
            {
                while (!ProcUtils.ProcessIsRunning("Clicker Heroes")) 
                    Thread.Sleep(250);

                proc = new ProcUtils("Clicker Heroes", WinAPI.ProcessAccessFlags.QueryLimitedInformation);
                keys = new KeyUtils();
                using(overlay = new SharpDXOverlay())
                {
                    overlay.ChildControls.Clear();
                    overlay.Attach(proc.Process.MainWindowHandle);
                    overlay.TickEvent += overlay_TickEvent;
                    overlay.DrawOnlyWhenInForeground = false;

                    SharpDXRenderer renderer = overlay.Renderer;
                    renderer.CreateFont("smallFont", "Segoe UI", 12f);
                    renderer.CreateFont("tallFont", "Segoe UI", 16f);

                    lblCaption.Font = renderer.GetFont("tallFont");
                    lblDescription.Font = renderer.GetFont("smallFont");
                    lblMenu.Font = renderer.GetFont("smallFont");
                    btnToggleMenu.Font = renderer.GetFont("smallFont");
                    chbAdvanceLevel.Font = renderer.GetFont("smallFont");
                    chbAutoClicker.Font = renderer.GetFont("smallFont");
                    chbAutoSpells.Font = renderer.GetFont("smallFont");
                    segments.Width = overlay.Width;
                    segments.Height = overlay.Height;

                    pnlPanel.ChildControls.Clear();
                    pnlPanel.AddChildControl(lblCaption);
                    pnlPanel.AddChildControl(lblDescription);
                    pnlPanel.InsertSpacer();
                    pnlPanel.AddChildControl(btnToggleMenu);
                    pnlPanel.AddChildControl(pnlControlsPanel);

                    pnlControlsPanel.ChildControls.Clear();
                    pnlControlsPanel.AddChildControl(lblMenu);
                    pnlControlsPanel.AddChildControl(chbAdvanceLevel);
                    pnlControlsPanel.AddChildControl(chbAutoClicker);
                    pnlControlsPanel.AddChildControl(chbAutoSpells);
                    overlay.ChildControls.Add(pnlPanel);
                    overlay.ChildControls.Add(segments);

                    System.Windows.Forms.Application.Run(overlay);
                }
            }
        }

        static void btnToggleMenu_MouseClickEventUp(object sender, Control<SharpDXRenderer, Color, Vector2, TextFormat>.MouseClickEventArgs e)
        {
            if (e.LeftButton)
                pnlControlsPanel.Visible = !pnlControlsPanel.Visible;
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
