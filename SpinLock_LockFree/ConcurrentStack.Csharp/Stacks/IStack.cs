using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConcurrentStack.Csharp
{
    interface IStack<T>
    {
        void Push(T value);
        bool Pop(out T value);
    }
}
