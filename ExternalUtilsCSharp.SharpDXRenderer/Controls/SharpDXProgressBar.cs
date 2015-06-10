using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.SharpDXRenderer.Controls
{
    public class SharpDXProgressBar : SharpDXControl
    {
        #region Variables
        private float minimum, maximum, curValue;
        #endregion

        #region PROPERTIES
        public float Maximum
        {
            get { return maximum; }
            set
            {
                if (value > Minimum)
                {
                    maximum = value;
                    if (Value > Maximum)
                        curValue = Maximum;
                }
            }
        }
        public float Minimum
        {
            get { return minimum; }
            set
            {
                if(value < Maximum)
                {
                    minimum = value;
                    if (Value < Minimum)
                        curValue = Minimum;
                }
            }
        }
        public float Value
        {
            get { return curValue; }
            set
            {
                if (Minimum > value)
                    curValue = Minimum;
                else if (Maximum < value)
                    curValue = Maximum;
                else
                    curValue = value;
            }
        }
        public Color FillColor { get; set; }
        #endregion

        #region CONSTRUCTOR
        public SharpDXProgressBar()
        {
            minimum = 0;
            curValue = 50;
            maximum = 100;
            this.FillColor = new Color(0.2f, 0.9f, 0.2f, 0.9f);
            this.Height = 14f;
        }
        #endregion

        #region METHODS
        public override void Draw(SharpDXRenderer renderer)
        {
            Vector2 location = this.GetAbsoluteLocation();
            Vector2 size = this.GetSize();
            Vector2 fillSize = new Vector2(size.X * (this.Value / this.Maximum), size.Y);

            renderer.FillRectangle(this.BackColor, location, size);
            renderer.FillRectangle(this.FillColor, location, fillSize);
            renderer.DrawRectangle(this.ForeColor, location, size);

            this.FillParent = true;
            base.Draw(renderer);
        }
        #endregion
    }
}
