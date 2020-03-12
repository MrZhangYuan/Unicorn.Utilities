﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;

namespace Unicorn.Utilities.Util
{

    public class TimerBuffer<T>
    {
        private DispatcherTimer _dispatcherTimer = null;

        private T _parameter;

        private int _dueTime = 100;
        public int DueTime
        {
            get
            {
                if (this._dispatcherTimer == null)
                {
                    return this._dueTime;
                }
                return this._dispatcherTimer.Interval.Milliseconds;
            }
            set
            {
                this._dueTime = value;

                if (this._dispatcherTimer != null)
                {
                    this._dispatcherTimer.Interval = TimeSpan.FromMilliseconds(this._dueTime);
                }
            }
        }

        public Action<T> Action
        {
            get;
            set;
        }

        private DispatcherPriority _priority;

        public TimerBuffer()
                : this(DispatcherPriority.Normal)
        {
        }

        public TimerBuffer(DispatcherPriority priority)
        {
            this._priority = priority;
        }

        private void _dispatcherTimer_Tick(object sender, EventArgs e)
        {
            this.InvokeAction();
        }

        private void InvokeAction()
        {
            this.Stop();

            this.Action?.Invoke(this._parameter);
        }

        public void ReSet(T parameter)
        {
            this._parameter = parameter;

            //不需延迟，直接调度
            if (this._dueTime <= 0)
            {
                this.Stop();

                System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke((Action)this.InvokeAction, this._priority);
            }
            else
            {
                if (this._dispatcherTimer == null)
                {
                    this._dispatcherTimer = new DispatcherTimer(this._priority);
                    this._dispatcherTimer.Tick += _dispatcherTimer_Tick;
                }

                this._dispatcherTimer.Interval = TimeSpan.FromMilliseconds(this._dueTime);

                this._dispatcherTimer.Start();
            }
        }

        public void Stop()
        {
            this._dispatcherTimer?.Stop();
        }
    }

}
