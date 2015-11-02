using System;
using System.Collections.Generic;
using System.Threading;

namespace LocksSync
{
    class Thread_create
    {
        public static void Go()
        {
            var threads = new List<Thread>();
            for (int i = 0; i < 5; ++i)
            {
                var th = new Thread(threadFunc);
                th.Start();
                threads.Add(th);
            }
            foreach (var th in threads)
                th.Join();
        }

        static void threadFunc(object state)
        {
            Console.WriteLine("start");
            Thread.Sleep(1000);
            Console.WriteLine("end");
        }
    }
}
