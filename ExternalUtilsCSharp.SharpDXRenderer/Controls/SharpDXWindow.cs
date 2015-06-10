using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.SharpDXRenderer.Controls
{
    public class SharpDXWindow : SharpDXPanel
    {
        #region VARIABLES
        private bool mouseDown;
        #endregion

        #region PROPERTIES
        public Vector2 TitleBarSize { get; set; }
        public SharpDXLabel Caption { get; set; }
        public SharpDXPanel Panel { get; set; }
        #endregion

        #region CONSTRUCTORS
        public SharpDXWindow() : base()
        {
            this.Caption = new SharpDXLabel();
            this.Panel = new SharpDXPanel();
            this.mouseDown = false;
            //this.DynamicHeight = false;
            //this.DynamicWidth = false;

            this.AddChildControl(this.Caption);
            this.AddChildControl(this.Panel);

            this.MouseClickEventUp += SharpDXWindow_MouseClickEventUp;
            this.MouseClickEventDown += SharpDXWindow_MouseClickEventDown;
            this.MouseLeftEvent += SharpDXWindow_MouseLeftEvent;
            this.MouseMovedEvent += SharpDXWindow_MouseMovedEvent;
            this.TextChangedEvent += SharpDXWindow_TextChangedEvent;
            this.FontChangedEvent += SharpDXWindow_FontChangedEvent;
        }

        void SharpDXWindow_FontChangedEvent(object sender, EventArgs e)
        {
            this.Caption.Font = this.Font;
        }

        void SharpDXWindow_TextChangedEvent(object sender, EventArgs e)
        {
            this.Caption.Text = this.Text;
        }
        #endregion

        #region METHODS
        public override void Draw(SharpDXRenderer renderer)
        {
            //Vector2 location = this.GetAbsoluteLocation();
            
            //Vector2 boxLocation = new Vector2(location.X - this.MarginLeft, location.Y - this.MarginTop);
            //Vector2 boxSize = new Vector2(this.Width + this.MarginLeft + this.MarginRight, this.Height + this.MarginBottom + this.MarginTop);
            //renderer.FillRectangle(this.BackColor,
            //    boxLocation,
            //    boxSize);
            //renderer.DrawRectangle(this.ForeColor,
            //    boxLocation,
            //    boxSize);

            //Vector2 textLocation = location + Vector2.UnitX * this.MarginLeft * 2 + Vector2.UnitY * this.MarginTop;
            //Vector2 textSize = renderer.MeasureString(this.Text, this.Font);
            //renderer.DrawText(this.Text, this.ForeColor, this.Font, textLocation);

            //Vector2 spacerLocation = location + Vector2.UnitY * textSize.Y + this.MarginTop * 3 * Vector2.UnitY - Vector2.UnitY * this.MarginLeft;
            //Vector2 spacerSize = new Vector2(this.Width, 2f);
            //renderer.FillRectangle(this.ForeColor, spacerLocation, spacerSize);

            base.Draw(renderer);
        }

        void SharpDXWindow_MouseMovedEvent(object sender, UI.Control<SharpDXRenderer, SharpDX.Color, SharpDX.Vector2, SharpDX.DirectWrite.TextFormat>.MouseEventArgs e)
        {
            if (mouseDown)
            {
                Vector2 offset = e.Position - this.LastMousePos;
                this.X += offset.X;
                this.Y += offset.Y;
            }
        }

        void SharpDXWindow_MouseLeftEvent(object sender, EventArgs e)
        {
            mouseDown = false;
        }

        void SharpDXWindow_MouseClickEventDown(object sender, UI.Control<SharpDXRenderer, SharpDX.Color, SharpDX.Vector2, SharpDX.DirectWrite.TextFormat>.MouseEventArgs e)
        {
            if (e.LeftButton)
                mouseDown = true;
        }

        void SharpDXWindow_MouseClickEventUp(object sender, UI.Control<SharpDXRenderer, SharpDX.Color, SharpDX.Vector2, SharpDX.DirectWrite.TextFormat>.MouseEventArgs e)
        {
            if (e.LeftButton)
                mouseDown = false;
        }
        #endregion
    }
}
