using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.SharpDXRenderer.Controls
{
    public class SharpDXColorControl : SharpDXPanel
    {
        #region VARIABLES
        private UI.UIObjects.Color color;
        private SharpDXTrackbar A, R, G, B;
        private SharpDXButton preview;
        #endregion

        #region PROPERTIES
        public SharpDX.Color SDXColor { get; private set; }
        public UI.UIObjects.Color Color
        {
            get { return this.color; }
            set
            {
                if(value.ToARGB() != color.ToARGB())
                {
                    this.color = value;
                    SDXColor = new SharpDX.Color(color.R, color.G, color.B, color.A);
                    this.A.SetValue(this.color.A / 255f);
                    this.R.SetValue(this.color.R / 255f);
                    this.G.SetValue(this.color.G / 255f);
                    this.B.SetValue(this.color.B / 255f);
                    this.preview.BackColor = SDXColor;
                    this.preview.ForeColor = new SharpDX.Color(255 - SDXColor.R, 255 - SDXColor.G, 255 - SDXColor.B);
                    this.OnColorChangedEvent(new EventArgs());
                }
            }
        }
        #endregion

        #region EVENTS
        public event EventHandler ColorChangedEvent;
        protected virtual void OnColorChangedEvent(EventArgs e)
        {
            if (ColorChangedEvent != null)
                ColorChangedEvent(this, e);
        }
        #endregion

        #region CONSTRUCTORS
        public SharpDXColorControl() : base()
        {
            this.color = new UI.UIObjects.Color(0, 0, 0, 0);
            this.TextChangedEvent += SharpDXColorControl_TextChangedEvent;
            this.DynamicWidth = false;
            this.FillParent = true;

            preview = new SharpDXButton();
            preview.FillParent = true;
            preview.Text = "";
            this.AddChildControl(preview);

            SetupChannel(ref this.A, "Alpha-channel", this.color.A);
            SetupChannel(ref this.R, "R-channel", this.color.R);
            SetupChannel(ref this.G, "G-channel", this.color.G);
            SetupChannel(ref this.B, "B-channel", this.color.B);

            this.Color = new UI.UIObjects.Color(1f, 1f, 1f, 1f);
        }

        void SharpDXColorControl_TextChangedEvent(object sender, EventArgs e)
        {
            preview.Text = this.Text;
        }
        #endregion

        #region METHODS
        private void SetupChannel(ref SharpDXTrackbar control, string channel, byte value)
        {
            control = new SharpDXTrackbar();
            control.Minimum = 0;
            control.Maximum = 1;
            control.NumberOfDecimals = 4;
            control.Value = value / 255f;
            control.ValueChangedEvent += control_ValueChangedEvent;
            control.FillParent = true;
            control.Text = channel;
            this.AddChildControl(control);
        }

        void control_ValueChangedEvent(object sender, EventArgs e)
        {
            this.Color = new UI.UIObjects.Color(A.Value, R.Value, G.Value, B.Value);
        }

        public override void ApplySettings(ConfigUtils config)
        {
            if (this.Tag != null)
                if (config.HasKey(this.Tag.ToString()))
                    this.Color = UI.UIObjects.Color.FromFormat(config.GetValue<uint>(this.Tag.ToString()), UI.UIObjects.Color.ColorFormat.RGBA);
        }
        #endregion
    }
}
