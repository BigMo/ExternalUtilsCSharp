using CSGOTriggerbot.CSGO.Enums;
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
using System.Windows.Forms;
using ExternalUtilsCSharp.InputUtils;

namespace CSGOTriggerbot
{
    public class WithOverlay
    {
        #region VARIABLES
        public static InputUtilities KeyUtils;
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
        private static SharpDXLabel labelHotkeys;
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
        private static SharpDXCheckBox checkBoxAimFilterSpotted;
        private static SharpDXCheckBox checkBoxAimFilterSpottedBy;
        private static SharpDXCheckBox checkBoxAimFilterEnemies;
        private static SharpDXCheckBox checkBoxAimFilterAllies;
        private static SharpDXRadioButton radioAimToggle;
        private static SharpDXRadioButton radioAimHold;
        private static SharpDXTrackbar trackBarAimFov;
        private static SharpDXCheckBox checkBoxAimSmoothEnaled;
        private static SharpDXTrackbar trackBarAimSmoothValue;
        private static SharpDXButtonKey keyAimKey;

        private static SharpDXLabel labelBoxRCSCaption;
        private static SharpDXButton buttonRCSToggle;
        private static SharpDXPanel panelRCSContent;
        private static SharpDXCheckBox checkBoxRCSEnabled;
        private static SharpDXTrackbar trackBarRCSForce;

        //Performance-window
        private static SharpDXWindow windowGraphs;
        private static SharpDXGraph graphMemRead;
        private static SharpDXGraph graphMemWrite;

        //Spectators-window
        private static SharpDXWindow windowSpectators;
        private static SharpDXLabel labelSpectators;

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
            KeyUtils = new InputUtilities();
            ConfigUtils = new CSGOConfigUtils();

            ConfigUtils.SetValue("espEnabled", true);
            ConfigUtils.SetValue("espBox", false);
            ConfigUtils.SetValue("espSkeleton", true);
            ConfigUtils.SetValue("espName", false);
            ConfigUtils.SetValue("espHealth", true);

            ConfigUtils.SetValue("aimEnabled", true);
            ConfigUtils.SetValue("aimKey", WinAPI.VirtualKeyShort.XBUTTON1);
            ConfigUtils.SetValue("aimToggle", false);
            ConfigUtils.SetValue("aimHold", true);
            ConfigUtils.SetValue("aimFov", 30f);
            ConfigUtils.SetValue("aimSmoothEnabled", true);
            ConfigUtils.SetValue("aimSmoothValue", 0.2f);
            ConfigUtils.SetValue("aimFilterSpotted", false);
            ConfigUtils.SetValue("aimFilterSpottedBy", false);
            ConfigUtils.SetValue("aimFilterEnemies", true);
            ConfigUtils.SetValue("aimFilterAllies", false);

            ConfigUtils.SetValue("rcsEnabled", true);
            ConfigUtils.SetValue("rcsForce", 100f);

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

                InitializeComponents();
                SharpDXRenderer renderer = SHDXOverlay.Renderer;
                TextFormat smallFont = renderer.CreateFont("smallFont", "Century Gothic", 10f);
                TextFormat largeFont = renderer.CreateFont("largeFont", "Century Gothic", 14f);
                TextFormat heavyFont = renderer.CreateFont("heavyFont", "Century Gothic", 14f, FontStyle.Normal, FontWeight.Heavy);

                windowMenu.Font = smallFont;
                windowMenu.Caption.Font = largeFont;
                windowGraphs.Font = smallFont;
                windowGraphs.Caption.Font = largeFont;
                windowSpectators.Font = smallFont;
                windowSpectators.Caption.Font = largeFont;
                graphMemRead.Font = smallFont;
                graphMemWrite.Font = smallFont;
                for (int i = 0; i < ctrlPlayerESP.Length; i++)
                {
                    ctrlPlayerESP[i].Font = heavyFont;
                    SHDXOverlay.ChildControls.Add(ctrlPlayerESP[i]);
                }
                ctrlRadar.Font = smallFont;

