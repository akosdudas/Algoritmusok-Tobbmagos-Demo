using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixMultiply_NET
{
    /// <summary>
    /// Immutable, generic, restricted to numbers, smart multiply
    /// </summary>
    class Matrix_v2<TData> where TData : struct, IConvertible
    {
        private readonly TData[,] data;

        public Matrix_v2(int row, int col)
        {
            data = new TData[row, col];
        }

        public Matrix_v2(TData[,] data)
        {
            this.data = new TData[data.GetLength(0), data.GetLength(1)];
            for (int i = 0; i < data.GetLength(0); ++i)
                for (int j = 0; j < data.GetLength(1); ++j)
                    this.data[i, j] = data[i, j];
        }

        public int Rows { get { return data.GetLength(0); } }
        public int Columns { get { return data.GetLength(1); } }

        public TData this[int row, int col]
        {
            get { return data[row, col]; }
        }

        public static Matrix_v2<TData> operator *(Matrix_v2<TData> a, Matrix_v2<TData> b)
        {
            if (a.Columns != b.Rows)
                throw new ArgumentException();
            var result = new TData[a.Rows, b.Columns];
            for (int i = 0; i < result.GetLength(0); ++i)
            {
                for (int j = 0; j < result.GetLength(1); ++j)
                {
                    decimal x = 0;
                    for (int k = 0; k < a.Columns; ++k)
                        x += Convert.ToDecimal(a[i, k]) * Convert.ToDecimal(b[k, j]);
                    result[i, j] = (TData)Convert.ChangeType(x, typeof(TData));
                }
            }
            return new Matrix_v2<TData>(result);
        }
    }
}
