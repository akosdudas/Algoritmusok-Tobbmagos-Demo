using System;
using System.Linq;
using System.Threading;

namespace ConcurrentStack.Csharp
{
    class LockBasedStack<T> : IStack<T>
    {
        class Node
        {
            /// <summary>
            /// The stored element.
            /// </summary>
            public readonly T Value;
            /// <summary>
            /// Points to the next item in the linked list.
            /// </summary>
            public Node Next = null;

            public Node(T value)
            {
                this.Value = value;
            }
        }

        /// <summary>
        /// First element, or null when stack is empty.
        /// </summary>
        private Node head = null;
        /// <summary>
        /// Spinlock for mutual exclusion.
        /// </summary>
        private SpinLock _lock = new SpinLock();

        public void Push(T value)
        {
            using (_lock.Enter())
            {
                // new node with the value
                var n = new Node(value);
                // new node will point to the current head
                n.Next = head;
                // new head will be the new node
                head = n;
            }
        }

        public bool Pop(out T value)
        {
            using (_lock.Enter())
            {
                if (head == null)
                {
                    // stack is empty
                    value = default(T);
                    return false;
                }
                else
                {
                    // get the return value
                    value = head.Value;
                    // next node replaces the current head
                    head = head.Next;
                    return true;
                }
            }
        }

        class SpinLock
        {
            /// <summary>
            /// 0: free, 1: busy, atomic read-write
            /// </summary>
            private int busy = 0;

            public IDisposable Enter()
            {
                // while the current value is not 0
                while (Interlocked.CompareExchange(ref busy, 1, 0) != 0)
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
}
