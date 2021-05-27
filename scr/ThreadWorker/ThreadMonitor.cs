using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ThreadWorker
{
    public class ThreadMonitor : ThreadedTask
    {
        List<ThreadWorker> workers;
        public ThreadMonitor(List<ThreadWorker> workers)
        {
            this.workers = workers;
        }

        public override string GetName() => "ThreadMonitor";

        public override void Run()
        {
            while(true)
            {
                Console.Clear();
                lock (workers)
                {
                    foreach(var thread in workers)
                    {
                        var task = thread.Task;
                        Console.Write("Thread" +thread.ThreadName);
                        if (task == null)
                            if (thread.Active)
                                Console.WriteLine(" waiting");
                            else
                            {
                                Console.WriteLine(" stopped");
                            }
                        else
                        {
                            if (thread.Active)
                                Console.Write(" active");
                            else
                                Console.Write(" in last task");
                            Console.WriteLine(" : " + task.GetName() + " " + task.GetHashCode());

                        }
                    }
                }
                Thread.Sleep(500);
            }
        }
    }
}
