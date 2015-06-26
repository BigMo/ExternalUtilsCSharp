using ExternalUtilsCSharp;
using ExternalUtilsCSharp.SharpDXRenderer;
using ExternalUtilsCSharp.SharpDXRenderer.Controls;
using SharpDX;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExternalUtilsCSharp.InputUtils;

namespace SteamMonsterGame
{
    class WithOverlay
    {
        private static ProcUtils proc;
        private static InputUtilities keys;
        private static Vector2 lastClickerPos;
        private static IntPtr hWnd;

        private static SharpDXOverlay overlay;
        private static SharpDXPanel pnlPanel;
        private static SharpDXLabel lblCaption;
        private static SharpDXLabel lblDescription;
        private static SharpDXButton btnToggleMenu;

        private static SharpDXWindow wndWindow;
        private static SharpDXLabel lblAutomation;
        private static SharpDXCheckBox chbAutoClicker;
        private static SharpDXCheckBox chbMoveMouse;
        private static SharpDXRadioButton rdbUseSend;
        private static SharpDXRadioButton rdbUsePost;
        private static SharpDXLabel lblVisuals;
        private static SharpDXCheckBox chbVisDrawClicker;
        private static SharpDXLabel lblPerformance;
        private static SharpDXLabel lblFpsLogic;
        private static SharpDXProgressBar pgbFpsLogic;
        private static SharpDXLabel lblFpsLogicAverage;
        private static SharpDXLabel lblFpsDraw;
        private static SharpDXProgressBar pgbFpsDraw;
        private static SharpDXLabel lblFpsDrawAverage;

        private static ClickerWindow clkWindow;

        [STAThread]
        public static void Main(string[] args)
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            Console.Title = "SteamMonsterGame";

            Console.WriteLine("> Waiting for steam to start up...");
            while (!ProcUtils.ProcessIsRunning("Steam"))
                Thread.Sleep(250);

            proc = new ProcUtils("Steam", WinAPI.ProcessAccessFlags.QueryLimitedInformation);
            hWnd = IntPtr.Zero;

            Console.WriteLine("> Waiting for steam-window to start up...");
            do
                hWnd = WinAPI.FindWindowByCaption(hWnd, "Steam");
            while (hWnd == IntPtr.Zero);

            Console.WriteLine("> Initializing utils");
            keys = new InputUtilities();
            lastClickerPos = new Vector2();

