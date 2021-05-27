using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using System.Linq;

namespace ThreadWorker
{
    public class ThreadDispatcher
    {
        private static ThreadDispatcher dispatcher;
        public int MaxPoolSize 
        { 
            get => workersManager.MaxPoolSize;

           set 
           {
                if (value < 1)
                    throw new ArgumentException();
                workersManager.MaxPoolSize = value;
           } 
        }
        List<ThreadWorker> workers = new List<ThreadWorker>();
        ThreadWorkerManager workersManager;
        Queue<ThreadedTask> taskQueue = new Queue<ThreadedTask>();
        private ThreadDispatcher(int maxPoolSize)
        {
            workersManager = new ThreadWorkerManager(maxPoolSize, taskQueue, workers);
            AddInQueue(workersManager);
            AddInQueue(new ThreadMonitor(workers));
        }

        public static ThreadDispatcher GetInstance()
        {
            if (dispatcher == null)
                    dispatcher = new ThreadDispatcher(4);
                return dispatcher;
            
        }

        public void Add(ThreadedTask task)
        {
            Thread thread = new Thread(task.Run);
            thread.Start();
        }

		public void AddInQueue(ThreadedTask task)
        {
            taskQueue.Enqueue(task);
        }
    }
}
