using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConcurrentStack.Csharp
{
    class Stack<T> : IStack<T>
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
            // new node will point to the current head
            n.Next = head;
            // new head will be the new node
            head = n;
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
                // get the return value
                value = head.Value;
                // next node replaces the current head
                head = head.Next;
                return true;
            }
        }
    }
}
