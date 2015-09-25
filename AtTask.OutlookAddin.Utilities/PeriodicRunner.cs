using System;
using System.Timers;

namespace AtTask.OutlookAddIn.Utilities
{
    public enum State { Running, Stopped }

    public class PeriodicRunner : IPeriodicRunner
    {    
        private Action callbackAction;
        private Timer timer;

        private long interval;
        private long initialDelay;

        /// <summary>
        /// Create an _instance, which will periodically call callbackAction after interval.
        /// </summary>
        /// <param name="interval">in milliseconds</param>
        /// <param name="callbackAction">action with object argument</param>
        public PeriodicRunner(long interval, Action callbackAction, long initialDelay = 0)
        {
            this.callbackAction = callbackAction;
            this.interval = interval;
            this.initialDelay = initialDelay;

            if (initialDelay == 0)
            {
                this.timer = new Timer(interval);
            }
            else
            {
                this.timer = new Timer(initialDelay);
            }
            this.timer.Elapsed += TimerCallback;
        }

        /// <summary>
        /// Runs the _instance.
        /// </summary>
        public virtual void Run()
        {
            if (this.initialDelay == 0)
            {
                //Invokes the first time
                this.callbackAction.Invoke();
            }
            this.timer.Start();
        }

        private void TimerCallback(object stateInfo, ElapsedEventArgs e)
        {
            if (this.initialDelay != 0)
            {
                this.timer.Interval = this.interval;
                this.initialDelay = 0;
            }

            this.callbackAction.Invoke();
        }

        /// <summary>
        /// Stops the _instance
        /// </summary>
        public void Stop()
        {
            this.timer.Stop();
        }
    }
}