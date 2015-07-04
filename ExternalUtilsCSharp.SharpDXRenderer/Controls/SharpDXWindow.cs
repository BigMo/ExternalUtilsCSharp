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
            this.Panel.DrawBackground = false;
            this.Panel.DrawBorder = false;
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
        }

        void SharpDXWindow_TextChangedEvent(object sender, EventArgs e)
        {
            this.Caption.Text = this.Text;
        }
        #endregion

        #region METHODS
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

        public override void Update(double secondsElapsed, KeyUtils keyUtils, Vector2 cursorPoint, bool checkMouse = false)
        {
            base.Update(secondsElapsed, keyUtils, cursorPoint, checkMouse);
        }
        #endregion
    }
}
