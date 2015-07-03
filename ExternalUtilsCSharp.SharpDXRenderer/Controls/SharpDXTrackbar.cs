using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExternalUtilsCSharp.InputUtils;

namespace ExternalUtilsCSharp.SharpDXRenderer.Controls
{
    public class SharpDXTrackbar : SharpDXControl
    {
        #region VARIABLES
        private float value;
        private Vector2 trackbarLocation;
        #endregion

        #region PROPERTIES
        public float Minimum { get; set; }
        public float Maximum { get; set; }
        public float Value {
            get { return this.value; } 
            set
            {
                if (this.value != value)
                {
                    if (value < Minimum)
                        value = Minimum;
                    if (value > Maximum)
                        value = Maximum;
                    this.value = value;
                    OnValueChangedEvent(new EventArgs());
                }
            }
        }
        public int NumberOfDecimals { get; set; }
        public float TrackbarHeight { get; set; }
        public float Percent
        {
            get
            {
                return 1f / Math.Abs(this.Minimum - this.Maximum) * this.value;
            }
        }
        #endregion

        #region EVENTS
        public event EventHandler ValueChangedEvent;
        protected virtual void OnValueChangedEvent(EventArgs e)
        {
            if (ValueChangedEvent != null)
                ValueChangedEvent(this, e);
        }
        #endregion

        #region CONSTRUCTOR
        public SharpDXTrackbar() : base()
        {
            this.Minimum = 0;
            this.Maximum = 100;
            this.value = 50;
            this.NumberOfDecimals = 2;
            this.TrackbarHeight = 16f;
            this.FillParent = true;
            this.MouseMovedEvent += SharpDXTrackbar_MouseMovedEvent;
            this.MouseWheelEvent += SharpDXTrackbar_MouseWheelEvent;
        }

        void SharpDXTrackbar_MouseMovedEvent(object sender, MouseEventExtArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            Vector2 size = this.GetSize();

            Vector2 trackbarSize = new Vector2(size.X - TrackbarHeight, 0);
            Vector2 cursorPos = new Vector2(((Vector2)e.PosOnForm).X - trackbarLocation.X,((Vector2)e.PosOnForm).Y - trackbarLocation.Y);


            if (cursorPos.X >= 0 && cursorPos.X <= trackbarSize.X)
            {
                if (cursorPos.Y >= -TrackbarHeight && cursorPos.Y <= TrackbarHeight)
                {
                    float percent = 1f / trackbarSize.X * cursorPos.X;            
                    float range = Math.Abs(this.Minimum - this.Maximum);
                    float val = range * percent;
                    this.Value = this.Minimum + val;
                }
            }

        }
        void SharpDXTrackbar_MouseWheelEvent(object sender, MouseEventExtArgs e)
        {
            if (!e.Wheel)
                return;

            float percent = this.Value/this.Maximum;
            if (e.UpOrDown == MouseEventExtArgs.UpDown.Up)
                percent += 0.01f;
            if (e.UpOrDown == MouseEventExtArgs.UpDown.Down)
                percent -= 0.01f;
            float range = Math.Abs(this.Minimum - this.Maximum);
            float val = range * percent;
            this.Value = this.Minimum + val;
            Console.WriteLine("Value"+Value);
        }
        #endregion

        #region METHODS
        public override void Draw(SharpDXRenderer renderer)
        {
            Vector2 location = this.GetAbsoluteLocation();
            Vector2 size = this.GetSize();
            Vector2 marginLocation = location - Vector2.UnitX * this.MarginLeft - Vector2.UnitY * this.MarginTop;
            Vector2 marginSize = size + Vector2.UnitX * this.MarginLeft + Vector2.UnitX * this.MarginRight + Vector2.UnitY * this.MarginTop + Vector2.UnitY * this.MarginBottom;

            //renderer.FillRectangle(this.BackColor, marginLocation, marginSize);
            //renderer.DrawRectangle(this.ForeColor, marginLocation, marginSize);

            string text = string.Format("{0} {1}", this.Text, Math.Round(this.value, this.NumberOfDecimals));
            Vector2 textSize = renderer.MeasureString(text, this.Font);

            renderer.DrawText(text, this.ForeColor, this.Font, location);

            trackbarLocation = location + Vector2.UnitY * textSize.Y;
            Vector2 trackBarHandleSize = new Vector2(TrackbarHeight, TrackbarHeight);

            trackbarLocation = location + Vector2.UnitY * (textSize.Y + MarginTop + TrackbarHeight / 2f) + Vector2.UnitX * TrackbarHeight / 2f;
            Vector2 trackbarSize = new Vector2(size.X - TrackbarHeight, 0);
            Vector2 trackbarMarkerLocation = new Vector2(trackbarLocation.X + trackbarSize.X * Percent, trackbarLocation.Y);
            Vector2 trackbarMarkerSize = new Vector2(TrackbarHeight / 4f, TrackbarHeight);

            renderer.DrawLine(this.ForeColor, trackbarLocation, trackbarLocation + trackbarSize, TrackbarHeight / 4f + 2f);
            renderer.DrawLine(this.BackColor, trackbarLocation, trackbarLocation + trackbarSize, TrackbarHeight / 4f);

            renderer.FillRectangle(this.ForeColor, trackbarMarkerLocation - (trackbarMarkerSize + 2f) / 2f, trackbarMarkerSize + 2f);
            renderer.FillRectangle(this.BackColor, trackbarMarkerLocation - trackbarMarkerSize / 2f, trackbarMarkerSize);

            this.Height = textSize.Y + TrackbarHeight + MarginTop + MarginBottom;

            base.Draw(renderer);
        }

        public override void ApplySettings(ConfigUtils config)
        {
            if (this.Tag != null)
                if (config.HasKey(this.Tag.ToString()))
                    this.Value = config.GetValue<float>(this.Tag.ToString());
        }
        #endregion
    }
}
