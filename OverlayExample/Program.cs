using ExternalUtilsCSharp.InputUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using ExternalUtilsCSharp;
using ExternalUtilsCSharp.SharpDXRenderer;
using ExternalUtilsCSharp.SharpDXRenderer.Controls;
using SharpDX.DirectWrite;

namespace OverlayExample
{
    class Program
    {
        public static SharpDXOverlay SHDXOverlay;

        private static SharpDXCursor cursor;
        //Menu-window
        private static SharpDXWindow windowMenu;
            private static SharpDXLabel label;
        private static SharpDXButton buttonToggle;
        private static SharpDXPanel panelContent;
        private static SharpDXCheckBox checkBox;
        static SharpDXTrackbar track;

        private static InputUtilities input;
        static void Main(string[] args)
        {
            Console.WriteLine("Change overlay attached window by pressing F5");
            Console.WriteLine("It will attach to currently active window");
            Console.WriteLine("Trackbar can be controled with mouse wheel");
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            input = new InputUtilities();
            var hWnd = WinAPI.GetForegroundWindow();
            using (SHDXOverlay = new SharpDXOverlay())
            {
                SHDXOverlay.Attach(hWnd);
                SHDXOverlay.TickEvent += overlay_TickEvent;
                InitializeComponents();
                SharpDXRenderer renderer = SHDXOverlay.Renderer;
                TextFormat smallFont = renderer.CreateFont("smallFont", "Century Gothic", 10f);
                TextFormat largeFont = renderer.CreateFont("largeFont", "Century Gothic", 14f);
                TextFormat heavyFont = renderer.CreateFont("heavyFont", "Century Gothic", 14f, SharpDX.DirectWrite.FontStyle.Normal, FontWeight.Heavy);
                windowMenu.Font = smallFont;
                windowMenu.Caption.Font = largeFont;
                SHDXOverlay.ChildControls.Add(windowMenu);

                System.Windows.Forms.Application.Run(SHDXOverlay);
            }


        }

        private static void InitializeComponents()
        {
            windowMenu = new SharpDXWindow();
            windowMenu.Caption.Text = "Overlay sample";
            windowMenu.X = 0;
            windowMenu.Panel.DynamicWidth = true;
            windowMenu.Panel.Width = 200;
            windowMenu.Panel.Height = 200;
            InitPanel(ref panelContent);
            InitLabel(ref label,"EXAMPLE",true,200);
            InitCheckBox(ref checkBox,"Checkbox","lel",true);
            InitTrackBar(ref track,"Trackbar","");
            panelContent.AddChildControl(label);
            panelContent.InsertSpacer();
            panelContent.AddChildControl(checkBox);
            panelContent.InsertSpacer();
            panelContent.AddChildControl(track);
            windowMenu.Panel.AddChildControl(panelContent);

        }                
        private static void overlay_TickEvent(object sender, SharpDXOverlay.DeltaEventArgs e)
        {
            var overlay = (SharpDXOverlay) sender;
            input.Update();
            if(input.Keys.KeyWentUp(WinAPI.VirtualKeyShort.F5))
            {             
                overlay.ChangeHandle(WinAPI.GetForegroundWindow());
            }
            overlay.UpdateControls(e.SecondsElapsed, input);

            if (input.Keys.KeyIsDown(WinAPI.VirtualKeyShort.END))
                e.Overlay.Close();
           // e.Overlay.ShowInactiveTopmost();

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
            control.MouseClickEventUp += (sender, e) =>
            {
                if (e.Wheel)
                    ((SharpDXCheckBox)sender).Checked = !((SharpDXCheckBox)sender).Checked;
            };
        }
        private static void InitPanel(ref SharpDXPanel control, bool dynamicWidth = true, bool dynamicHeight = true, bool fillParent = true, bool visible = true)
        {
            control = new SharpDXPanel();
            control.DynamicHeight = dynamicHeight;
            control.DynamicWidth = dynamicWidth;
            control.FillParent = fillParent;
            control.Visible = visible;
        }
        private static void InitTrackBar(ref SharpDXTrackbar control, string text, object tag, float min = 0, float max = 100, float value = 50, int numberofdecimals = 2)
        {
            control = new SharpDXTrackbar();
            control.Text = text;
            control.Tag = tag;
            control.Minimum = min;
            control.Maximum = max;
            control.Value = value;
            control.NumberOfDecimals = numberofdecimals;
        }
        static void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            SharpDXCheckBox control = (SharpDXCheckBox)sender;
        }
        private static void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            SharpDXRadioButton control = (SharpDXRadioButton)sender;
        }
    }
}
