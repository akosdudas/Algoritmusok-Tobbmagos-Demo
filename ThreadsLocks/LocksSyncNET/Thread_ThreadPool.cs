using System;
using System.Threading;

namespace LocksSync
{
    class Thread_ThreadPool
    {
        class Job { public int Input; public int Output; }

        public static void Go()
        {
            for (int i = 0; i < 20; ++i)
                ThreadPool.QueueUserWorkItem(threadFunc, new Job() { Input = i });
            Thread.Sleep(5000);
        }

        static void threadFunc(object state)
        {
            var myJob = (Job)state;
            myJob.Output = 2 * myJob.Input;
            Thread.Sleep(100);
            Console.WriteLine("fin");
        }
    }
}
