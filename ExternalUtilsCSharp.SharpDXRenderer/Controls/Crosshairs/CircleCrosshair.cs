using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.SharpDXRenderer.Controls.Crosshairs
{
    public class CircleCrosshair : Crosshair
    {
        #region METHODS
        public override void Draw(SharpDXRenderer renderer)
        {
            base.Draw(renderer);
            Vector2 location = this.GetAbsoluteLocation();
            float length = (this.Radius + this.Spread * this.SpreadScale) * 2f;
            Vector2 size = new Vector2(length, length);

            if (this.Outline)
                renderer.DrawEllipse(this.SecondaryColor, location, size, true, this.Width + 2f);
            renderer.DrawEllipse(this.PrimaryColor, location, size, true, this.Width);
        }
        #endregion
    }
}
