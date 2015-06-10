using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp
{
    public class Updater
    {
        #region VARIABLES
        private Thread thread;
        private bool work;
        private long lastTick;

        private long fpsTick;
        #endregion

        #region PROPERTIES
        public int Interval { get; set; }
        public int TickCount { get; private set; }
        public int FrameRate { get; private set; }
        public int LastFrameRate { get; private set; }
        #endregion

        #region EVENTS
        public event EventHandler<DeltaEventArgs> TickEvent;
        public virtual void OnTickEvent(DeltaEventArgs e)
        {
            if (TickEvent != null)
                TickEvent(this, e);
        }
        public class DeltaEventArgs : EventArgs
        {
            public double SecondsElapsed { get; private set; }
            public DeltaEventArgs(double secondsElapsed) : base()
            {
                this.SecondsElapsed = secondsElapsed;
            }
        }
        #endregion

        #region CONSTRUCTOR
        public Updater(int tickRate)
        {
            this.Interval = 1000 / tickRate;
            this.TickCount = 0;
        }
        public Updater() : this(60) { }
        #endregion

        #region METHODS

        public void StartUpdater()
        {
            if (thread != null)
                StopUpdater();
            work = true; 
            this.thread = new Thread(new ThreadStart(Loop));
            thread.IsBackground = true;
            thread.Priority = ThreadPriority.Highest;
            thread.Start();
        }

        public void StopUpdater()
        {
            work = false;
            if (thread == null)
                return;
            if (thread.ThreadState == ThreadState.Running)
                thread.Abort();
            thread = null;
        }

        private void Loop()
        {
            lastTick = DateTime.Now.Ticks;
            while (work)
            {
                CalculateFPS();
                this.OnTickEvent(new DeltaEventArgs(new TimeSpan(DateTime.Now.Ticks - lastTick).TotalSeconds));
                this.TickCount++;
                lastTick = DateTime.Now.Ticks;
                Thread.Sleep(Interval);
            }
        }
        #endregion
        public void CalculateFPS()
        {
            if (DateTime.Now.Ticks - fpsTick >= TimeSpan.TicksPerSecond)
            {
                LastFrameRate = FrameRate;
                FrameRate = 0;
                fpsTick = DateTime.Now.Ticks;
            }
            FrameRate++;
        }
    }
}
