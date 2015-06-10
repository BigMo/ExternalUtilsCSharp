using ExternalUtilsCSharp.UI.UIObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExternalUtilsCSharp.UI
{
    /// <summary>
    /// A very basic class to use as a base for more complex controls
    /// </summary>
    /// <typeparam name="TRenderer">Renderer-type</typeparam>
    /// <typeparam name="TColor">Color-type</typeparam>
    /// <typeparam name="TVector2">Vector-type</typeparam>
    /// <typeparam name="TFont">Font-type</typeparam>
    public abstract class Control<TRenderer, TColor, TVector2, TFont> where TRenderer : Renderer<TColor, TVector2, TFont>
    {
        #region VARIABLES
        private bool mouseOver;
        #endregion
        #region PROPERTIES
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public TColor BackColor { get; set; }
        public TColor ForeColor { get; set; }
        public TFont Font { get; set; }
        public Rectangle Rectangle { get { return new Rectangle(this.X, this.Y, this.Width, this.Height); } }
        public Control<TRenderer, TColor, TVector2, TFont> Parent { get; set; }
        public List<Control<TRenderer, TColor, TVector2, TFont>> ChildControls { get; set; }
        public string Text { get; set; }
        public bool MouseOver
        {
            get 
            { 
                return mouseOver; 
            }
            protected set 
            {
                if (mouseOver != value)
                {
                    mouseOver = value;
                    if (value)
                        OnMouseEnteredEvent(new EventArgs());
                    else
                        OnMouseLeftEvent(new EventArgs());
                }
            }
        }
        public float MarginTop { get; set; }
        public float MarginBottom { get; set; }
        public float MarginLeft { get; set; }
        public float MarginRight { get; set; }
        public bool Visible { get; set; }
        public bool FillParent { get; set; }
        #endregion
        #region EVENTS
        protected struct MouseEvent
        {
            public bool Handled;
            public int Depth;
        }
        public class MouseClickEventArgs : EventArgs
        {
            public bool LeftButton;
            public bool RightButton;
            public bool MiddleButton;
        }
        public event EventHandler MouseEnteredEvent;
        public event EventHandler MouseLeftEvent;
        public event EventHandler MouseMovedEvent;
        public event EventHandler<MouseClickEventArgs> MouseClickEventDown;
        public event EventHandler<MouseClickEventArgs> MouseClickEventUp;
        protected virtual void OnMouseEnteredEvent(EventArgs e)
        {
            if (MouseEnteredEvent != null)
                MouseEnteredEvent(this, e);
        }
        protected virtual void OnMouseLeftEvent(EventArgs e)
        {
            if (MouseLeftEvent != null)
                MouseLeftEvent(this, e);
        }
        protected virtual void OnMouseMovedEvent(EventArgs e)
        {
            if (MouseMovedEvent != null)
                MouseMovedEvent(this, e);
        }
        protected virtual void OnMouseClickEventDown(MouseClickEventArgs e)
        {
            if (MouseClickEventDown != null)
                MouseClickEventDown(this, e);
        }
        protected virtual void OnMouseClickEventUp(MouseClickEventArgs e)
        {
            if (MouseClickEventUp != null)
                MouseClickEventUp(this, e);
        }
        #endregion
        #region CONSTRUCTOR
        public Control()
        {
            this.X = 0f;
            this.Y = 0f;
            this.Width = 0f;
            this.Height = 0f;
            this.Parent = null;
            this.ChildControls = new List<Control<TRenderer, TColor, TVector2, TFont>>();
            this.Text = "<Control>";
            this.Visible = true;
        }
        #endregion
        #region METHODS
        /// <summary>
        /// Draws the control
        /// </summary>
        /// <param name="renderer"></param>
        public virtual void Draw(TRenderer renderer)
        {
            foreach (Control<TRenderer, TColor, TVector2, TFont> control in ChildControls)
                if(control.Visible)
                    control.Draw(renderer);
        }
        /// <summary>
        /// Performs an update on this control and its chilcontrols
        /// </summary>
        /// <param name="secondsElapsed"></param>
        /// <param name="cursorPoint"></param>
        public virtual void Update(double secondsElapsed, KeyUtils keyUtils, TVector2 cursorPoint)
        {
            #region MOUSE
            MouseEvent result = new MouseEvent() { Handled = false, Depth = 0 };
            CheckMouseEvents(cursorPoint, keyUtils, result);
            #endregion
            #region CHILDCONTROLS
            foreach (Control<TRenderer, TColor, TVector2, TFont> control in ChildControls)
                control.Update(secondsElapsed, keyUtils, cursorPoint);
            #endregion
        }
        /// <summary>
        /// Checks whether the mouse left or entered this control (or one of its childcontrols)
        /// </summary>
        /// <param name="cursorPoint"></param>
        /// <param name="result"></param>
        protected void CheckMouseEvents(TVector2 cursorPoint, KeyUtils keyUtils, MouseEvent result)
        {
            foreach(Control<TRenderer,TColor,TVector2,TFont> control in ChildControls)
            {
                result.Depth++;
                control.CheckMouseEvents(cursorPoint, keyUtils, result);
                result.Depth--;
                if (result.Handled)
                    break;
            }
            if (!result.Handled && result.Depth == 0)
            {
                this.MouseOver = this.CheckMouseOver(cursorPoint);
                if (this.MouseOver)
                {
                    if (keyUtils.KeyWentDown(WinAPI.VirtualKeyShort.LBUTTON))
                        OnMouseClickEventDown(new MouseClickEventArgs() { LeftButton = true });
                    if (keyUtils.KeyWentDown(WinAPI.VirtualKeyShort.RBUTTON))
                        OnMouseClickEventDown(new MouseClickEventArgs() { RightButton = true });
                    if (keyUtils.KeyWentDown(WinAPI.VirtualKeyShort.MBUTTON))
                        OnMouseClickEventDown(new MouseClickEventArgs() { MiddleButton = true });

                    if (keyUtils.KeyWentUp(WinAPI.VirtualKeyShort.LBUTTON))
                        OnMouseClickEventUp(new MouseClickEventArgs() { LeftButton = true });
                    if (keyUtils.KeyWentUp(WinAPI.VirtualKeyShort.RBUTTON))
                        OnMouseClickEventUp(new MouseClickEventArgs() { RightButton = true });
                    if (keyUtils.KeyWentUp(WinAPI.VirtualKeyShort.MBUTTON))
                        OnMouseClickEventUp(new MouseClickEventArgs() { MiddleButton = true });
                    result.Handled = true;
                }
            }
        }
        /// <summary>
        /// Whether the mouse is over this control
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public abstract bool CheckMouseOver(TVector2 cursorPoint);
        /// <summary>
        /// Adds a control to the childcontrols of this control
        /// </summary>
        /// <param name="control"></param>
        public virtual void AddChildControl(Control<TRenderer, TColor, TVector2, TFont> control)
        {
            this.ChildControls.Add(control);
            control.Parent = this;
        }
        /// <summary>
        /// Removes a control from this control's childcontrols
        /// </summary>
        /// <param name="control"></param>
        public virtual void RemoveChildControl(Control<TRenderer, TColor, TVector2, TFont> control)
        {
            this.ChildControls.Remove(control);
            control.Parent = null;
        }
        /// <summary>
        /// Removes a control at the given index from this control's list of childcontrols
        /// </summary>
        /// <param name="index"></param>
        public virtual void RemoveChildControlAt(int index)
        {
            this.RemoveChildControl(this.ChildControls[index]);
        }
        /// <summary>
        /// Returns the location of this control
        /// </summary>
        /// <returns></returns>
        public abstract TVector2 GetLocation();
        /// <summary>
        /// Returns the size of this control
        /// </summary>
        /// <returns></returns>
        public abstract TVector2 GetSize();
        #endregion
    }
}
