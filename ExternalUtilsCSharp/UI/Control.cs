using System;
using System.Collections.Generic;
using System.Text;

namespace ExternalUtilsCSharp.UI
{
    /// <summary>
    /// A very basic class to use as a base for more complex controls
    /// </summary>
    public abstract class Control
    {
        #region PROPERTIES
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public Rectangle Rectangle { get { return new Rectangle(this.X, this.Y, this.Width, this.Height); } }
        public Control Parent { get; set; }
        public Control[] ChildControls { get; set; }
        #endregion

        #region METHODS
        public abstract void Draw(object drawContext);
        #endregion
    }
}
