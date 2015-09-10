using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixMultiply_NET
{
    class Program
    {
        static void Main(string[] args)
        {
            const int row = 1024;
            const int col = 1024;
            var testData1 = CreateTestData(row, col);
            var testData2 = CreateTestData(col, row);

            // class, generic, slow multiply --- slow!!!
            //{
            //    var a = new Matrix_v1<double>(testData1);
            //    var b = new Matrix_v1<double>(testData2);

            //    var start = DateTime.Now;

            //    var c = a * b;

            //    var time = DateTime.Now - start;

            //    Console.WriteLine(time.TotalMilliseconds);
            //}

            // class, generic, smart multiply
            {
                var a = new Matrix_v2<double>(testData1);
                var b = new Matrix_v2<double>(testData2);

                var start = DateTime.Now;

                var c = a * b;

                var time = DateTime.Now - start;

                Console.WriteLine(time.TotalMilliseconds);
            }

            // class, double, smart multiply
            {
                var a = new Matrix_v3(testData1);
                var b = new Matrix_v3(testData2);

                var start = DateTime.Now;

                var c = a * b;

                var time = DateTime.Now - start;

                Console.WriteLine(time.TotalMilliseconds);
            }

            // c-style inline arrays
            {
                var a = testData1;
                var b = testData2;

                var start = DateTime.Now;

                var result = new double[a.GetLength(0), b.GetLength(1)];
                for (int i = 0; i < result.GetLength(0); ++i)
                    for (int j = 0; j < result.GetLength(1); ++j)
                        for (int k = 0; k < a.GetLength(1); ++k)
                            result[i, j] += a[i, k] * b[k, j];

                var time = DateTime.Now - start;

                Console.WriteLine(time.TotalMilliseconds);
            }
            
            // c-style inline arrays, for cycle order changed (memory pattern optimization)
            {
                var a = testData1;
                var b = testData2;

                var start = DateTime.Now;

                var result = new double[a.GetLength(0), b.GetLength(1)];
                for (int i = 0; i < result.GetLength(0); ++i)
                    for (int k = 0; k < a.GetLength(1); ++k)
                        for (int j = 0; j < result.GetLength(1); ++j)
                                result[i, j] += a[i, k] * b[k, j];

                var time = DateTime.Now - start;

                Console.WriteLine(time.TotalMilliseconds);
            }

            // go parallel
            {
                var a = testData1;
                var b = testData2;

                var start = DateTime.Now;

                var result = new double[row, row];
                Parallel.For(0, row, i =>
                {
                    for (int k = 0; k < col; ++k)
                    {
                        for (int j = 0; j < row; ++j)
                            result[i, j] += a[i, k] * b[k, j];
                    }
                });

                var time = DateTime.Now - start;

                Console.WriteLine(time.TotalMilliseconds);
            }
        }

        static double[,] CreateTestData(int row, int col)
        {
            var data = new double[row, col];
            for (int i = 0; i < row; ++i)
                for (int j = 0; j < col; ++j)
                    data[i, j] = (double)(i+1) / (j+1);
            return data;
        }
    }
}
