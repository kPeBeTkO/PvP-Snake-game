using System;
using System.Collections.Generic;
using System.Text;

namespace ThreadWorker
{
    public abstract class ThreadedTask
    {
        public abstract string GetName();
        public abstract void Run();
    }
}
