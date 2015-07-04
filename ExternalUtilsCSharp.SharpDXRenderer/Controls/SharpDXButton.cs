using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.SharpDXRenderer.Controls
{
    public class SharpDXButton : SharpDXControl
    {
        public SharpDXButton()
        {
            this.FillParent = true;
            this.TextAlign = TextAlignment.Center;
            this.DrawBackground = true;
        }
        public override void Draw(SharpDXRenderer renderer)
        {
            Vector2 location = this.GetAbsoluteLocation();
            Vector2 size = this.GetSize();

            if(this.MouseOver || this.DrawBackground)
                renderer.FillRectangle(this.BackColor,
                    new Vector2(location.X - MarginLeft, location.Y - MarginTop),
                    new Vector2(size.X + MarginLeft + MarginRight, size.Y + MarginTop + MarginBottom));
            if(this.DrawBorder)
                renderer.DrawRectangle(this.ForeColor,
                new Vector2(location.X - MarginLeft, location.Y - MarginTop),
                new Vector2(size.X + MarginLeft + MarginRight, size.Y + MarginTop + MarginBottom));

            float fontSize = (float)Math.Ceiling(this.Font.FontSize);
            Vector2 textSize = renderer.MeasureString(this.Text, this.Font);

            this.Height = textSize.Y;
            switch (this.TextAlign)
            {
                case TextAlignment.Center:
                    location.X += this.Width / 2f - textSize.X / 2f;
                    break;
                case TextAlignment.Right:
                    location.X += this.Width - textSize.X;
                    break;
            }
            renderer.DrawText(this.Text,
                this.ForeColor,
                this.Font,
                new Vector2(location.X + MarginLeft, location.Y));

            base.Draw(renderer);
        }
    }
}
