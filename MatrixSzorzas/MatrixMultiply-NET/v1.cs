using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixMultiply_NET
{
    /// <summary>
    /// Immutable, generic, restricted to numbers
    /// </summary>
    class Matrix_v1<TData> where TData : struct, IConvertible
    {
        private readonly TData[,] data;

        public Matrix_v1(int row, int col)
        {
            data = new TData[row, col];
        }

        public Matrix_v1(TData[,] data)
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

        public Matrix_v1<TData> Change(int row, int col, TData value)
        {
            var newMatrix = new Matrix_v1<TData>(this.data);
            newMatrix.data[row, col] = value;
            return newMatrix;
        }

        public static Matrix_v1<TData> operator *(Matrix_v1<TData> a, Matrix_v1<TData> b)
        {
            if (a.Columns != b.Rows)
                throw new ArgumentException();
            var result = new Matrix_v1<TData>(a.Rows, b.Columns);
            for (int i = 0; i < result.data.GetLength(0); ++i)
            {
                for (int j = 0; j < result.data.GetLength(1); ++j)
                {
                    decimal x = 0;
                    for (int k = 0; k < a.Columns; ++k)
                        x += Convert.ToDecimal(a[i, k]) * Convert.ToDecimal(b[k, j]);
                    result = result.Change(i, j, (TData)Convert.ChangeType(x, typeof(TData)));
                }
            }
            return result;
        }
    }
}
