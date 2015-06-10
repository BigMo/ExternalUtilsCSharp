using ExternalUtilsCSharp.UI;
using SharpDX;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.SharpDXRenderer.Controls
{
    /// <summary>
    /// A panel which repositions its childcontrols in the order they were added
    /// </summary>
    public class SharpDXPanel : SharpDXControl
    {
        #region PROPERTIES
        /// <summary>
        /// Whether this panel tightly wraps around its childcontrols or has a fixed width
        /// </summary>
        public bool DynamicWidth { get; set; }
        /// <summary>
        /// Whether this panel tightly wraps around its childcontrols or has a fixed height
        /// </summary>
        public bool DynamicHeight { get; set; }
        #endregion

        #region CONSTUCTOR
        public SharpDXPanel()
            : base()
        {
            this.DynamicWidth = true;
            this.DynamicHeight = true;
            this.BackColor = new Color(0.9f, 0.9f, 0.9f, 1f);
        }
        #endregion

        #region METHODS
        public override void Update(double secondsElapsed, KeyUtils keyUtils, SharpDX.Vector2 cursorPoint)
        {
            base.Update(secondsElapsed, keyUtils, cursorPoint);
            if (this.Visible)
            {
                float width = 0, height = 0;
                for (int i = 0; i < this.ChildControls.Count; i++)
                {
                    Control<SharpDXRenderer, Color, Vector2, TextFormat> control = this.ChildControls[i];
                    if (i == 0)
                    {
                        control.X = control.MarginLeft + this.MarginLeft;
                        control.Y = control.MarginTop;
                    }
                    else
                    {
                        Control<SharpDXRenderer, Color, Vector2, TextFormat> lastControl = this.ChildControls[i - 1];
                        control.X = lastControl.X;
                        control.Y = lastControl.Y + lastControl.Height + lastControl.MarginBottom + control.MarginTop;
                    }

                    if (this.DynamicHeight)
                        if (i == this.ChildControls.Count - 1)
                            height = control.Y + control.Height + control.MarginBottom ;
                    if (this.DynamicWidth)
                        if (control.Width + control.MarginLeft + control.MarginRight > width)
                            width = control.Width + control.MarginLeft + control.MarginRight;
                }
                if (this.DynamicWidth)
                    this.Width = width + this.MarginLeft + this.MarginRight;
                if (this.DynamicHeight)
                    this.Height = height + this.MarginBottom;
            }
            else
            {
                this.Height = 0;
            }
        }

        public override void Draw(SharpDXRenderer renderer)
        {
            Vector2 location = this.GetAbsoluteLocation();
            Vector2 boxLocation = new Vector2(location.X - this.MarginLeft, location.Y - this.MarginTop);
            Vector2 boxSize = new Vector2(this.Width + this.MarginLeft + this.MarginRight, this.Height + this.MarginBottom + this.MarginTop);
            renderer.FillRectangle(this.BackColor,
                boxLocation,
                boxSize);
            renderer.DrawRectangle(this.ForeColor,
                boxLocation,
                boxSize);
            base.Draw(renderer);
        }

        public void InsertSpacer()
        {
            this.AddChildControl(new SharpDXSpacer());
        }
        #endregion
    }
}
