using System;
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;

namespace Unicorn.Utilities.Util
{
    public class TimerBufferAsync<T>
    {
        private Timer _timer = null;

        private T _parameter;

        private int _dueTime = 100;
        public int DueTime
        {
            get
            {
                return this._dueTime;
            }
            set
            {
                if (this._timer != null)
                {
                    this._timer.Change(value, Timeout.Infinite);
                }
                this._dueTime = value;
            }
        }

        public TimerBufferAsync()
        {

        }

        public Action<T> Action
        {
            get;
            set;
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

                Task.Factory.StartNew(this.InvokeAction);
            }
            else
            {
                if (this._timer == null)
                {
                    this._timer = new Timer(_p=> 
                    {
                        this.InvokeAction();
                    });
                }

                this._timer.Change(this._dueTime, Timeout.Infinite);
            }
        }

        public void Stop()
        {
            this._timer?.Change(Timeout.Infinite,Timeout.Infinite);
        }
    }
}