            Console.WriteLine("> Initializing overlay");
            using (overlay = new SharpDXOverlay())
            {
                overlay.ChildControls.Clear();
                Console.WriteLine("> Attaching overlay");
                overlay.Attach(hWnd);
                overlay.TickEvent += overlay_TickEvent;
                overlay.DrawOnlyWhenInForeground = false;
                overlay.BeforeDrawingEvent += overlay_BeforeDrawingEvent;

                Console.WriteLine("> Setting up fonts");
                SharpDXRenderer renderer = overlay.Renderer;
                renderer.CreateFont("smallFont", "Century Gothic", 12f);
                renderer.CreateFont("tallFont", "Century Gothic", 16f);

                Console.WriteLine("> Initializing controls");
                InitializeComponent();

                Console.WriteLine("> Setting up controls");
                lblCaption.Font = renderer.GetFont("tallFont");
                lblDescription.Font = renderer.GetFont("smallFont");
                btnToggleMenu.Font = renderer.GetFont("smallFont");

                wndWindow.Font = renderer.GetFont("tallFont");
                lblAutomation.Font = renderer.GetFont("smallFont");
                chbAutoClicker.Font = renderer.GetFont("smallFont");
                chbMoveMouse.Font = renderer.GetFont("smallFont");
                rdbUsePost.Font = renderer.GetFont("smallFont");
                rdbUseSend.Font = renderer.GetFont("smallFont");
                lblVisuals.Font = renderer.GetFont("smallFont");
                chbVisDrawClicker.Font = renderer.GetFont("smallFont");
                lblPerformance.Font = renderer.GetFont("smallFont");
                lblFpsLogic.Font = renderer.GetFont("smallFont");
                lblFpsLogicAverage.Font = renderer.GetFont("smallFont");
                lblFpsDraw.Font = renderer.GetFont("smallFont");
                lblFpsDrawAverage.Font = renderer.GetFont("smallFont");

                pnlPanel.ChildControls.Clear();
                pnlPanel.AddChildControl(lblCaption);
                pnlPanel.AddChildControl(lblDescription);
                pnlPanel.InsertSpacer();
                pnlPanel.AddChildControl(btnToggleMenu);

                wndWindow.Panel.AddChildControl(lblAutomation);
                wndWindow.Panel.InsertSpacer();
                wndWindow.Panel.AddChildControl(chbAutoClicker);
                wndWindow.Panel.AddChildControl(chbMoveMouse);
                wndWindow.Panel.AddChildControl(rdbUsePost);
                wndWindow.Panel.AddChildControl(rdbUseSend);

                wndWindow.Panel.InsertSpacer();
                wndWindow.Panel.AddChildControl(lblVisuals);
                wndWindow.Panel.InsertSpacer();
                wndWindow.Panel.AddChildControl(chbVisDrawClicker);

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
                overlay.ChildControls.Add(wndWindow);
                overlay.ChildControls.Add(clkWindow);

                Console.WriteLine("> Running overlay (close this console to terminate!)");
                System.Windows.Forms.Application.Run(overlay);
            }
        }
        private static void InitializeComponent()
        {
            pnlPanel = new SharpDXPanel();
            pnlPanel.X = 2;
            pnlPanel.Y = 2;

            lblCaption = new SharpDXLabel();
            lblCaption.Text = "SteamMonsterGame";
            lblDescription = new SharpDXLabel();
            lblDescription.Text = "A sample of ExternalUtilsCSharp";

            btnToggleMenu = new SharpDXButton();
            btnToggleMenu.Text = "Toggle configuration-window";
            btnToggleMenu.MouseClickEventUp += btnToggleMenu_MouseClickEventUp;

            wndWindow = new SharpDXWindow();
            wndWindow.Text = "Configuration";
            wndWindow.Width = 400;
            wndWindow.Height = 200;
            wndWindow.X = 500;
            wndWindow.Y = 500;
            wndWindow.Visible = false;

            lblAutomation = new SharpDXLabel();
            lblAutomation.Text = "~ Automation ~";
            lblAutomation.FixedWidth = true;
            lblAutomation.Width = 150;
            lblAutomation.TextAlign = SharpDXLabel.TextAlignment.Center;
            chbAutoClicker = new SharpDXCheckBox();
            chbAutoClicker.Text = "[INS] Auto clicker";
            chbMoveMouse = new SharpDXCheckBox();
            chbMoveMouse.Text = "[DEL] Move mouse";
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

            clkWindow = new ClickerWindow();
            clkWindow.X = 500;
            clkWindow.Y = 500;
            clkWindow.Width = 500;
            clkWindow.Height = 500;

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
            if (e.Button== MouseButtons.Left)
                wndWindow.Visible = !wndWindow.Visible;
        }
        private static void overlay_TickEvent(object sender, SharpDXOverlay.DeltaEventArgs e)
        {
            keys.Update();
            pnlPanel.Y = overlay.Location.Y + overlay.Height / 2f - pnlPanel.Height;

            if (keys.keyUtils.KeyWentUp(WinAPI.VirtualKeyShort.INSERT))
                chbAutoClicker.Checked = !chbAutoClicker.Checked;
            if (keys.keyUtils.KeyWentUp(WinAPI.VirtualKeyShort.DELETE))
                chbMoveMouse.Checked = !chbMoveMouse.Checked;

            overlay.UpdateControls(e.SecondsElapsed, keys);

            if (keys.keyUtils.KeyIsDown(WinAPI.VirtualKeyShort.END))
                e.Overlay.Close();

            #region AutoClicker
            if (chbAutoClicker.Checked)
            {
                //Click-area
                ExternalUtilsCSharp.MathObjects.Vector2 areaSize = new ExternalUtilsCSharp.MathObjects.Vector2(clkWindow.GetSize().X, clkWindow.GetSize().Y);
                ExternalUtilsCSharp.MathObjects.Vector2 location = new ExternalUtilsCSharp.MathObjects.Vector2(clkWindow.GetAbsoluteLocation().X, clkWindow.GetAbsoluteLocation().Y);
                ExternalUtilsCSharp.MathObjects.Vector2 areaCenter = location + areaSize * 0.5f;
                bool sec = DateTime.Now.Second % 2 == 0;
                ExternalUtilsCSharp.MathObjects.Vector2 areaTop = new ExternalUtilsCSharp.MathObjects.Vector2(areaCenter.X, areaCenter.Y - areaSize.Y / (sec ? 2f : 4f));
                ExternalUtilsCSharp.MathObjects.Vector2 clickPoint = areaCenter;
                if (chbMoveMouse.Checked)
                {
                    clickPoint = MathUtils.RotatePoint(areaTop, areaCenter, DateTime.Now.Millisecond % 360);
                    WinAPI.SetCursorPos((int)(overlay.Location.X + clickPoint.X), (int)(overlay.Location.Y + clickPoint.Y));
                }
                int lParam = WinAPI.MakeLParam((int)clickPoint.X, (int)clickPoint.Y);
                lastClickerPos.X = clickPoint.X;
                lastClickerPos.Y = clickPoint.Y;
                Message(hWnd, (uint)WinAPI.WindowMessage.WM_LBUTTONDOWN, 0, lParam);
                Message(hWnd, (uint)WinAPI.WindowMessage.WM_LBUTTONUP, 0, lParam);
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
