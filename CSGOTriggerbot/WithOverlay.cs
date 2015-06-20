using CSGOTriggerbot.CSGOClasses;
using CSGOTriggerbot.UI;
using ExternalUtilsCSharp;
using ExternalUtilsCSharp.MathObjects;
using ExternalUtilsCSharp.SharpDXRenderer;
using ExternalUtilsCSharp.SharpDXRenderer.Controls;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSGOTriggerbot
{
    public class WithOverlay
    {
        #region VARIABLES
        private static KeyUtils keys;
        private static IntPtr hWnd;
        private static double seconds = 0;
        public static Framework Framework;
        public static ProcUtils ProcUtils;
        public static MemUtils MemUtils;
        public static ConfigUtils ConfigUtils;
        #endregion

        #region CONTROLS
        public static SharpDXOverlay SHDXOverlay;

        private static SharpDXCursor cursor;
        //Menu-window
        private static SharpDXWindow windowMenu;
        private static SharpDXLabel labelBoxESPCaption;
        private static SharpDXButton buttonESPToggle;
        private static SharpDXPanel panelESPContent;
        private static SharpDXCheckBox checkBoxESPEnabled;
        private static SharpDXCheckBox checkBoxESPBox;
        private static SharpDXCheckBox checkBoxESPSkeleton;
        private static SharpDXCheckBox checkBoxESPName;
        private static SharpDXCheckBox checkBoxESPHealth;

        private static SharpDXLabel labelBoxAimCaption;
        private static SharpDXButton buttonAimToggle;
        private static SharpDXPanel panelAimContent;
        private static SharpDXCheckBox checkBoxAimEnabled;
        private static SharpDXRadioButton radioAimToggle;
        private static SharpDXRadioButton radioAimHold;
        private static SharpDXTrackbar trackBarAimFov;
        private static SharpDXCheckBox checkBoxAimSmoothEnaled;
        private static SharpDXTrackbar trackBarAimSmoothValue;
        private static SharpDXCheckBox checkBoxAimBone;

        //Performance-window
        private static SharpDXWindow windowGraphs;
        private static SharpDXGraph graphMemRead;
        private static SharpDXGraph graphMemWrite;

        //Others
        private static PlayerRadar ctrlRadar;
        private static PlayerESP[] ctrlPlayerESP;
        #endregion

        #region METHODS
        public static void Main(string[] args)
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            PrintSuccess("[>]=-- Zat's CSGO-ESP");
            keys = new KeyUtils();
            ConfigUtils = new CSGOConfigUtils();

            ConfigUtils.SetValue("espEnabled", true);
            ConfigUtils.SetValue("espBox", true);
            ConfigUtils.SetValue("espSkeleton", true);
            ConfigUtils.SetValue("espName", true);
            ConfigUtils.SetValue("espHealth", true);

            ConfigUtils.SetValue("aimEnabled", true);
            ConfigUtils.SetValue("aimToggle", true);
            ConfigUtils.SetValue("aimHold", true);
            ConfigUtils.SetValue("aimFov", 1f);
            ConfigUtils.SetValue("aimSmoothEnabled", true);
            ConfigUtils.SetValue("aimSmoothValue", 1f);
            ConfigUtils.SetValue("aimBone", 10);
            ConfigUtils.ReadSettingsFromFile("euc_csgo.cfg");

            PrintInfo("> Waiting for CSGO to start up...");
            while (!ProcUtils.ProcessIsRunning("csgo"))
                Thread.Sleep(250);

            ProcUtils = new ProcUtils("csgo", WinAPI.ProcessAccessFlags.VirtualMemoryRead | WinAPI.ProcessAccessFlags.VirtualMemoryWrite | WinAPI.ProcessAccessFlags.VirtualMemoryOperation);
            MemUtils = new ExternalUtilsCSharp.MemUtils();
            MemUtils.Handle = ProcUtils.Handle;

            PrintInfo("> Waiting for CSGOs window to show up...");
            while ((hWnd = WinAPI.FindWindowByCaption(hWnd, "Counter-Strike: Global Offensive")) == IntPtr.Zero)
                Thread.Sleep(250);

            ProcessModule clientDll, engineDll;
            PrintInfo("> Waiting for CSGO to load client.dll...");
            while ((clientDll = ProcUtils.GetModuleByName(@"bin\client.dll")) == null)
                Thread.Sleep(250);
            PrintInfo("> Waiting for CSGO to load engine.dll...");
            while ((engineDll = ProcUtils.GetModuleByName(@"engine.dll")) == null)
                Thread.Sleep(250);

            Framework = new Framework(clientDll, engineDll);

            PrintInfo("> Initializing overlay");
            using (SHDXOverlay = new SharpDXOverlay())
            {
                SHDXOverlay.Attach(hWnd);
                SHDXOverlay.TickEvent += overlay_TickEvent;
                SHDXOverlay.DrawOnlyWhenInForeground = false;

                InitializeComponents();
                SharpDXRenderer renderer = SHDXOverlay.Renderer;
                TextFormat smallFont = renderer.CreateFont("smallFont", "Century Gothic", 10f);
                TextFormat largeFont = renderer.CreateFont("largeFont", "Century Gothic", 14f);
                TextFormat heavyFont = renderer.CreateFont("heavyFont", "Century Gothic", 14f, FontStyle.Normal, FontWeight.Heavy);

                windowMenu.Font = smallFont;
                windowMenu.Caption.Font = largeFont;
                windowGraphs.Font = smallFont;
                windowGraphs.Caption.Font = largeFont;

                graphMemRead.Font = smallFont;
                graphMemWrite.Font = smallFont;
                for (int i = 0; i < ctrlPlayerESP.Length; i++)
                {
                    ctrlPlayerESP[i].Font = heavyFont;
                    SHDXOverlay.ChildControls.Add(ctrlPlayerESP[i]);
                }
                ctrlRadar.Font = smallFont;

                SHDXOverlay.ChildControls.Add(ctrlRadar);
                SHDXOverlay.ChildControls.Add(windowMenu);
                SHDXOverlay.ChildControls.Add(windowGraphs);
                SHDXOverlay.ChildControls.Add(cursor);
                PrintInfo("> Running overlay");
                System.Windows.Forms.Application.Run(SHDXOverlay);
            }
            ConfigUtils.SaveSettingsToFile("euc_csgo.cfg");
        }

        private static void overlay_TickEvent(object sender, SharpDXOverlay.DeltaEventArgs e)
        {
            seconds += e.SecondsElapsed;
            keys.Update();
            Framework.Update();
            if(keys.KeyIsDown(WinAPI.VirtualKeyShort.XBUTTON1))
                Framework.Aimbot();
            SHDXOverlay.UpdateControls(e.SecondsElapsed, keys);

            if (keys.KeyWentUp(WinAPI.VirtualKeyShort.UP))
                ctrlRadar.Scaling -= 0.005f;
            if (keys.KeyWentUp(WinAPI.VirtualKeyShort.DOWN))
                ctrlRadar.Scaling += 0.005f;
            if (keys.KeyWentUp(WinAPI.VirtualKeyShort.INSERT))
                Framework.MouseEnabled = !Framework.MouseEnabled;
            cursor.Visible = !Framework.MouseEnabled;
            if (seconds >= 1)
            {
                seconds = 0;
                graphMemRead.AddValue(MemUtils.BytesRead);
                graphMemWrite.AddValue(MemUtils.BytesWritten);
            }

            ctrlRadar.X = SHDXOverlay.Width - ctrlRadar.Width;

            for (int i = 0; i < ctrlPlayerESP.Length; i++)
                ctrlPlayerESP[i].Visible = false;

            if (Framework.IsPlaying())
            {
                for (int i = 0; i < ctrlPlayerESP.Length && i < Framework.Players.Length; i++)
                {
                    ctrlPlayerESP[i].Visible = true;
                    ctrlPlayerESP[i].Player = Framework.Players[i].Item2;
                }
            }
        }

        private static void InitializeComponents()
        {
            PrintInfo("> Initializing controls");

            cursor = new SharpDXCursor();

            windowGraphs = new SharpDXWindow();
            windowGraphs.Caption.Text = "Performance";
            graphMemRead = new SharpDXGraph();
            graphMemRead.DynamicMaximum = true;
            graphMemRead.Width = 256;
            graphMemRead.Height = 48;
            graphMemRead.Text = "RPM data/s";
            graphMemWrite = new SharpDXGraph();
            graphMemWrite.DynamicMaximum = true;
            graphMemWrite.Width = 256;
            graphMemWrite.Height = 48;
            graphMemWrite.Text = "WPM data/s";

            windowMenu = new SharpDXWindow();
            windowMenu.Caption.Text = "[CSGO] Multihack";
            windowMenu.X = 500;
            windowMenu.Panel.DynamicWidth = false;
            windowMenu.Panel.Width = 200;

            InitLabel(ref labelBoxESPCaption, "~~~ ESP ~~~", true, 150, SharpDXLabel.TextAlignment.Center);
            InitPanel(ref panelESPContent, false, true, true, true);
            InitToggleButton(ref buttonESPToggle, "[Toggle ESP-menu]", panelESPContent);
            InitCheckBox(ref checkBoxESPEnabled, "Enabled", "espEnabled", true);
            InitCheckBox(ref checkBoxESPBox, "Draw box", "espBox", false);
            InitCheckBox(ref checkBoxESPSkeleton, "Draw skeleton", "espSkeleton", true);
            InitCheckBox(ref checkBoxESPName, "Draw name", "espName", false);
            InitCheckBox(ref checkBoxESPHealth, "Draw health", "espHealth", true);

            InitLabel(ref labelBoxAimCaption, "~~~ Aim ~~~", true, 150, SharpDXLabel.TextAlignment.Center);
            InitPanel(ref panelAimContent, false, true, true, true);
            InitToggleButton(ref buttonAimToggle, "[Toggle aim-menu]", panelAimContent);
            InitCheckBox(ref checkBoxAimEnabled, "Enabled", "aimEnabled", true);
            InitTrackBar(ref trackBarAimFov, "Aimbot FOV", "aimFov", 1, 180, 20, 0);
            InitRadioButton(ref radioAimHold, "Mode: Hold key", "aimHold", true);
            InitRadioButton(ref radioAimToggle, "Mode: Toggle", "aimToggle", false);
            InitCheckBox(ref checkBoxAimSmoothEnaled, "Smoothing", "aimSmoothEnabled", true);
            InitTrackBar(ref trackBarAimSmoothValue, "Smooth-factor", "aimSmoothValue", 0, 1, 0.2f, 4);

            windowMenu.Panel.AddChildControl(buttonESPToggle);
            windowMenu.Panel.AddChildControl(panelESPContent);
            windowMenu.Panel.AddChildControl(buttonAimToggle);
            windowMenu.Panel.AddChildControl(panelAimContent);

            panelESPContent.AddChildControl(labelBoxESPCaption);
            panelESPContent.AddChildControl(checkBoxESPEnabled);
            panelESPContent.AddChildControl(checkBoxESPBox);
            panelESPContent.AddChildControl(checkBoxESPSkeleton);
            panelESPContent.AddChildControl(checkBoxESPName);
            panelESPContent.AddChildControl(checkBoxESPHealth);

            panelAimContent.AddChildControl(labelBoxAimCaption);
            panelAimContent.AddChildControl(checkBoxAimEnabled);
            panelAimContent.AddChildControl(trackBarAimFov);
            panelAimContent.AddChildControl(radioAimHold);
            panelAimContent.AddChildControl(radioAimToggle);
            panelAimContent.AddChildControl(checkBoxAimSmoothEnaled);
            panelAimContent.AddChildControl(trackBarAimSmoothValue);

            ctrlRadar = new PlayerRadar();
            ctrlRadar.Width = 128;
            ctrlRadar.Height = 128;
            ctrlRadar.Scaling = 0.02f;
            ctrlRadar.DotRadius = 2f;
            ctrlRadar.Rotating = true;

            ctrlPlayerESP = new PlayerESP[64];
            for (int i = 0; i < ctrlPlayerESP.Length;i++)
            {
                ctrlPlayerESP[i] = new PlayerESP();
                ctrlPlayerESP[i].Visible = false;
            }

            windowGraphs.Panel.AddChildControl(graphMemRead);
            windowGraphs.Panel.AddChildControl(graphMemWrite);          
        }

        static void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            SharpDXCheckBox control = (SharpDXCheckBox)sender;
            ConfigUtils.SetValue(control.Tag.ToString(), control.Checked);
        }
        private static void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            SharpDXRadioButton control = (SharpDXRadioButton)sender;
            ConfigUtils.SetValue(control.Tag.ToString(), control.Checked);
        }
        private static void trackBar_ValueChangedEvent(object sender, EventArgs e)
        {
            SharpDXTrackbar control = (SharpDXTrackbar)sender;
            ConfigUtils.SetValue(control.Tag.ToString(), control.Value);
        }
        static void button_MouseClickEventUp(object sender, ExternalUtilsCSharp.UI.Control<SharpDXRenderer, SharpDX.Color, SharpDX.Vector2, TextFormat>.MouseEventArgs e)
        {
            if (!e.LeftButton)
                return;
            SharpDXPanel panel = (SharpDXPanel)((SharpDXButton)sender).Tag;
            panel.Visible = !panel.Visible;
        }
        #endregion

        #region HELPERS
        private static void InitPanel(ref SharpDXPanel control, bool dynamicWidth = true, bool dynamicHeight = true, bool fillParent = true, bool visible = true)
        {
            control = new SharpDXPanel();
            control.DynamicHeight = dynamicHeight;
            control.DynamicWidth = dynamicWidth;
            control.FillParent = fillParent;
            control.Visible = visible;
        }
        private static void InitToggleButton(ref SharpDXButton control, string text, SharpDXPanel tag)
        {
            control = new SharpDXButton();
            control.Text = text;
            control.Tag = tag;
            control.MouseClickEventUp += button_MouseClickEventUp;
        }
        private static void InitTrackBar(ref SharpDXTrackbar control, string text, object tag, float min =0, float max = 100, float value = 50, int numberofdecimals = 2)
        {
            control = new SharpDXTrackbar();
            control.Text = text;
            control.Tag = tag;
            control.Minimum = min;
            control.Maximum = max;
            control.Value = value;
            control.NumberOfDecimals = numberofdecimals;
            control.ValueChangedEvent += trackBar_ValueChangedEvent;
        }

        private static void InitRadioButton(ref SharpDXRadioButton control, string text, object tag, bool bChecked)
        {
            control = new SharpDXRadioButton();
            control.Text = text;
            control.Tag = tag;
            control.Checked = bChecked;
            control.CheckedChangedEvent += radioButton_CheckedChanged;
        }
        private static void InitLabel(ref SharpDXLabel control, string text, bool fixedWidth = false, float width = 0f, SharpDXLabel.TextAlignment alignment = SharpDXLabel.TextAlignment.Left)
        {
            control = new SharpDXLabel();
            control.FixedWidth = fixedWidth;
            control.Width = width;
            control.TextAlign = alignment;
            control.Text = text;
            control.Tag = null;
        }
        private static void InitCheckBox(ref SharpDXCheckBox control, string text, object tag, bool bChecked)
        {
            control = new SharpDXCheckBox();
            control.Text = text;
            control.Tag = tag;
            control.Checked = bChecked;
            control.CheckedChangedEvent += checkBox_CheckedChanged;
        }
        private static void PrintInfo(string text, params object[] arguments)
        {
            PrintEncolored(text, ConsoleColor.White, arguments);
        }
        private static void PrintSuccess(string text, params object[] arguments)
        {
            PrintEncolored(text, ConsoleColor.Green, arguments);
        }
        private static void PrintError(string text, params object[] arguments)
        {
            PrintEncolored(text, ConsoleColor.Red, arguments);
        }
        private static void PrintException(Exception ex)
        {
            PrintError("An Exception occured: {0}\n\"{1}\"\n{2}", ex.GetType().Name, ex.Message, ex.StackTrace);
        }
        private static void PrintEncolored(string text, ConsoleColor color, params object[] arguments)
        {
            ConsoleColor clr = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text, arguments);
            Console.ForegroundColor = clr;
        }
        #endregion
    }
}
