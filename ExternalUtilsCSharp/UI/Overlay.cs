using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExternalUtilsCSharp.UI
{
    /// <summary>
    /// An abstract class which implements the basics of an external overlay(-window)
    /// </summary>
    /// <typeparam name="TColor">Color-type</typeparam>
    /// <typeparam name="TVector2">Vector2-type</typeparam>
    /// <typeparam name="TFont">Font-type</typeparam>
    public abstract class Overlay<TColor, TVector2, TFont> : Form
    {
        #region VARIABLES
        private Timer ctrlTimer;
        private long lastTimerTick, lastDrawTick;
        #endregion

        #region PROPERTIES
        public Renderer<TColor, TVector2, TFont> Renderer { get; protected set; }
        /// <summary>
        /// Whether to perform drawing-operations when the target-window is in foreground only
        /// </summary>
        public bool DrawOnlyWhenInForeground { get; set; }
        public IntPtr hWnd { get; protected set; }
        #endregion

        #region EVENTS
        public event EventHandler TickEvent;

        public virtual void OnTickEvent(DeltaEventArgs e)
        {
            if (TickEvent != null)
                TickEvent(this, e);
        }
        public class DeltaEventArgs : EventArgs
        {
            public double SecondsElapsed { get; private set; }
            public DeltaEventArgs(double secondsElapsed)
                : base()
            {
                this.SecondsElapsed = secondsElapsed;
            }
        }
        public event EventHandler DrawEvent;

        public virtual void OnDrawEvent(DrawEventArgs e)
        {
            if (DrawEvent != null)
                DrawEvent(this, e);
        }
        public class DrawEventArgs : DeltaEventArgs
        {
            public double SecondsElapsed { get; private set; }
            public Renderer<TColor, TVector2, TFont> Renderer { get; private set; }
            public DrawEventArgs(double secondsElapsed, Renderer<TColor, TVector2, TFont> renderer)
                : base(secondsElapsed)
            {
                this.Renderer = renderer;
            }
        }
        #endregion

        #region CONSTUCTOR
        public Overlay()
        {
            //Setup form-properties
            this.BackColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "";
            this.Name = "";
            this.TransparencyKey = System.Drawing.Color.Black;
            this.TopMost = true;

            //Make form transparent and fully topmost
            int initialStyle = WinAPI.GetWindowLong(this.Handle, -20);
            WinAPI.SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);
            WinAPI.SetWindowPos(this.Handle, (IntPtr)WinAPI.SetWindpwPosHWNDFlags.TopMost, 0, 0, 0, 0, (uint)(WinAPI.SetWindowPosFlags.NOMOVE | WinAPI.SetWindowPosFlags.NOSIZE));
        
            //Controls
            ctrlTimer = new Timer();
            ctrlTimer.Interval = 1000 / 60;
            ctrlTimer.Tick += ctrlTimer_Tick;
            lastTimerTick = Environment.TickCount;
            lastDrawTick = Environment.TickCount;

            //Overlay-properties
            this.DrawOnlyWhenInForeground = true;
        }

        void ctrlTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan deltaDraw = new TimeSpan(Environment.TickCount - lastDrawTick);
            TimeSpan deltaTimer = new TimeSpan(Environment.TickCount - lastTimerTick);

            lastTimerTick = Environment.TickCount;
            this.OnTick(deltaTimer.TotalSeconds);

            lastDrawTick = Environment.TickCount;
            this.OnDraw(deltaDraw.TotalSeconds);
        }
        #endregion

        #region METHODS
        /// <summary>
        /// Perform your drawing-operations here
        /// </summary>
        /// <param name="seconds">Time in seconds since the last Draw-call</param>
        protected virtual void OnDraw(double seconds)
        {
            if (this.DrawOnlyWhenInForeground)
            {
                if (WinAPI.GetForegroundWindow() != this.hWnd)
                    return;
            }
            OnDrawEvent(new DrawEventArgs(seconds, this.Renderer));
        }
        /// <summary>
        /// Perform your logic-operations here
        /// </summary>
        /// <param name="seconds">Time in seconds since the last Tick-call</param>
        protected virtual void OnTick(double seconds) 
        {
            OnTickEvent(new DeltaEventArgs(seconds));
        }
        /// <summary>
        /// Attach to the given window-handle (and initialize your device here)
        /// </summary>
        /// <param name="hWnd">Handle to the window to attach to</param>
        public abstract void Attach(IntPtr hWnd);
        /// <summary>
        /// Detach from the window-handle which was earler attached to (and destroy your device here)
        /// </summary>
        public abstract void Detach();
        #endregion
    }
}
