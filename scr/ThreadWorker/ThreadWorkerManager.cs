using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ThreadWorker
{
    class ThreadWorkerManager : ThreadedTask
    {
        public volatile int MaxPoolSize;
        private List<ThreadWorker> workers;
        private Queue<ThreadedTask> taskQueue;

        public ThreadWorkerManager(int maxPoolSize, Queue<ThreadedTask> taskQueue, List<ThreadWorker> workers)
        {
            MaxPoolSize = maxPoolSize;
            for (var i = 0; i < MaxPoolSize; i++)
                workers.Add(new ThreadWorker(taskQueue));
            this.taskQueue = taskQueue;
            this.workers = workers;
        }

        public override string GetName()
        {
            return "ThreadWorkerManager";
        }

        public override void Run()
        {
            while(true)
            { 
                if(workers.Count < MaxPoolSize)
                {
                    lock (workers)
                    {
                        workers.Add(new ThreadWorker(taskQueue));
                    }
                }
                if(workers.Count > MaxPoolSize)
                {
                    var freeWorker = workers.FirstOrDefault( x => x.Task == null && x.Active);
                    if (freeWorker != null)
                    {
                        freeWorker.Stop();
                    }
                    else
                    {
                        freeWorker = workers.FirstOrDefault(x => x.Active);
                        if (freeWorker != null)     
                            freeWorker.Stop();
                    }
                }
                var uselessWorker = workers.FirstOrDefault( x => x.Task == null && !x.Active);
                if (uselessWorker != null)
                {
                    lock (workers)
                    {
                        workers.Remove(uselessWorker);
                    }
                }
                System.Threading.Thread.Sleep(100);
            }
        }
    }
}
