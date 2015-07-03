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
using System.Windows.Forms;
using ExternalUtilsCSharp.InputUtils;
using ExternalUtilsCSharp.UI;
using SharpDX;
using SharpDX.DirectWrite;

namespace ClickerHeroes
{
    class WithOverlay
    {
        private static ProcUtils proc;
        private static InputUtilities keys;
        private static Vector2 lastClickerPos;

        private static SharpDXOverlay overlay;
        private static SharpDXPanel pnlPanel;
        private static SharpDXLabel lblCaption;
        private static SharpDXLabel lblDescription;
        private static SharpDXButton btnToggleMenu;

        private static SharpDXWindow wndWindow;
        private static SharpDXLabel lblAutomation;
        private static SharpDXCheckBox chbAutoClicker;
        private static SharpDXCheckBox chbAutoSpells;
        private static SharpDXRadioButton rdbUseSend;
        private static SharpDXRadioButton rdbUsePost;
        private static SharpDXLabel lblVisuals;
        private static SharpDXCheckBox chbVisDrawClicker;
        private static SharpDXCheckBox chbVisDrawLevels;
        private static SharpDXLabel lblPerformance;
        private static SharpDXLabel lblFpsLogic;
        private static SharpDXProgressBar pgbFpsLogic;
        private static SharpDXLabel lblFpsLogicAverage;
        private static SharpDXLabel lblFpsDraw;
        private static SharpDXProgressBar pgbFpsDraw;
        private static SharpDXLabel lblFpsDrawAverage;
        private static Segments segments;

        [STAThread]
        public static void Main(string[] args)
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            
            while (!ProcUtils.ProcessIsRunning("Clicker Heroes"))
                Thread.Sleep(250);

            InitializeComponent();

