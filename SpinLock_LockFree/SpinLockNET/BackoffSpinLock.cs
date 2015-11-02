using System;
using System.Threading;

namespace SpinLockNET
{
    class SpinLockWithBackoff
    {
        /// <summary>
        /// 0: free, 1: busy, atomic read-write
        /// </summary>
        private int busy = 0;
        /// <summary>
        /// random for backoff delay
        /// </summary>
        private readonly Random rnd = new Random();

        public IDisposable Enter()
        {
            int backoffMax = 5;
            while (true)
            {
                // while the current value is not 0
                if (Interlocked.CompareExchange(ref busy, 1, 0) == 0)
                    break;
                // back off for a random time
                Thread.Sleep(rnd.Next(backoffMax));
                backoffMax *= 2;
            }
            return new LockedRegion(this);
        }

        public void Exit()
        {
            // write back 0 meaning free
            Interlocked.Exchange(ref busy, 0);
        }

        private class LockedRegion : IDisposable
        {
            private readonly SpinLockWithBackoff myLock;
            public LockedRegion(SpinLockWithBackoff myLock)
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
