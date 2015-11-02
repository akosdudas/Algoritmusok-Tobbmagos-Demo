using System;
using System.Threading;
using System.Threading.Tasks;

namespace SpinLockNET
{
    class Interlocked_Demo
    {
        public static void Go()
        {
            int[] vector = new int[1000];
            for (int i = 0; i < vector.Length; ++i) vector[i] = i;
            int sum = 0;
            int max = -1;

            Parallel.For(0, vector.Length,
                i => Interlocked.Add(ref sum, vector[i]));

            Parallel.For(0, vector.Length,
                i =>
                {
                    if (vector[i] > max)
                    {
                        do
                        {
                            int myMax = max;
                            if (vector[i] > myMax)
                            {
                                if (Interlocked.CompareExchange(ref max, vector[i], myMax) == myMax)
                                    break;
                            }
                        }
                        while (true);
                    }
                });

            Console.WriteLine(sum);
            Console.WriteLine(max);
        }
    }
}
