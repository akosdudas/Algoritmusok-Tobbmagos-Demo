using System;
using System.Linq;
using System.Threading;

namespace ConcurrentStack.Csharp
{
    class LockFreeStack<T> : IStack<T>
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

        public void Push(T value)
        {
            // new node with the value
            var n = new Node(value);
            while(true)
            {
                var myHead = head;
                n.Next = myHead;
                if (Interlocked.CompareExchange(ref head, n, myHead) == myHead)
                    break;
                // optional backoff
            }
        }

        public bool Pop(out T value)
        {
            if (head == null)
            {
                // stack is empty
                value = default(T);
                return false;
            }
            else
            {
                while (true)
                {
                    var myHead = head;
                    value = myHead.Value;
                    if (Interlocked.CompareExchange(ref head, head.Next, myHead) == myHead)
                        return true;
                    // optional backoff
                }
            }
        }
    }
}
