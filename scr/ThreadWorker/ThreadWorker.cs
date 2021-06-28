using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ThreadWorker
{
    public class ThreadWorker
    {
        private readonly Queue<ThreadedTask> queue;
        public readonly Thread thread;

        public int ThreadName => thread.GetHashCode();
        public ThreadedTask Task {get; private set; }
        public bool Active {get; private set; }
        public ThreadWorker(Queue<ThreadedTask> queue)
        {
            this.queue = queue;
            thread = new Thread(Run){ IsBackground = true };
            Start();
        }

        public void Start()
        {
            if (!thread.IsAlive)
                thread.Start();
        }

        private void Run()
        {
            Active = true;
            while (Active)
            {
                lock(queue)
                {
                    if (queue.Count > 0)
                    {
                        Task = queue.Dequeue();
                    }
                }
                if (Task != null)
                {
                    Task.Run();
                    Task = null;
                }
            }
        }

        public void Stop()
        {
            Active = false;
        }
    }
}
