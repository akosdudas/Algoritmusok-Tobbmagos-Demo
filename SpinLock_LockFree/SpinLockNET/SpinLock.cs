using System;
using System.Threading;

namespace SpinLockNET
{
    class SpinLock
    {
        /// <summary>
        /// 0: free, 1: busy, atomic read-write
        /// </summary>
        private int busy = 0;

        public IDisposable Enter()
        {
            // while the current value is not 0
            while(Interlocked.CompareExchange(ref busy, 1, 0) != 0)
            { /* spin */ }
            return new LockedRegion(this);
        }

        public void Exit()
        {
            // write back 0 meaning free
            Interlocked.Exchange(ref busy, 0);
        }

        private class LockedRegion : IDisposable
        {
            private readonly SpinLock myLock;
            public LockedRegion(SpinLock myLock)
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
