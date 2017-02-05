using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Prost.Runtime
{
    public class ReusableThread
    {
        ManualResetEvent oSignalEvent = new ManualResetEvent(false);
        private Thread th;
        private Object thLock = new object();

        bool abort = false;
        bool running = false;
        WaitCallback currentTask;
        Object currentTaskParam;

        public ReusableThread()
        {
        }

        private void InternalTarget()
        {
            running = false;

            while (!abort)
            {
                try
                {
                    this.oSignalEvent.WaitOne();
                    this.oSignalEvent.Reset();
                    this.running = true;
                    try
                    {
                        currentTask(currentTaskParam);
                    }
                    catch (Exception e)
                    {
                        // TODO: 
                    }
                    this.running = false;
                    this.oSignalEvent.Set();
                    this.oSignalEvent.Reset();
                }
                catch (ThreadInterruptedException)
                {
                    this.running = false;
                    this.abort = true;
                }
            }
        }

        public void Run(WaitCallback task, Object param = null)
        {
            if (th == null || !th.IsAlive)
            {
                lock (this.thLock)
                {
                    th = new Thread(new ThreadStart(InternalTarget));
                    th.Start();
                }
            }
            
            if (!this.running)
            {
                this.currentTask = task;
                this.currentTaskParam = param;
                this.oSignalEvent.Set();
            }
            else
            {
                throw new ThreadStateException("The current thread is executing another task.");
            }
        }

        public bool IsAlive { get { return this.th.IsAlive; } }
        public void Stop(bool wait)
        {
            lock (this.thLock)
            {
                if (this.th != null)
                {
                    if (wait && running)
                    {
                        this.abort = true;
                        this.oSignalEvent.WaitOne();
                    }
                    this.th.Interrupt();
                    this.th.Abort();
                    if (wait) this.th.Join();
                }
            }
        }
    }
}
