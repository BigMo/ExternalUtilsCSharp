using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.SharpDXRenderer.Controls
{
    public abstract class SharpDXCheckable : SharpDXControl
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
    }
}
