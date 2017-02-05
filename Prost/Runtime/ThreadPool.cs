using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Prost.Runtime
{
    public class ThreadPoolFullException : Exception {
        public ThreadPoolFullException() : base("Pool don't have idle threads in this moment") { }
    }

    public class ThreadPool
    {
        private uint maxThreads;

        private List<ReusableThread> threads = new List<ReusableThread>();
        private Queue<ReusableThread> waitThreads = new Queue<ReusableThread>();

        public ThreadPool(uint maxThreads)
        {
            this.maxThreads = maxThreads;
        }

        ~ThreadPool()
        {
            Stop();
        }

        public void Stop(bool wait = false)
        {
            foreach (ReusableThread th in threads) { if (th.IsAlive) { th.Stop(wait); } }
            waitThreads.Clear();
            threads.Clear();
        }

        public uint MaxThreads { get { return this.maxThreads; } set { this.maxThreads = value; } }
        public uint AvailableThreads { get { return (uint)(this.maxThreads - (this.threads.Count - this.waitThreads.Count)); } }

        public void Run(WaitCallback task, Object param = null)
        {
            lock(this.threads)
            {
                if (AvailableThreads > 0)
                {
                    ReusableThread th;

                    if (this.waitThreads.Count > 0)
                    {
                        th = waitThreads.Dequeue();
                    }
                    else
                    {
                        th = new ReusableThread();
                        this.threads.Add(th);
                    }

                    th.Run((_) =>
                    {
                        task(param);
                        this.waitThreads.Enqueue(th);
                    });
                }
                else
                {
                    throw new ThreadPoolFullException();
                }
            }
        }
    }
}
