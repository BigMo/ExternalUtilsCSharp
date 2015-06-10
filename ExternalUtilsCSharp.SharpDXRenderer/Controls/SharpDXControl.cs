using SharpDX;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return
                cursorPoint.X >= this.X && cursorPoint.X <= this.X + this.Width &&
                cursorPoint.Y >= this.Y && cursorPoint.Y <= this.Y + this.Height;
        }

        public override Vector2 GetLocation()
        {
            return new Vector2(this.X, this.Y);
        }

        public override void Update(double secondsElapsed, KeyUtils keyUtils, Vector2 cursorPoint)
        {
            if (this.FillParent && this.Parent != null)
                this.Width = Parent.Width - Parent.MarginLeft - Parent.MarginRight - this.MarginLeft - this.MarginRight;
            base.Update(secondsElapsed, keyUtils, cursorPoint);
        }

        public override Vector2 GetSize()
        {
            return new Vector2(this.Width, this.Height);
        }
        #endregion
    }
}
