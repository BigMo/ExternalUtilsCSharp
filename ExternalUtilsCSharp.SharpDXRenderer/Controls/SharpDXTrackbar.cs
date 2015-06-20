using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.SharpDXRenderer.Controls
{
    public class SharpDXTrackbar : SharpDXControl
    {
        #region VARIABLES
        private float minimum, maximum, value;
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
                    if (value < minimum)
                        value = minimum;
                    if (value > maximum)
                        value = maximum;
                    this.value = value;
                    OnValueChangedEvent(new EventArgs());
                }
            }
        }
        public int NumberOfDecimals { get; set; }
        public float TrackbarHeight { get; set; }
        public float StepSize { get; set; }
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
            this.minimum = 0;
            this.maximum = 100;
            this.value = 50;
            this.StepSize = 5f;
            this.NumberOfDecimals = 2;
            this.TrackbarHeight = 16f;
            this.FillParent = true;
        }

        void SharpDXTrackbar_FontChangedEvent(object sender, EventArgs e)
        {
        }
        #endregion

        #region METHODS
        public override void Draw(SharpDXRenderer renderer)
        {
            Vector2 location = this.GetAbsoluteLocation();
            Vector2 size = this.GetSize();
            Vector2 marginLocation = location - Vector2.UnitX * this.MarginLeft - Vector2.UnitY * this.MarginTop;
            Vector2 marginSize = size + Vector2.UnitX * this.MarginLeft + Vector2.UnitX * this.MarginRight + Vector2.UnitY * this.MarginTop + Vector2.UnitY * this.MarginBottom;

            renderer.FillRectangle(this.BackColor, marginLocation, marginSize);
            renderer.DrawRectangle(this.ForeColor, marginLocation, marginSize);

            string text = string.Format("{0} {1}", this.Text, Math.Round(this.value, this.NumberOfDecimals));
            Vector2 textSize = renderer.MeasureString(text, this.Font);

            renderer.DrawText(text, this.ForeColor, this.Font, location);

            float range = Math.Abs(this.minimum - this.maximum);
            float percent = 1f / range * this.value;
            Vector2 trackbarLocation = location + Vector2.UnitY * textSize.Y;
            Vector2 trackBarHandleSize = new Vector2(TrackbarHeight, TrackbarHeight);

            trackbarLocation = location + Vector2.UnitY * (textSize.Y + MarginTop + TrackbarHeight / 2f) + Vector2.UnitX * TrackbarHeight / 2f;
            Vector2 trackbarSize = new Vector2(size.X - TrackbarHeight, 0);
            Vector2 trackbarMarkerLocation = new Vector2(trackbarLocation.X + trackbarSize.X * percent, trackbarLocation.Y);
            Vector2 trackbarMarkerSize = new Vector2(TrackbarHeight / 2f, TrackbarHeight);

            renderer.DrawLine(this.ForeColor, trackbarLocation, trackbarLocation + trackbarSize, TrackbarHeight / 2f + 2f);
            renderer.DrawLine(this.BackColor, trackbarLocation, trackbarLocation + trackbarSize, TrackbarHeight / 2f);

            renderer.FillRectangle(this.ForeColor, trackbarMarkerLocation - (trackbarMarkerSize + 2f) / 2f, trackbarMarkerSize + 2f);
            renderer.FillRectangle(this.BackColor, trackbarMarkerLocation - trackbarMarkerSize / 2f, trackbarMarkerSize);

            this.Height = textSize.Y + TrackbarHeight + MarginTop + MarginBottom;

            base.Draw(renderer);
        }
        #endregion
    }
}
