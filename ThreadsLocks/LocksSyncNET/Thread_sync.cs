using System;
using System.Threading;

namespace LocksSync
{
    class Thread_sync
    {
        private object syncLock = new object();

        void threadFunc()
        {
            lock (syncLock)
            {
                // access shared variables
            }
        }


        private ReaderWriterLock rwLock = new ReaderWriterLock();

        void threadFunc2()
        {
            rwLock.AcquireReaderLock(100);
            try
            {
                // access shared variables
                rwLock.UpgradeToWriterLock(100);
            }
            finally { rwLock.ReleaseWriterLock(); }
        }



        private ReaderWriterLockSlim rwLockSlim = new ReaderWriterLockSlim();

        void threadFunc3()
        {
            if (rwLockSlim.TryEnterReadLock(100))
            {
                try
                {
                    // access shared variables
                }
                finally { rwLockSlim.ExitReadLock(); }
            }
        }
    }
}
