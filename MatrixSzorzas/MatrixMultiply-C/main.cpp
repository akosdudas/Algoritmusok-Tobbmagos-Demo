#include <stdio.h>
#include <stdlib.h>
#include <limits.h>
#include <Windows.h>
#include <mmintrin.h>
#include <xmmintrin.h>

double* createTestData( int row, int col )
{
	double* data = new double[row, col];
	for (int i = 0; i < row; ++i)
		for (int j = 0; j < col; ++j)
			data[i, j] = (double)(i+1) / (j+1);
	return data;
}

float* createTestDataF( int row, int col )
{
	float* data = new float[row, col];
	for (int i = 0; i < row; ++i)
		for (int j = 0; j < col; ++j)
			data[i, j] = (float)(i+1) / (j+1);
	return data;
}

void main()
{
	const int row = 1024;
	const int col = 1024;

	double* a = createTestData(row, col);
	double* b = createTestData(col, row);
	float* af = createTestDataF(row, col);
	float* bf = createTestDataF(col, row);

	// simple arrays
	{
		unsigned __int64 start = GetTickCount64();

		double* result = new double[row, row];
		for (int i = 0; i < row; ++i)
		{
			for (int j = 0; j < row; ++j)
			{
				double temp = 0;
				for (int k = 0; k < col; ++k)
					temp += a[i, k] * b[k, j];
				result[i, j] = temp;
			}
		}

		printf( "%d\n", GetTickCount64() - start );

		delete[] result;
	}


	// vectorization using SSE
	{
		unsigned __int64 start = GetTickCount64();

		float* result = new float[row, row];
		float* q = (float*)_aligned_malloc( 4*sizeof(float), 16 );
		for (int i = 0; i < row; ++i)
		{
			for (int j = 0; j < row; ++j)
			{
				__m128 sum = _mm_setzero_ps();
				for (int k = 0; k < col; k+=4)
				{
					__m128 x = _mm_set_ps( af[i,k], af[i,k+1], af[i,k+2], af[i,k+3] );
					__m128 y = _mm_set_ps( bf[k,j], bf[k+1,j], bf[k+2,j], bf[k+3,j] );
					__m128 mul = _mm_mul_ps( x, y );
					sum = _mm_add_ps( sum, mul );
				}
				_mm_store_ps(q, sum);
				result[i, j] = q[0];
				result[i, j] += q[1];
				result[i, j] += q[2];
				result[i, j] += q[3];
			}
		}

		printf( "%d\n", GetTickCount64() - start );

		_aligned_free( q );
		delete[] result;
	}


	// OpenMP parallel w/ simple arrays
	{
		unsigned __int64 start = GetTickCount64();

		double* result = new double[row, row];
		#pragma omp parallel for shared(a,b,result)
		for (int i = 0; i < row; ++i)
		{
			for (int j = 0; j < row; ++j)
			{
				double temp = 0;
				for (int k = 0; k < col; ++k)
					temp += a[i, k] * b[k, j];
				result[i, j] = temp;
			}
		}

		printf( "%d\n", GetTickCount64() - start );

		delete[] result;
	}


	// OpenMP parallel w/ vectorization using SSE
	{
		unsigned __int64 start = GetTickCount64();

		float* result = new float[row, row];
		#pragma omp parallel for shared(a,b,result)
		for (int i = 0; i < row; ++i)
		{
			float* q = (float*)_aligned_malloc( 4*sizeof(float), 16 );
			for (int j = 0; j < row; ++j)
			{
				__m128 sum = _mm_setzero_ps();
				for (int k = 0; k < col; k+=4)
				{
					__m128 x = _mm_set_ps( af[i,k], af[i,k+1], af[i,k+2], af[i,k+3] );
					__m128 y = _mm_set_ps( bf[k,j], bf[k+1,j], bf[k+2,j], bf[k+3,j] );
					__m128 mul = _mm_mul_ps( x, y );
					sum = _mm_add_ps( sum, mul );
				}
				_mm_store_ps(q, sum);
				result[i, j] = q[0];
				result[i, j] += q[1];
				result[i, j] += q[2];
				result[i, j] += q[3];
			}
			_aligned_free( q );
		}

		printf( "%d\n", GetTickCount64() - start );

		delete[] result;
	}

	delete[] a;
	delete[] b;
}