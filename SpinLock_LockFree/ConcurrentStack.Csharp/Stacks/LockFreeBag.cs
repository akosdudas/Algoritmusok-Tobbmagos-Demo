using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConcurrentStack.Csharp
{
    class LockFreeBag<T>
    {
        private System.Threading.SpinLock globalLock = new System.Threading.SpinLock();
        private List<LockFreeStack<T>> allLists = new List<LockFreeStack<T>>();

        private ThreadLocal<LockFreeStack<T>> myList = new ThreadLocal<LockFreeStack<T>>();

        public void Add(T value)
        {
            if (!myList.IsValueCreated)
            {
                myList.Value = new LockFreeStack<T>();
                bool lockTaken = false;
                try
                {
                    globalLock.Enter(ref lockTaken);
                    allLists.Add(myList.Value);
                }
                finally { if (lockTaken) globalLock.Exit(); }
            }
            myList.Value.Push(value);
        }

        public bool TryTake(out T value)
        {
            if(myList.IsValueCreated && myList.Value.Pop(out value))
                return true;

            LockFreeStack<T>[] allListsCache;
            bool lockTaken = false;
            try
            {
                globalLock.Enter(ref lockTaken);
                allListsCache = allLists.ToArray();
            }
            finally { if (lockTaken) globalLock.Exit(); }

            for (int i = 0; i < allListsCache.Length; ++i)
            {
                if (allListsCache[i].Pop(out value))
                    return true;
            }

            value = default(T);
            return false;
        }
    }
}
