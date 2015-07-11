using ExternalUtilsCSharp.SharpDXRenderer.Controls;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExternalUtilsCSharp.InputUtils;

namespace SteamMonsterGame
{
    class ClickerWindow : SharpDXControl
    {
        private bool mouseDrag = false;
        public ClickerWindow() : base()
        {
            this.MouseMovedEvent += ClickerWindow_MouseMovedEvent;
            this.MouseLeftEvent += ClickerWindow_MouseLeftEvent;
            this.MouseClickEventUp += ClickerWindow_MouseClickEventUp;
            this.MouseClickEventDown += ClickerWindow_MouseClickEventDown;
        }

        void ClickerWindow_MouseClickEventDown(object sender, MouseEventExtArgs e)
        {
            if (e.Button == MouseButtons.Left)
                mouseDrag = true;
        }

        void ClickerWindow_MouseClickEventUp(object sender, MouseEventExtArgs e)
        {
            if (e.Button == MouseButtons.Left)
                mouseDrag = false;
        }

        void ClickerWindow_MouseLeftEvent(object sender, EventArgs e)
        {
            mouseDrag = false;
        }

        void ClickerWindow_MouseMovedEvent(object sender, MouseEventExtArgs e)
        {
            Vector2 location = this.GetAbsoluteLocation();
            Vector2 localMousePos = new Vector2(((Vector2)e.PosOnForm).X - location.X,((Vector2)e.PosOnForm).Y-location.Y);
            Vector2 localLastMousePos = LastMousePos - location;
            if (mouseDrag)
            {

                if (localLastMousePos.X > this.Width - 8)
                {
                    this.Width += ((Vector2)e.PosOnForm).X - LastMousePos.X;
                }
                if (localLastMousePos.Y > this.Height - 8)
                {
                    this.Height += ((Vector2)e.PosOnForm).Y - LastMousePos.Y;
                }
                else
                {
                    this.X += ((Vector2)e.PosOnForm).X - LastMousePos.X;
                    this.Y += ((Vector2)e.PosOnForm).Y - LastMousePos.Y;
                }
            }
        }

        public override void Draw(ExternalUtilsCSharp.SharpDXRenderer.SharpDXRenderer renderer)
        {
            Vector2 location = this.GetAbsoluteLocation();
            Vector2 size = this.GetSize();
            renderer.DrawRectangle(this.BackColor, location + Vector2.One * 4, size - Vector2.One * 8, 8f);
            renderer.DrawRectangle(this.ForeColor, location, size);
            renderer.DrawRectangle(this.ForeColor, location + Vector2.One * 8, size - Vector2.One * 16);
            base.Draw(renderer);
        }
    }
}
