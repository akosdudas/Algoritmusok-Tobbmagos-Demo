using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConcurrentStack.Csharp
{
    class StackWithArray<T> : IStack<T>
    {
        private int current = 0;
        private T[] items = new T[128];

        public void Push(T value)
        {
            if (current == items.Length)
            {
                // resize: allocate double the array, copy
                var newItems = new T[items.Length * 2];
                items.CopyTo(newItems, 0);
                items = newItems;
            }
            items[current++] = value;
        }

        public bool Pop(out T value)
        {
            if (current == 0)
            {
                // stack is empty
                value = default(T);
                return false;
            }
            else
            {
                // get the return value
                value = items[--current];
                return true;
            }
        }
    }
}