                windowMenu.ApplySettings(ConfigUtils);

                SHDXOverlay.ChildControls.Add(ctrlRadar);
                SHDXOverlay.ChildControls.Add(windowMenu);
                SHDXOverlay.ChildControls.Add(windowGraphs);
                SHDXOverlay.ChildControls.Add(windowSpectators);
                SHDXOverlay.ChildControls.Add(cursor);
                PrintInfo("> Running overlay");
                System.Windows.Forms.Application.Run(SHDXOverlay);
            }
            ConfigUtils.SaveSettingsToFile("euc_csgo.cfg");
        }

        private static void overlay_TickEvent(object sender, SharpDXOverlay.DeltaEventArgs e)
        {
            seconds += e.SecondsElapsed;
            KeyUtils.Update();
            Framework.Update();
            SHDXOverlay.UpdateControls(e.SecondsElapsed, KeyUtils);
            if (KeyUtils.Keys.KeyWentUp(WinAPI.VirtualKeyShort.DELETE))
                SHDXOverlay.Kill();
            if (KeyUtils.Keys.KeyWentUp(WinAPI.VirtualKeyShort.UP))
                ctrlRadar.Scaling -= 0.005f;
            if (KeyUtils.Keys.KeyWentUp(WinAPI.VirtualKeyShort.DOWN))
                ctrlRadar.Scaling += 0.005f;
            if (KeyUtils.Keys.KeyWentUp(WinAPI.VirtualKeyShort.INSERT))
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
                if (Framework.LocalPlayer != null)
                {
                    var spectators = Framework.Players.Where(x => x.Item2.m_hObserverTarget == Framework.LocalPlayer.m_iID);
                    StringBuilder builder = new StringBuilder();
                    foreach (Tuple<int, CSPlayer> spec in spectators)
                    {
                        CSPlayer player = spec.Item2;
                        builder.AppendFormat("{0} [{1}]{2}", Framework.Names[player.m_iID], (SpectatorView)player.m_iObserverMode, builder.Length > 0 ? "\n" : "");
                    }
                    if (builder.Length > 0)
                        labelSpectators.Text = builder.ToString();
                    else
                        labelSpectators.Text = "<none>";
                }
                else
                {
                    labelSpectators.Text = "<none>";
                }
            }
            else
            {
                labelSpectators.Text = "<none>";
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

            windowGraphs.Panel.AddChildControl(graphMemRead);
            windowGraphs.Panel.AddChildControl(graphMemWrite);  

            windowMenu = new SharpDXWindow();
            windowMenu.Caption.Text = "[CSGO] Multihack";
            windowMenu.X = 500;
            windowMenu.Panel.DynamicWidth = false;
            windowMenu.Panel.Width = 200;

            InitLabel(ref labelHotkeys, "[INS] Toggle mouse\n[DEL] Terminate hack", false, 150, SharpDXLabel.TextAlignment.Center);

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
            InitButtonKey(ref keyAimKey, "Key", "aimKey");
            InitTrackBar(ref trackBarAimFov, "Aimbot FOV", "aimFov", 1, 180, 20, 0);
            InitRadioButton(ref radioAimHold, "Mode: Hold key", "aimHold", true);
            InitRadioButton(ref radioAimToggle, "Mode: Toggle", "aimToggle", false);
            InitCheckBox(ref checkBoxAimSmoothEnaled, "Smoothing", "aimSmoothEnabled", true);
            InitTrackBar(ref trackBarAimSmoothValue, "Smooth-factor", "aimSmoothValue", 0, 1, 0.2f, 4);
            InitCheckBox(ref checkBoxAimFilterSpotted, "Filter: Spotted by me", "aimFilterSpotted", false);
            InitCheckBox(ref checkBoxAimFilterSpottedBy, "Filter: Spotted me", "aimFilterSpottedBy", false);
            InitCheckBox(ref checkBoxAimFilterEnemies, "Filter: Enemies", "aimFilterEnemies", true);
            InitCheckBox(ref checkBoxAimFilterAllies, "Filter: Allies", "aimFilterAllies", false);

            InitLabel(ref labelBoxRCSCaption, "~~~ RCS ~~~", true, 150, SharpDXLabel.TextAlignment.Center);
            InitPanel(ref panelRCSContent, false, true, true, true);
            InitToggleButton(ref buttonRCSToggle, "[Toggle RCS-menu]", panelRCSContent);
            InitCheckBox(ref checkBoxRCSEnabled, "Enabled", "rcsEnabled", true);
            InitTrackBar(ref trackBarRCSForce, "Force (%)", "rcsForce", 1, 100, 100, 2);


            windowMenu.Panel.AddChildControl(labelHotkeys);
            windowMenu.Panel.AddChildControl(buttonESPToggle);
            windowMenu.Panel.AddChildControl(panelESPContent);
            windowMenu.Panel.AddChildControl(buttonAimToggle);
            windowMenu.Panel.AddChildControl(panelAimContent);
            windowMenu.Panel.AddChildControl(buttonRCSToggle);
            windowMenu.Panel.AddChildControl(panelRCSContent);

            panelESPContent.AddChildControl(labelBoxESPCaption);
            panelESPContent.AddChildControl(checkBoxESPEnabled);
            panelESPContent.AddChildControl(checkBoxESPBox);
            panelESPContent.AddChildControl(checkBoxESPSkeleton);
            panelESPContent.AddChildControl(checkBoxESPName);
            panelESPContent.AddChildControl(checkBoxESPHealth);

            panelAimContent.AddChildControl(labelBoxAimCaption);
            panelAimContent.AddChildControl(checkBoxAimEnabled);
            panelAimContent.AddChildControl(keyAimKey);
            panelAimContent.AddChildControl(trackBarAimFov);
            panelAimContent.AddChildControl(radioAimHold);
            panelAimContent.AddChildControl(radioAimToggle);
            panelAimContent.AddChildControl(checkBoxAimSmoothEnaled);
            panelAimContent.AddChildControl(trackBarAimSmoothValue);
            panelAimContent.AddChildControl(checkBoxAimFilterSpotted);
            panelAimContent.AddChildControl(checkBoxAimFilterSpottedBy);
            panelAimContent.AddChildControl(checkBoxAimFilterEnemies);
            panelAimContent.AddChildControl(checkBoxAimFilterAllies);

            panelRCSContent.AddChildControl(labelBoxRCSCaption);
            panelRCSContent.AddChildControl(checkBoxRCSEnabled);
            panelRCSContent.AddChildControl(trackBarRCSForce);

            windowSpectators = new SharpDXWindow();
            windowSpectators.Caption.Text = "Spectators";
            windowSpectators.Y = 500;
            InitLabel(ref labelSpectators, "<none>", false, 200f, SharpDXLabel.TextAlignment.Left);
            windowSpectators.Panel.AddChildControl(labelSpectators);

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
        static void buttonKey_KeyChangedEvent(object sender, EventArgs e)
        {
            SharpDXButtonKey control = (SharpDXButtonKey)sender;
            ConfigUtils.SetValue(control.Tag.ToString(), control.Key);
        }
        static void button_MouseClickEventUp(object sender, MouseEventExtArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            SharpDXPanel panel = (SharpDXPanel)((SharpDXButton)sender).Tag;
            panel.Visible = !panel.Visible;
        }
        #endregion

        #region HELPERS
        private static void InitButtonKey(ref SharpDXButtonKey control, string text, object tag)
        {
            control = new SharpDXButtonKey();
            control.Text = text;
            control.Tag = tag;
            control.KeyChangedEvent += buttonKey_KeyChangedEvent;
        }
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
