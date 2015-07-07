using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExternalUtilsCSharp.InputUtils;

namespace ExternalUtilsCSharp.SharpDXRenderer.Controls
{
    public class SharpDXTabControl : SharpDXPanel
    {
        #region VARIABLES
        private int selectedIndex;
        #endregion

        #region PROPERTIES
        public float MinimumHeaderWidth { get; set; }
        private RectangleF[] TabHeaders { get; set; }
        public int SelectedIndex
        {
            get { return this.selectedIndex; }
            set
            {
                if (this.selectedIndex != value && value >= 0 && value < ChildControls.Count)
                {
                    foreach (SharpDXControl panel in ChildControls)
                        panel.Visible = false;
                    this.selectedIndex = value;
                    ChildControls[this.selectedIndex].Visible = true;
                }
            }
        }
        #endregion

        #region CONSTRUCTORS
        public SharpDXTabControl() : base()
        {
            this.MouseClickEventUp += SharpDXTabControl_MouseClickEventUp;
            this.FontChangedEvent += SharpDXTabControl_FontChangedEvent;
            this.MinimumHeaderWidth = 50f;
        }

        void SharpDXTabControl_FontChangedEvent(object sender, EventArgs e)
        {
            foreach (SharpDXControl control in this.ChildControls)
                control.Font = this.Font;
        }
        #endregion

        #region METHODS

        void SharpDXTabControl_MouseClickEventUp(object sender, MouseEventExtArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            if (TabHeaders == null)
                return;

            Vector2 cursorPoint = (Vector2)e.PosOnForm - this.GetAbsoluteLocation();
            RectangleF cursor = new RectangleF(cursorPoint.X, cursorPoint.Y, 1, 1);
            for(int i = 0; i < TabHeaders.Length; i++)
            {
                if (TabHeaders[i].Intersects(cursor))
                {
                    this.SelectedIndex = i;
                    break;
                }
            }
        }
        public override void Update(double secondsElapsed, InputUtilities inputUtils, SharpDX.Vector2 cursorPoint, bool checkMouse = false)
        {
            base.Update(secondsElapsed, inputUtils, cursorPoint, checkMouse);
            if (TabHeaders == null)
                return;

            float maxHeight = TabHeaders.Max(x => x.Height);
            this.Height = ChildControls[this.SelectedIndex].Height + maxHeight;
            this.ChildControls[this.SelectedIndex].Y = maxHeight;
            this.Width = TabHeaders[TabHeaders.Length - 1].X + TabHeaders[TabHeaders.Length - 1].Width;
        }

        public override void Draw(SharpDXRenderer renderer)
        {
            base.Draw(renderer);

            if (this.ChildControls.Count == 0)
                return;

            TabHeaders = new RectangleF[ChildControls.Count];
            int idx = 0;
            Vector2 location = this.GetAbsoluteLocation();

            foreach (SharpDXControl panel in ChildControls)
            {
                Vector2 size = renderer.MeasureString(panel.Text, this.Font);
                if (idx == 0)
                    TabHeaders[idx] = new RectangleF(0, 0, (float)Math.Max(MinimumHeaderWidth, size.X + this.MarginLeft + this.MarginRight), size.Y);
                else
                    TabHeaders[idx] = new RectangleF(TabHeaders[idx - 1].X + TabHeaders[idx - 1].Width, TabHeaders[idx - 1].Y, (float)Math.Max(MinimumHeaderWidth, size.X + this.MarginLeft + this.MarginRight), size.Y);

                Vector2 tabLocation = location + new Vector2(TabHeaders[idx].X, TabHeaders[idx].Y);
                
                renderer.FillRectangle(this.BackColor, tabLocation, new Vector2(TabHeaders[idx].Width, TabHeaders[idx].Height));

                if (this.SelectedIndex == idx)
                    renderer.FillRectangle(this.ForeColor * 0.1f, tabLocation, new Vector2(TabHeaders[idx].Width, TabHeaders[idx].Height));

                renderer.DrawRectangle(this.ForeColor, tabLocation, new Vector2(TabHeaders[idx].Width, TabHeaders[idx].Height));
                renderer.DrawText(panel.Text, this.ForeColor, this.Font, tabLocation + Vector2.UnitX * this.MarginLeft);
                idx++;
            }
        }

        public override void AddChildControl(UI.Control<SharpDXRenderer, Color, Vector2, SharpDX.DirectWrite.TextFormat> control)
        {
            base.AddChildControl(control);
            control.Visible = this.SelectedIndex == this.ChildControls.IndexOf(control);
        }
        #endregion
    }
}
