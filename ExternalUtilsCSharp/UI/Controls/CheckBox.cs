using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.UI.Controls
{
    public abstract class CheckBox<TRenderer, TColor, TVector2, TFont> : Control<TRenderer, TColor, TVector2, TFont> where TRenderer : Renderer<TColor, TVector2, TFont>
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
                if(isChecked != value)
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
        public CheckBox() : base()
        {
            this.Text = "<CheckBox>";
            this.MouseClickEventUp += CheckBox_MouseClickEventUp;
        }

        protected void CheckBox_MouseClickEventUp(object sender, Control<TRenderer, TColor, TVector2, TFont>.MouseClickEventArgs e)
        {
            this.Checked = !this.Checked;
        }
        #endregion
    }
}
