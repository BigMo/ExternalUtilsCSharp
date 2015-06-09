using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.SharpDXRenderer.Controls
{
    /// <summary>
    /// A simple checkbox
    /// </summary>
    public class SharpDXCheckBox : SharpDXControl
    {
        #region VARIABLES
        private bool isChecked;
        #endregion
        #region PROPERTIES
        public bool Checked
        {
            get { return isChecked; }
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    OnCheckedChangedEvent(new EventArgs());
                }
            }
        }
        #endregion
        #region EVENTS
        public event EventHandler CheckedChangedEvent;
        protected virtual void OnCheckedChangedEvent(EventArgs e)
        {
            if (CheckedChangedEvent != null)
                CheckedChangedEvent(this, e);
        }
        #endregion
        #region CONSTRUCTOR
        public SharpDXCheckBox()
            : base()
        {
            this.Text = "<SharpDXCheckBox>";
            this.MouseClickEventUp += SharpDXCheckBox_MouseClickEventUp;
            this.FillParent = true;
        }
        #endregion
        #region METHODS

        void SharpDXCheckBox_MouseClickEventUp(object sender, UI.Control<SharpDXRenderer, Color, Vector2, SharpDX.DirectWrite.TextFormat>.MouseClickEventArgs e)
        {
            if (e.LeftButton)
                this.Checked = !this.Checked;
        }
        public override void Draw(SharpDXRenderer renderer)
        {
            base.Draw(renderer);
            float fontSize = (float)Math.Ceiling(this.Font.FontSize);
            Vector2 size = renderer.MeasureString(this.Text, this.Font);
            if(!this.FillParent)
                this.Width = size.X + fontSize;
            this.Height = size.Y;
            Vector2 location = this.GetLocation();
            Vector2 box = new Vector2(fontSize, fontSize);
            Vector2 boxLocation = new Vector2(this.X, this.Y + this.Height / 2f - box.Y / 2f);
            if(this.MouseOver)
                renderer.FillRectangle(this.BackColor, 
                    new Vector2(location.X - MarginLeft, location.Y - MarginTop),
                    new Vector2(this.Width + MarginLeft + MarginRight, this.Height + MarginTop + MarginBottom));

            if (this.Checked)
                renderer.FillRectangle(this.ForeColor, boxLocation, box);
            else
                renderer.DrawRectangle(this.ForeColor, boxLocation, box);
            renderer.DrawText(this.Text, 
                this.ForeColor, 
                this.Font,
                new Vector2(location.X + box.X + MarginLeft, location.Y));
        }
        #endregion
    }
}
