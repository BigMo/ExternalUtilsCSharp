using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.SharpDXRenderer.Controls
{
    /// <summary>
    /// A label displaying text
    /// </summary>
    public class SharpDXLabel : SharpDXControl
    {

        #region CONSTRUCTOR
        public SharpDXLabel()
            : base()
        {
            this.Text = "<SharpDXLabel>";
        }
        #endregion

        #region METHODS
        public override void Draw(SharpDXRenderer renderer)
        {
            base.Draw(renderer);
            float fontSize = (float)Math.Ceiling(this.Font.FontSize);
            Vector2 size = renderer.MeasureString(this.Text, this.Font);
            if (!this.FillParent)
                this.Width = size.X;
            this.Height = size.Y;
            Vector2 location = this.GetLocation();
            renderer.DrawText(this.Text,
                this.ForeColor,
                this.Font,
                new Vector2(location.X + MarginLeft, location.Y));
        }
        #endregion
    }
}