            proc = new ProcUtils("Clicker Heroes", WinAPI.ProcessAccessFlags.QueryLimitedInformation);
            keys = new InputUtilities();
            lastClickerPos = new Vector2();
            using (overlay = new SharpDXOverlay())
            {
                overlay.ChildControls.Clear();
                overlay.Attach(proc.Process.MainWindowHandle);
                overlay.TickEvent += overlay_TickEvent;
                overlay.DrawOnlyWhenInForeground = false;
                overlay.BeforeDrawingEvent += overlay_BeforeDrawingEvent;

                SharpDXRenderer renderer = overlay.Renderer;
                renderer.CreateFont("smallFont", "Century Gothic", 12f);
                renderer.CreateFont("tallFont", "Century Gothic", 16f);

                lblCaption.Font = renderer.GetFont("tallFont");
                lblDescription.Font = renderer.GetFont("smallFont");
                btnToggleMenu.Font = renderer.GetFont("smallFont");

                wndWindow.Font = renderer.GetFont("tallFont");
                lblAutomation.Font = renderer.GetFont("smallFont");
                chbAutoClicker.Font = renderer.GetFont("smallFont");
                chbAutoSpells.Font = renderer.GetFont("smallFont");
                rdbUsePost.Font = renderer.GetFont("smallFont");
                rdbUseSend.Font = renderer.GetFont("smallFont");
                lblVisuals.Font = renderer.GetFont("smallFont");
                chbVisDrawClicker.Font = renderer.GetFont("smallFont");
                chbVisDrawLevels.Font = renderer.GetFont("smallFont");
                lblPerformance.Font = renderer.GetFont("smallFont");
                lblFpsLogic.Font = renderer.GetFont("smallFont");
                lblFpsLogicAverage.Font = renderer.GetFont("smallFont");
                lblFpsDraw.Font = renderer.GetFont("smallFont");
                lblFpsDrawAverage.Font = renderer.GetFont("smallFont");

                segments.Width = overlay.Width;
                segments.Height = overlay.Height;

                pnlPanel.ChildControls.Clear();
                pnlPanel.AddChildControl(lblCaption);
                pnlPanel.AddChildControl(lblDescription);
                pnlPanel.InsertSpacer();
                pnlPanel.AddChildControl(btnToggleMenu);

                wndWindow.Panel.AddChildControl(lblAutomation);
                wndWindow.Panel.InsertSpacer();
                wndWindow.Panel.AddChildControl(chbAutoClicker);
                wndWindow.Panel.AddChildControl(chbAutoSpells);
                wndWindow.Panel.AddChildControl(rdbUsePost);
                wndWindow.Panel.AddChildControl(rdbUseSend);

                wndWindow.Panel.InsertSpacer();
                wndWindow.Panel.AddChildControl(lblVisuals);
                wndWindow.Panel.InsertSpacer();
                wndWindow.Panel.AddChildControl(chbVisDrawClicker);
                wndWindow.Panel.AddChildControl(chbVisDrawLevels);

                wndWindow.Panel.InsertSpacer();
                wndWindow.Panel.AddChildControl(lblPerformance);
                wndWindow.Panel.InsertSpacer();
                wndWindow.Panel.AddChildControl(lblFpsLogic);
                wndWindow.Panel.AddChildControl(pgbFpsLogic);
                wndWindow.Panel.AddChildControl(lblFpsLogicAverage);
                wndWindow.Panel.AddChildControl(lblFpsDraw);
                wndWindow.Panel.AddChildControl(pgbFpsDraw);
                wndWindow.Panel.AddChildControl(lblFpsDrawAverage);

                overlay.ChildControls.Add(pnlPanel);
                overlay.ChildControls.Add(segments);
                overlay.ChildControls.Add(wndWindow);
                System.Windows.Forms.Application.Run(overlay);
            }
        }
        private static void InitializeComponent()
        {
            pnlPanel = new SharpDXPanel();
            pnlPanel.X = 2;
            pnlPanel.Y = 2;

            lblCaption = new SharpDXLabel();
            lblCaption.Text = "ClickerHeroes";
            lblDescription = new SharpDXLabel();
            lblDescription.Text = "A sample of ExternalUtilsCSharp";

            btnToggleMenu = new SharpDXButton();
            btnToggleMenu.Text = "Toggle configuration-window";
            btnToggleMenu.MouseClickEventUp += btnToggleMenu_MouseClickEventUp;

            wndWindow = new SharpDXWindow();
            wndWindow.Text = "Configuration";
            wndWindow.Width = 400;
            wndWindow.Height = 200;
            wndWindow.X = 400;
            wndWindow.Visible = false;

            lblAutomation = new SharpDXLabel();
            lblAutomation.Text = "~ Automation ~";
            lblAutomation.FixedWidth = true;
            lblAutomation.Width = 150;
            lblAutomation.TextAlign = SharpDXLabel.TextAlignment.Center;
            chbAutoClicker = new SharpDXCheckBox();
            chbAutoClicker.Text = "Auto clicker";
            chbAutoSpells = new SharpDXCheckBox();
            chbAutoSpells.Text = "Auto spell-casting";
            rdbUsePost = new SharpDXRadioButton();
            rdbUsePost.Text = "[Use PostMessage]";
            rdbUsePost.Checked = true;
            rdbUseSend = new SharpDXRadioButton();
            rdbUseSend.Text = "[Use SendMessage]";
            rdbUseSend.Checked = false;

            lblVisuals = new SharpDXLabel();
            lblVisuals.Text = "~ Visuals ~";
            lblVisuals.FixedWidth = true;
            lblVisuals.Width = 150;
            lblVisuals.TextAlign = SharpDXLabel.TextAlignment.Center;
            chbVisDrawClicker = new SharpDXCheckBox();
            chbVisDrawClicker.Text = "Draw auto-clicker";
            chbVisDrawLevels = new SharpDXCheckBox();
            chbVisDrawLevels.Text = "Draw levels";
            chbVisDrawLevels.CheckedChangedEvent += chbVisDrawLevels_CheckedChangedEvent;
            lblPerformance = new SharpDXLabel();
            lblPerformance.Text = "~ Performance ~";
            lblPerformance.FixedWidth = true;
            lblPerformance.Width = 150;
            lblPerformance.TextAlign = SharpDXLabel.TextAlignment.Center;
            lblFpsLogic = new SharpDXLabel();
            lblFpsLogic.Text = "FPS logic: 0";
            pgbFpsLogic = new SharpDXProgressBar();
            pgbFpsLogic.Maximum = 60;
            lblFpsLogicAverage = new SharpDXLabel();
            lblFpsLogicAverage.Text = "Average FPS: 0 (0 ticks total)";
            lblFpsDraw = new SharpDXLabel();
            lblFpsDraw.Text = "FPS logic: 0";
            pgbFpsDraw = new SharpDXProgressBar();
            pgbFpsDraw.Maximum = 60;
            lblFpsDrawAverage = new SharpDXLabel();
            lblFpsDrawAverage.Text = "Average FPS: 0 (0 ticks total)";

            segments = new Segments();
            segments.Visible = false;
        }
        private static void chbVisDrawLevels_CheckedChangedEvent(object sender, EventArgs e)
        {
            segments.Visible = !segments.Visible;
        }
        private static void overlay_BeforeDrawingEvent(object sender, SharpDXOverlay.OverlayEventArgs e)
        {
            if (chbAutoClicker.Checked && chbVisDrawClicker.Checked)
            {
                e.Overlay.Renderer.FillEllipse(Color.Red, lastClickerPos, new Vector2(16), true);
            }
        }
        private static void btnToggleMenu_MouseClickEventUp(object sender, MouseEventExtArgs e)
        {
            if (e.Button == MouseButtons.Left)
                wndWindow.Visible = !wndWindow.Visible;
        }
        private static void overlay_TickEvent(object sender, SharpDXOverlay.DeltaEventArgs e)
        {
            keys.Update();

            overlay.UpdateControls(e.SecondsElapsed, keys);
            segments.Width = overlay.Width;
            segments.Height = overlay.Height;

            if (keys.Keys.KeyIsDown(WinAPI.VirtualKeyShort.INSERT))
                e.Overlay.Close();

            #region AutoClicker
            if (chbAutoClicker.Checked)
            {
                //Click-area
                ExternalUtilsCSharp.MathObjects.Vector2 areaSize = new ExternalUtilsCSharp.MathObjects.Vector2(overlay.Width / 2f * 0.6f, overlay.Width * 0.15f);
                ExternalUtilsCSharp.MathObjects.Vector2 location = new ExternalUtilsCSharp.MathObjects.Vector2(overlay.Width / 2f + overlay.Width / 4f, overlay.Width / 2f * 0.7f);
                ExternalUtilsCSharp.MathObjects.Vector2 areaCenter = location + areaSize * 0.5f;
                bool sec = DateTime.Now.Second % 2 == 0;
                ExternalUtilsCSharp.MathObjects.Vector2 areaTop = new ExternalUtilsCSharp.MathObjects.Vector2(areaCenter.X, areaCenter.Y - areaSize.Y / (sec ? 2f : 4f));
                ExternalUtilsCSharp.MathObjects.Vector2 clickPoint = MathUtils.RotatePoint(areaTop, areaCenter, DateTime.Now.Millisecond % 360) - areaSize * 0.5f;
                int lParam = WinAPI.MakeLParam((int)clickPoint.X, (int)clickPoint.Y);
                lastClickerPos.X = clickPoint.X;
                lastClickerPos.Y = clickPoint.Y;
                Message(proc.Process.MainWindowHandle, (uint)WinAPI.WindowMessage.WM_LBUTTONDOWN, 0, lParam);
                Message(proc.Process.MainWindowHandle, (uint)WinAPI.WindowMessage.WM_LBUTTONUP, 0, lParam);
            }
            #endregion
            #region Cast spells
            if (chbAutoSpells.Checked && overlay.LogicUpdater.TickCount % 10 == 0)
            {
                for (uint i = 0; i < 10; i++)
                {
                    uint key = (uint)WinAPI.VirtualKeyShort.KEY_0 + i;
                    uint scanCode = WinAPI.MapVirtualKey(key, 0);
                    uint lParam = lParam = (0x00000001 | (scanCode << 16));

                    Message(proc.Process.MainWindowHandle, (uint)WinAPI.WindowMessage.WM_KEYDOWN, (int)key, (int)lParam);
                    Message(proc.Process.MainWindowHandle, (uint)WinAPI.WindowMessage.WM_KEYUP, (int)key, (int)lParam);
                }
            }
            #endregion
            lblFpsLogic.Text = string.Format("FPS logic: {0}", overlay.LogicUpdater.LastFrameRate.ToString());
            lblFpsDraw.Text = string.Format("FPS draw: {0}", overlay.DrawUpdater.LastFrameRate.ToString());
            pgbFpsLogic.Value = overlay.LogicUpdater.LastFrameRate;
            pgbFpsDraw.Value = overlay.DrawUpdater.LastFrameRate;
            lblFpsDrawAverage.Text = string.Format("Average FPS: {0} ({1} ticks in {2}s)", overlay.DrawUpdater.GetAverageFPS(), MiscUtils.GetUnitFromNumber(overlay.DrawUpdater.TickCount, true), ((int)overlay.DrawUpdater.GetRuntime().TotalSeconds).ToString());
            lblFpsLogicAverage.Text = string.Format("Average FPS: {0} ({1} ticks in {2}s)", overlay.LogicUpdater.GetAverageFPS(), MiscUtils.GetUnitFromNumber(overlay.LogicUpdater.TickCount, true), ((int)overlay.LogicUpdater.GetRuntime().TotalSeconds).ToString());
        }
        private static void Message(IntPtr hWnd, uint message, int wParam, int lParam)
        {
            if (rdbUseSend.Checked)
            {
                WinAPI.SendMessage(hWnd, message, wParam, lParam);
            }
            else
            {
                WinAPI.PostMessage(hWnd, message, wParam, lParam);
            }
        }
    }
}
