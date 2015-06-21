using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.SharpDXRenderer.Controls
{
    public class SharpDXButtonKey : SharpDXButton
    {
        #region VARIABLES
        private int skip;
        private bool listen;
        private WinAPI.VirtualKeyShort key;
        #endregion

        #region PROPERTIES
        public WinAPI.VirtualKeyShort Key 
        {
            get { return this.key; }
            set
            {
                if(this.key != value)
                {
                    this.key = value;
                    OnKeyChangedEvent(new EventArgs());
                }
            }
        }
        #endregion


        #region EVENTS
        public event EventHandler KeyChangedEvent;
        protected virtual void OnKeyChangedEvent(EventArgs e)
        {
            if (KeyChangedEvent != null)
                KeyChangedEvent(this, e);
        }
        #endregion

        #region CONSTRUCTOR
        public SharpDXButtonKey() : base()
        {
            listen = false;
            this.key = WinAPI.VirtualKeyShort.XBUTTON1;
            this.MouseClickEventUp += SharpDXButtonKey_MouseClickEventUp;
        }
        #endregion

        #region METHODS
        void SharpDXButtonKey_MouseClickEventUp(object sender, UI.Control<SharpDXRenderer, SharpDX.Color, SharpDX.Vector2, SharpDX.DirectWrite.TextFormat>.MouseEventArgs e)
        {
            if (!e.LeftButton)
                return;
            listen = true;
            skip = 10;
        }

        public override void Draw(SharpDXRenderer renderer)
        {
            string text = null;
            string orig = this.Text;
            if (listen)
                text = string.Format("{0} <press key>", this.Text);
            else
                text = string.Format("{0} {1}", this.Text, this.Key);
            this.Text = text;
            base.Draw(renderer);
            this.Text = orig;
        }

        public override void Update(double secondsElapsed, KeyUtils keyUtils, SharpDX.Vector2 cursorPoint, bool checkMouse = false)
        {
            base.Update(secondsElapsed, keyUtils, cursorPoint, checkMouse);
            if (listen)
            {
                if(skip > 0)
                {
                    skip--;
                    return;
                }
                WinAPI.VirtualKeyShort[] buttons = keyUtils.KeysThatWentUp();
                if (buttons.Length > 0)
                {
                    Key = buttons[0];
                    listen = false;
                }
            }
        }

        public override void ApplySettings(ConfigUtils config)
        {
            if (this.Tag != null)
                if (config.HasKey(this.Tag.ToString()))
                    this.Key = config.GetValue<WinAPI.VirtualKeyShort>(this.Tag.ToString());
        }
        #endregion
    }
}
