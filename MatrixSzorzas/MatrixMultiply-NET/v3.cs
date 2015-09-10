using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixMultiply_NET
{
    /// <summary>
    /// Immutable, double type, smart multiply
    /// </summary>
    class Matrix_v3
    {
        private readonly double[,] data;

        public Matrix_v3(int row, int col)
        {
            data = new double[row, col];
        }

        public Matrix_v3(double[,] data)
        {
            this.data = new double[data.GetLength(0), data.GetLength(1)];
            for (int i = 0; i < data.GetLength(0); ++i)
                for (int j = 0; j < data.GetLength(1); ++j)
                    this.data[i, j] = data[i, j];
        }

        public int Rows { get { return data.GetLength(0); } }
        public int Columns { get { return data.GetLength(1); } }

        public double this[int row, int col]
        {
            get { return data[row, col]; }
        }

        public static Matrix_v3 operator *(Matrix_v3 a, Matrix_v3 b)
        {
            if (a.Columns != b.Rows)
                throw new ArgumentException();
            var result = new double[a.Rows, b.Columns];
            for (int i = 0; i < result.GetLength(0); ++i)
                for (int j = 0; j < result.GetLength(1); ++j)
                    for (int k = 0; k < a.Columns; ++k)
                        result[i, j] += a[i, k] * b[k, j];
            return new Matrix_v3(result);
        }
    }
}
