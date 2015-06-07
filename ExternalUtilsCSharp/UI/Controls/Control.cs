using ExternalUtilsCSharp.UI.UIObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExternalUtilsCSharp.UI.Controls
{
    /// <summary>
    /// A very basic class to use as a base for more complex controls
    /// </summary>
    /// <typeparam name="TColor">Color-type</typeparam>
    /// <typeparam name="TVector2">Vector-type</typeparam>
    /// <typeparam name="TFont">Font-type</typeparam>
    public abstract class Control<TColor, TVector2, TFont>
    {
        #region PROPERTIES
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public TColor BackColor { get; set; }
        public TColor ForeColor { get; set; }
        public TFont Font { get; set; }
        public Rectangle Rectangle { get { return new Rectangle(this.X, this.Y, this.Width, this.Height); } }
        public Control<TColor, TVector2, TFont> Parent { get; set; }
        public List<Control<TColor, TVector2, TFont>> ChildControls { get; set; }
        #endregion

        #region CONSTRUCTOR
        public Control()
        {
            this.X = 0f;
            this.Y = 0f;
            this.Width = 0f;
            this.Height = 0f;
            this.Parent = null;
            this.ChildControls = new List<Control<TColor, TVector2, TFont>>();
        }
        #endregion

        #region METHODS
        public abstract void Draw<TRenderer>(TRenderer renderer) where TRenderer : Renderer<TColor, TVector2, TFont>;
        #endregion
    }
}
