using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalUtilsCSharp.UI.Controls;
using ExternalUtilsCSharp.SharpDXRenderer;
using SharpDX.DirectWrite;
using SharpDX;

namespace ClickerHeroes.UI
{
    class CHCheckBox : CheckBox<SharpDXRenderer, Color, Vector2, TextFormat>
    {
        public CHCheckBox(TextFormat font)
        {
            this.Font = font;
        }
        public override void Draw(SharpDXRenderer renderer)
        {
            base.Draw(renderer);
            Vector2 size = renderer.MeasureString(this.Text, this.Font);
            this.Width = size.X + this.Font.FontSize;
            this.Height = size.Y;

            Vector2 location = this.GetLocation();

            renderer.FillRectangle(this.BackColor, location - new Vector2(2, 2), this.GetSize() + new Vector2(4, 4));
            if (this.Checked)
                renderer.FillRectangle(this.ForeColor, location, new Vector2(this.Font.FontSize, this.Font.FontSize));
            else
                renderer.DrawRectangle(this.ForeColor, location, new Vector2(this.Font.FontSize, this.Font.FontSize));
            renderer.DrawText(this.Text, this.ForeColor, this.Font, location);
        }
        public override bool CheckMouseOver(SharpDX.Vector2 cursorPoint)
        {
            return
                this.X <= cursorPoint.X && this.X + this.Width <= cursorPoint.X &&
                this.Y <= cursorPoint.Y && this.Y + this.Height <= cursorPoint.Y;
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
