using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.SharpDXRenderer.Controls
{
    public class SharpDXGraph : SharpDXControl
    {
        #region VARIABLES
        private long minimum, maximum, numberofvalues;
        #endregion
        #region PROPERTIES
        public long NumberOfValues 
        {
            get { return numberofvalues; }
            set
            {
                if (numberofvalues != value)
                {
                    numberofvalues = value;
                    long[] newValues = new long[numberofvalues];
                    if (this.Values == null)
                        this.Values = newValues;
                    else if (this.Values.Length > newValues.Length)
                        Array.Copy(this.Values, 0, newValues, 0, newValues.Length);
                    else if (this.Values.Length < newValues.Length)
                        Array.Copy(this.Values, 0, newValues, 0, this.Values.Length);

                    this.Values = newValues;
                }
            }
        }
        public long Minimum 
        {
            get { return minimum; } 
            set
            {
                if(minimum != value)
                {
                    minimum = value;
                    this.Values = this.Values.Select(x => x < minimum ? minimum : x).ToArray();
                }
            }
        }
        public long Maximum
        {
            get { return maximum; }
            set
            {
                if (maximum != value)
                {
                    maximum = value;
                    this.Values = this.Values.Select(x => x > maximum ? maximum : x).ToArray();
                }
            }
        }
        public bool DynamicMaximum { get; set; }
        public long LastValue { get; set; }
        private long[] Values { get; set; }
        #endregion

        #region CONSTRUCTOR
        public SharpDXGraph(): base()
        {
            this.NumberOfValues = 100;
            this.Minimum = 0;
            this.Maximum = 100;
            this.DynamicMaximum = false;
        }
        #endregion

        #region METHODS
        public void AddValue(long value)
        {
            long nextValue = value - LastValue;
            LastValue = value;

            long[] newValues = new long[NumberOfValues];
            Array.Copy(Values, 1, newValues, 0, NumberOfValues - 1);
            newValues[newValues.Length - 1] = nextValue;

            this.Values = newValues;
        }

        public override void Draw(SharpDXRenderer renderer)
        {
            Vector2 location = this.GetAbsoluteLocation();
            Vector2 size = this.GetSize();

            renderer.FillRectangle(this.BackColor, location, size);
            renderer.DrawRectangle(this.ForeColor, location, size);

            Vector2[] points = new Vector2[NumberOfValues];
            long max = DynamicMaximum ? this.Values.Max() : this.Maximum;
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = location + new Vector2(this.Width / (points.Length - 1) * i, this.Height + this.MarginTop - (this.Height - this.MarginTop - this.MarginBottom) / max * this.Values[i]);
            }
            renderer.DrawLines(this.ForeColor, points);

            string maxString = MiscUtils.GetUnitFromSize(max, true);
            Vector2 maxSize = renderer.MeasureString(maxString, this.Font);
            Vector2 maxLocation = location + new Vector2(this.Width - maxSize.X, 0);
            renderer.DrawText(maxString, this.ForeColor, this.Font, maxLocation);

            base.Draw(renderer);
        }
        #endregion
    }
}
