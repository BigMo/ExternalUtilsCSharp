using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalUtilsCSharp.UI.Controls;
using ExternalUtilsCSharp.SharpDXRenderer;
using SharpDX.DirectWrite;
using SharpDX;
using ExternalUtilsCSharp;

namespace ClickerHeroes.UI
{
    class CHCheckBox : CheckBox<SharpDXRenderer, Color, Vector2, TextFormat>
    {
        public CHCheckBox()
        {
            this.BackColor = Color.Gray;
            this.ForeColor = Color.White;
        }
        public override void Draw(SharpDXRenderer renderer)
        {
            base.Draw(renderer);
            Vector2 size = renderer.MeasureString(this.Text, this.Font);
            this.Width = size.X + this.Font.FontSize + 4;
            this.Height = size.Y;

            Vector2 location = this.GetLocation();
            Vector2 box = new Vector2(this.Font.FontSize, this.Font.FontSize);

            Color clr = this.BackColor;
            if (this.MouseOver)
            {
                float factor = (float)Math.Sin(MathUtils.DegreesToRadians((float)DateTime.Now.Millisecond)) / (float)Math.PI;
                clr *= (0.75f + 0.25f * factor);
            }
            renderer.FillRectangle(clr, location - Vector2.One * 2, this.GetSize() + Vector2.One * 4);

            if (this.Checked)
                renderer.FillRectangle(this.ForeColor, location + Vector2.One * 2, box);
            else
                renderer.DrawRectangle(this.ForeColor, location + Vector2.One * 2, box);
            renderer.DrawText(this.Text, this.ForeColor, this.Font, location + new Vector2(4 + box.X, 2));
        }
        public override bool CheckMouseOver(SharpDX.Vector2 cursorPoint)
        {
            return
                cursorPoint.X >= this.X && cursorPoint.X <= this.X + this.Width &&
                cursorPoint.Y >= this.Y && cursorPoint.Y <= this.Y + this.Height;
        }

        public override Vector2 GetLocation()
        {
            return new Vector2(this.X, this.Y);
        }

        public override Vector2 GetSize()
        {
            return new Vector2(this.Width, this.Height);
        }
    }
}
