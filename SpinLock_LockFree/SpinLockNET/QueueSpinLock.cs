using System;
using System.Threading;

namespace SpinLockNET
{
    class QueueSpinLock
    {
        long current = 1;
        long next = 0;

        public IDisposable Enter()
        {
            long my = Interlocked.Increment(ref next);
            while (true)
            {
                int tries = 100;
                // spin for a while
                while (my != current && tries > 0) { --tries; }
                if (my == current) // spinning stopped because lock acquired 
                    return new LockedRegion(this);
                Thread.Sleep(10); // back off
            }
        }

        public void Exit()
        {
            Interlocked.Increment(ref current);
        }


        private class LockedRegion : IDisposable
        {
            private readonly QueueSpinLock myLock;
            public LockedRegion(QueueSpinLock myLock)
            {
                this.myLock = myLock;
            }
            public void Dispose()
            {
                myLock.Exit();
            }
        }
    }
}
