using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.SharpDXRenderer.Controls
{
    public class SharpDXRadioButton : SharpDXCheckable
    {
        #region PROPERTIES
        public string GroupName { get; set; }
        #endregion

        #region CONSTRUCTOR
        public SharpDXRadioButton()
            : base()
        {
            this.Text = "<SharpDXRadioButton>";
            this.FillParent = true;
            this.MouseClickEventUp += SharpDXRadioButton_MouseClickEventUp;
            this.CheckedChangedEvent += SharpDXRadioButton_CheckedChangedEvent;
        }
        #endregion 

        #region METHODS
        void SharpDXRadioButton_CheckedChangedEvent(object sender, EventArgs e)
        {
            if(this.Checked && this.Parent != null)
            {
                foreach (SharpDXControl control in this.Parent.ChildControls)
                {
                    if (control == this)
                        continue;
                    if(control is SharpDXRadioButton)
                    {
                        SharpDXRadioButton rdb = (SharpDXRadioButton)control;
                        if (rdb.Checked && rdb.GroupName == this.GroupName)
                            rdb.Checked = false;
                    }
                }
            }
        }

        void SharpDXRadioButton_MouseClickEventUp(object sender, UI.Control<SharpDXRenderer, SharpDX.Color, SharpDX.Vector2, SharpDX.DirectWrite.TextFormat>.MouseEventArgs e)
        {
            if (e.LeftButton && !this.Checked)
            {
                this.Checked = true;
            }
        }

        public override void Draw(SharpDXRenderer renderer)
        {
            base.Draw(renderer);
            float fontSize = (float)Math.Ceiling(this.Font.FontSize);
            Vector2 size = renderer.MeasureString(this.Text, this.Font);
            if (!this.FillParent)
                this.Width = size.X + fontSize;
            this.Height = size.Y;
            Vector2 location = this.GetAbsoluteLocation();
            Vector2 box = new Vector2(fontSize, fontSize);
            Vector2 boxLocation = new Vector2(location.X, location.Y + this.Height / 2f - box.Y / 2f);
            if (this.MouseOver)
                renderer.FillRectangle(this.BackColor,
                    new Vector2(location.X - MarginLeft, location.Y - MarginTop),
                    new Vector2(this.Width + MarginLeft + MarginRight, this.Height + MarginTop + MarginBottom));

            renderer.DrawEllipse(this.ForeColor, boxLocation, box);

            if (this.Checked)
                renderer.FillEllipse(this.ForeColor, boxLocation + Vector2.One * 2, box - Vector2.One * 4);

            renderer.DrawText(this.Text,
                this.ForeColor,
                this.Font,
                new Vector2(location.X + box.X + MarginLeft, location.Y));
        }

        public override void ApplySettings(ConfigUtils config)
        {
            if (this.Tag != null)
                if (config.HasKey(this.Tag.ToString()))
                    this.Checked = config.GetValue<bool>(this.Tag.ToString());
        }
        #endregion
    }
}
