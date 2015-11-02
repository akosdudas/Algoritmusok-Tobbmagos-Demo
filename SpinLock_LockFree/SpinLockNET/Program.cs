using System;

namespace SpinLockNET
{
    class Program
    {
        static void Main(string[] args)
        {
            Interlocked_Demo.Go();

            var lock1 = new SpinLock();
            using (lock1.Enter())
            {
                // ...
            }

            var lock2 = new SpinLockWithBackoff();
            using (lock2.Enter())
            {
                // ...
            }

            var lock3 = new QueueSpinLock();
            using (lock3.Enter())
            {
                // ...
            }
        }
    }
}
