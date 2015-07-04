﻿using ExternalUtilsCSharp.UI.UIObjects;
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
        private bool mouseOver, visible;
        private string text;
        private TFont font;
        #endregion

        #region ENUMS
        public enum TextAlignment { Left, Center, Right };
        #endregion

        #region PROPERTIES
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public TColor BackColor { get; set; }
        public TColor ForeColor { get; set; }
        public TFont Font
        {
            get { return this.font; }
            set
            {
                if (this.font == null || !this.font.Equals(value))
                {
                    this.font = value;
                    OnFontChangedEvent(new EventArgs());
                }
            }
        }
        public Rectangle Rectangle { get { return new Rectangle(this.X, this.Y, this.Width, this.Height); } }
        public Control<TRenderer, TColor, TVector2, TFont> Parent { get; set; }
        public List<Control<TRenderer, TColor, TVector2, TFont>> ChildControls { get; set; }
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (this.text != value)
                {
                    this.text = value;
                    OnTextChangedEvent(new EventArgs());
                }
            }
        }
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
        public bool Visible
        {
            get { return this.visible; }
            set
            {
                if (this.visible != value)
                {
                    this.visible = value;
                    OnVisibleChangedEvent(new EventArgs());
                }
            }
        }
        public bool FillParent { get; set; }
        public TVector2 LastMousePos { get; private set; }
        public object Tag { get; set; }
        public TextAlignment TextAlign { get; set; }
        #endregion
        #region EVENTS
        protected struct MouseEvent
        {
            public bool Handled;
            public int Depth;
        }
        public class MouseEventArgs : EventArgs
        {
            public bool LeftButton;
            public bool RightButton;
            public bool MiddleButton;

            public TVector2 Position;
        }
        public event EventHandler MouseEnteredEvent;
        public event EventHandler MouseLeftEvent;
        public event EventHandler TextChangedEvent;
        public event EventHandler FontChangedEvent;
        public event EventHandler VisibleChangedEvent;
        public event EventHandler<MouseEventArgs> MouseMovedEvent;
        public event EventHandler<MouseEventArgs> MouseClickEventDown;
        public event EventHandler<MouseEventArgs> MouseClickEventUp;
        protected virtual void OnTextChangedEvent(EventArgs e)
        {
            if (TextChangedEvent != null)
                TextChangedEvent(this, e);
        }
        protected virtual void OnVisibleChangedEvent(EventArgs e)
        {
            if (VisibleChangedEvent != null)
                VisibleChangedEvent(this, e);
        }
        protected virtual void OnFontChangedEvent(EventArgs e)
        {
            if (FontChangedEvent != null)
                FontChangedEvent(this, e);
        }
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
        protected virtual void OnMouseMovedEvent(MouseEventArgs e)
        {
            if (MouseMovedEvent != null)
                MouseMovedEvent(this, e);
        }
        protected virtual void OnMouseClickEventDown(MouseEventArgs e)
        {
            if (MouseClickEventDown != null)
                MouseClickEventDown(this, e);
        }
        protected virtual void OnMouseClickEventUp(MouseEventArgs e)
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
                if (control.Visible)
                    control.Draw(renderer);
        }
        /// <summary>
        /// Performs an update on this control and its chilcontrols
        /// </summary>
        /// <param name="secondsElapsed"></param>
        /// <param name="cursorPoint"></param>
        public virtual void Update(double secondsElapsed, KeyUtils keyUtils, TVector2 cursorPoint, bool checkMouse = false)
        {
            bool canUpdate = this.visible;
            if (this.Parent != null)
                if (!this.Parent.Visible)
                    canUpdate = false;

            if (canUpdate)
            {
                #region MOUSE
                if (checkMouse)
                {
                    MouseEvent result = new MouseEvent() { Handled = false, Depth = 0 };
                    CheckMouseEvents(cursorPoint, keyUtils, ref result);
                }
                #endregion
                #region CHILDCONTROLS

                foreach (Control<TRenderer, TColor, TVector2, TFont> control in ChildControls)
                    control.Update(secondsElapsed, keyUtils, cursorPoint, false);
                #endregion
            }
        }
        /// <summary>
        /// Checks whether the mouse left or entered this control (or one of its childcontrols)
        /// </summary>
        /// <param name="cursorPoint"></param>
        /// <param name="result"></param>
        protected void CheckMouseEvents(TVector2 cursorPoint, KeyUtils keyUtils, ref MouseEvent result)
        {
            foreach (Control<TRenderer, TColor, TVector2, TFont> control in ChildControls)
            {
                if (result.Handled)
                    return;
                if (!control.Visible)
                    continue;
                result.Depth++;
                control.CheckMouseEvents(cursorPoint, keyUtils, ref result);
                result.Depth--;
            }
            if (!result.Handled)
            {
                this.MouseOver = this.CheckMouseOver(cursorPoint);
                if (this.MouseOver)
                {
                    result.Handled = true;
                    if (keyUtils.KeyWentDown(WinAPI.VirtualKeyShort.LBUTTON))
                        OnMouseClickEventDown(new MouseEventArgs() { LeftButton = true, Position = cursorPoint });
                    if (keyUtils.KeyWentDown(WinAPI.VirtualKeyShort.RBUTTON))
                        OnMouseClickEventDown(new MouseEventArgs() { RightButton = true, Position = cursorPoint });
                    if (keyUtils.KeyWentDown(WinAPI.VirtualKeyShort.MBUTTON))
                        OnMouseClickEventDown(new MouseEventArgs() { MiddleButton = true, Position = cursorPoint });

                    if (keyUtils.KeyWentUp(WinAPI.VirtualKeyShort.LBUTTON))
                        OnMouseClickEventUp(new MouseEventArgs() { LeftButton = true, Position = cursorPoint });
                    if (keyUtils.KeyWentUp(WinAPI.VirtualKeyShort.RBUTTON))
                        OnMouseClickEventUp(new MouseEventArgs() { RightButton = true, Position = cursorPoint });
                    if (keyUtils.KeyWentUp(WinAPI.VirtualKeyShort.MBUTTON))
                        OnMouseClickEventUp(new MouseEventArgs() { MiddleButton = true, Position = cursorPoint });

                    if (!LastMousePos.Equals(cursorPoint))
                    {
                        OnMouseMovedEvent(new MouseEventArgs()
                        {
                            Position = cursorPoint,
                            LeftButton = keyUtils.KeyIsDown(WinAPI.VirtualKeyShort.LBUTTON),
                            RightButton = keyUtils.KeyIsDown(WinAPI.VirtualKeyShort.RBUTTON),
                            MiddleButton = keyUtils.KeyIsDown(WinAPI.VirtualKeyShort.MBUTTON)
                        });
                        LastMousePos = cursorPoint;
                    }
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
            control.Parent = this;
            this.ChildControls.Add(control);
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
        public abstract TVector2 GetAbsoluteLocation();
        /// <summary>
        /// Returns the size of this control
        /// </summary>
        /// <returns></returns>
        public abstract TVector2 GetSize();
        #endregion
    }
}
