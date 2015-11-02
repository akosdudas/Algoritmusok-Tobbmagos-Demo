using System;
using System.Threading;

namespace LocksSync
{
    class Volatile
    {
        /*volatile*/ bool stop = false;
        public void Go()
        {
            new Thread((x) =>
                {
                    int i = 0;
                    while (!stop)
                        ++i;
                    Console.WriteLine("done");
                }).Start();

            Thread.Sleep(1000);
            stop = true;
        }
    }
}
