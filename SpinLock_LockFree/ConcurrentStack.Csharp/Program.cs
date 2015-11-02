using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ConcurrentStack.Csharp
{
    class Program
    {
        static void Main(string[] args)
        {
            testStack(new Stack<int>(), false);

            testStack(new StackWithArray<int>(), false);

            testStack(new LockBasedStack<int>());

            testStack(new LockFreeStack<int>());

            testStack(new BalancerTreeStack<int>());

            {
                var s = new Pool<int>(1);
                for (int i = 0; i < 1000; ++i)
                    s.Push(i, 0);
                for (int i = 999; i >= 0; --i)
                {
                    int v;
                    Debug.Assert(s.Pop(out v, 0));
                    Debug.Assert(v == i);
                }

                s = new Pool<int>(2);
                Parallel.Invoke(
                    () =>
                    {
                        for (int i = 0; i < 1000000; ++i)
                            s.Push(i, 0);
                    },
                    () =>
                    {
                        int v;
                        for (int i = 0; i < 1000000; ++i)
                            s.Pop(out v, 0);
                    },
                    () =>
                    {
                        for (int i = 0; i < 1000000; ++i)
                            s.Push(i, 1);
                    },
                    () =>
                    {
                        int v;
                        for (int i = 0; i < 1000000; ++i)
                            s.Pop(out v, 1);
                    }
                    );
            }

            {
                var s = new LockFreeBag<int>();
                for (int i = 0; i < 1000; ++i)
                    s.Add(i);
                for (int i = 999; i >= 0; --i)
                {
                    int v;
                    Debug.Assert(s.TryTake(out v));
                    Debug.Assert(v == i);
                }

                s = new LockFreeBag<int>();
                Parallel.Invoke(
                    () =>
                    {
                        for (int i = 0; i < 1000000; ++i)
                            s.Add(i);
                    },
                    () =>
                    {
                        int v;
                        for (int i = 0; i < 1000000; ++i)
                            s.TryTake(out v);
                    },
                    () =>
                    {
                        for (int i = 0; i < 1000000; ++i)
                            s.Add(i);
                    },
                    () =>
                    {
                        int v;
                        for (int i = 0; i < 1000000; ++i)
                            s.TryTake(out v);
                    }
                    );
            }

            Console.WriteLine("done");
        }

        static void testStack(IStack<int> s, bool parallel = true)
        {
            for (int i = 0; i < 1000; ++i)
                s.Push(i);
            for (int i = 999; i >= 0; --i)
            {
                int v;
                Debug.Assert(s.Pop(out v));
                Debug.Assert(v == i);
            }

            if (!parallel)
                return;

            Parallel.Invoke(
                () =>
                {
                    for (int i = 0; i < 1000000; ++i)
                        s.Push(i);
                },
                () =>
                {
                    int v;
                    for (int i = 0; i < 1000000; ++i)
                        s.Pop(out v);
                });
        }
    }
}
