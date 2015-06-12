using ExternalUtilsCSharp.SharpDXRenderer;
using ExternalUtilsCSharp.SharpDXRenderer.Controls;
using ExternalUtilsCSharp.UI;
using SharpDX;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickerHeroes.UI
{
    class Segments : SharpDXControl
    {
        public override bool CheckMouseOver(Vector2 cursorPoint)
        {
            return false;
        }

        public override Vector2 GetAbsoluteLocation()
        {
            return Vector2.Zero;
        }

        public override Vector2 GetSize()
        {
            return new Vector2(this.Width, this.Height);
        }
        public Segments()
        {
            this.BackColor = Color.Black;
            this.ForeColor = Color.Black;
        }
        public override void Draw(SharpDXRenderer renderer)
        {
            base.Draw(renderer);
            //Vector2 rightHalf = new Vector2(this.Width / 2f, 0);
            ////Right half of the window
            //renderer.DrawRectangle(
            //    this.ForeColor,
            //    rightHalf,
            //    new Vector2(this.Width /2f, this.Height));
            ////Levelbar
            //renderer.DrawRectangle(
            //    this.ForeColor,
            //    new Vector2(this.Width / 2f, 0),
            //    new Vector2(this.Width / 2f, this.Width * 0.0675f));
            //Levelbar items
            Vector2 item = new Vector2(this.Width * 0.0375f);
            Vector2 itemWidth = new Vector2(this.Width * 0.0375f, 0);
            Vector2 itemY = new Vector2(0, this.Width * 0.020f);
            Vector2 itemDistance = new Vector2(this.Width * 0.020f, 0);
            renderer.DrawRectangle(
                this.ForeColor,
                new Vector2(this.Width * 0.75f - item.X / 2f, this.Width * 0.020f),
                item);
            renderer.DrawRectangle(
                this.ForeColor,
                new Vector2(this.Width * 0.75f - item.X / 2f, 0) + itemY - itemDistance - itemWidth,
                item);
            renderer.DrawRectangle(
                this.ForeColor,
                new Vector2(this.Width * 0.75f - item.X / 2f, 0) + itemY - itemDistance * 2f - itemWidth * 2f,
                item);
            renderer.DrawRectangle(
                this.ForeColor,
                new Vector2(this.Width * 0.75f - item.X / 2f, 0) + itemY + itemDistance + itemWidth,
                item);
            ////Click-area
            //Vector2 areaSize = new Vector2(this.Width / 2f * 0.6f, this.Width * 0.15f);
            //Vector2 location = new Vector2(this.Width / 4f, this.Width / 2f * 0.7f);
            //renderer.DrawRectangle(
            //    this.ForeColor,
            //    rightHalf + location - areaSize / 2f,
            //    areaSize);
        }
    }
}
