using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.SharpDXRenderer.Controls
{
    public class SharpDXComboValue<T> : SharpDXControl
    {
        #region VARIABLES
        private int selectedIndex;
        #endregion

        #region PROPERTIES
        public Tuple<string, T>[] Values { get; set; }
        public int SelectedIndex 
        {
            get { return this.selectedIndex; }
            set
            {
                if(this.selectedIndex != value)
                {
                    if (value >= Values.Length)
                        value %= Values.Length;
                    if (value < 0)
                        value = Values.Length + value % Values.Length;
                    this.selectedIndex = value;
                    this.OnSelectedIndexChangedEvent(new ComboValueEventArgs(this.Tag, this.Value));
                }
            }
        }
        public T Value
        {
            get { return Values[this.SelectedIndex].Item2; }
        } 
        #endregion

        #region EVENTS
        public class ComboValueEventArgs : EventArgs
        {
            public object Tag { get; private set; }
            public object Value { get; private set; }
            public ComboValueEventArgs(object tag, object value)
            { 
                this.Tag = tag;
                this.Value = value;
            }
        }
        public event EventHandler<ComboValueEventArgs> SelectedIndexChangedEvent;
        protected virtual void OnSelectedIndexChangedEvent(ComboValueEventArgs e)
        {
            if (SelectedIndexChangedEvent != null)
                SelectedIndexChangedEvent(this, e);
        }
        #endregion

        #region CONSTRUCTORS
        public SharpDXComboValue() : base()
        {
            this.FillParent = true;
            this.TextAlign = TextAlignment.Center;
            this.MouseClickEventUp += SharpDXComboValue_MouseClickEventUp;
        }

        void SharpDXComboValue_MouseClickEventUp(object sender, UI.Control<SharpDXRenderer, Color, Vector2, SharpDX.DirectWrite.TextFormat>.MouseEventArgs e)
        {
            Vector2 location = this.GetAbsoluteLocation();
            Vector2 size = this.GetSize();

            Vector2 clickPoint = e.Position - location;
            if (clickPoint.X < size.X / 2f)
                this.SelectedIndex--;
            else
                this.SelectedIndex++;
        }
        #endregion

        #region METHODS
        public override void Draw(SharpDXRenderer renderer)
        {

            Vector2 location = this.GetAbsoluteLocation();
            Vector2 size = this.GetSize();

            if (this.MouseOver)
                renderer.FillRectangle(this.BackColor,
                    new Vector2(location.X - MarginLeft, location.Y - MarginTop),
                    new Vector2(size.X + MarginLeft + MarginRight, size.Y + MarginTop + MarginBottom));

            renderer.DrawRectangle(this.ForeColor,
                new Vector2(location.X - MarginLeft, location.Y - MarginTop),
                new Vector2(size.X + MarginLeft + MarginRight, size.Y + MarginTop + MarginBottom));

            float fontSize = (float)Math.Ceiling(this.Font.FontSize);
            string display = string.Format("{0}: {1}", this.Text, this.Values != null ? this.Values[this.SelectedIndex].Item1 : "<none>");
            Vector2 textSize = renderer.MeasureString(display, this.Font);
            Vector2 textLocation = location;
            this.Height = textSize.Y;
            switch (this.TextAlign)
            {
                case TextAlignment.Center:
                    textLocation.X += this.Width / 2f - textSize.X / 2f;
                    break;
                case TextAlignment.Right:
                    textLocation.X += this.Width - textSize.X;
                    break;
            }
            renderer.DrawText(display,
                this.ForeColor,
                this.Font,
                new Vector2(textLocation.X + MarginLeft, textLocation.Y));

            renderer.DrawText("<", this.ForeColor, this.Font, location);
            textSize = renderer.MeasureString(">", this.Font);
            textLocation = location + Vector2.UnitX * size.X - Vector2.UnitX * textSize.X;
            renderer.DrawText(">", this.ForeColor, this.Font, textLocation);
            base.Draw(renderer);
        }

        public override void ApplySettings(ConfigUtils config)
        {
            if (this.Tag != null)
            {
                if (config.HasKey(this.Tag.ToString()))
                {
                    T value = config.GetValue<T>(this.Tag.ToString());
                    for (int i = 0; i < Values.Length; i++)
                    {
                        if (Values[i].Item2.Equals(value))
                        {
                            this.SelectedIndex = i;
                            break;
                        }
                    }                            
                }
            }
        }
        #endregion
    }
}
