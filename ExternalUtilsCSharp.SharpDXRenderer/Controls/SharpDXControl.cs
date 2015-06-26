using SharpDX;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalUtilsCSharp.InputUtils;

namespace ExternalUtilsCSharp.SharpDXRenderer.Controls
{
    public abstract class SharpDXControl : ExternalUtilsCSharp.UI.Control<SharpDXRenderer, Color, Vector2, TextFormat>
    {
        #region CONSTRUCTOR
        public SharpDXControl()
        {
            this.ForeColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
            this.BackColor = new Color(0.8f, 0.8f, 0.8f, 0.9f);
            this.MarginBottom = 2f;
            this.MarginLeft = 2f;
            this.MarginRight = 2f;
            this.MarginTop = 2f;
        }
        #endregion
        #region METHODS
        public override bool CheckMouseOver(Vector2 cursorPoint)
        {
            Vector2 location = this.GetAbsoluteLocation();
            return
                cursorPoint.X >= location.X && cursorPoint.X <= location.X + this.Width &&
                cursorPoint.Y >= location.Y && cursorPoint.Y <= location.Y + this.Height;
        }

        public override Vector2 GetAbsoluteLocation()
        {
            if (this.Parent == null)
                return new Vector2(this.X, this.Y);
            else
                return this.Parent.GetAbsoluteLocation() + new Vector2(this.X, this.Y);
        }

        public override void Update(double secondsElapsed, InputUtilities keyUtils, Vector2 cursorPoint, bool checkMouse = false)
        {
            if (this.FillParent && this.Parent != null)
                this.Width = Parent.Width - Parent.MarginLeft - Parent.MarginRight - this.MarginLeft - this.MarginRight;
            base.Update(secondsElapsed, keyUtils, cursorPoint, checkMouse);
        }

        public override Vector2 GetSize()
        {
            return new Vector2(this.Width, this.Height);
        }

        public virtual void ApplySettings(ConfigUtils config)
        { }
        #endregion
    }
}
