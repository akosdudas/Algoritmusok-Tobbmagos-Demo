using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConcurrentStack.Csharp
{
    class Pool<T>
    {
        private readonly int threadNum;
        private readonly LockFreeStack<T>[] stacks;

        public Pool(int threads)
        {
            threadNum = threads;
            stacks = new LockFreeStack<T>[threadNum];
            for (int i = 0; i < threadNum; ++i)
                stacks[i] = new LockFreeStack<T>();
        }

        public void Push(T value, int threadId)
        {
            System.Diagnostics.Debug.Assert(threadId >= 0 && threadId < threadNum);
            stacks[threadId].Push(value);
        }

        public bool Pop(out T value, int threadId)
        {
            System.Diagnostics.Debug.Assert(threadId >= 0 && threadId < threadNum);
            if (stacks[threadId].Pop(out value))
                return true;
            var rnd = new Random();
            for (int i = 0; i < threadNum; ++i)
            {
                var q = rnd.Next(0, threadNum - 1);
                if (stacks[q].Pop(out value))
                    return true;
            }
            return false;
        }
    }
}
