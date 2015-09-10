#include "cuda_runtime.h"
#include "device_launch_parameters.h"
#include <stdio.h>
#include <Windows.h>

__global__ void matrixMultiplyKernel(float* result, const float* a, const float* b, const int row, const int col)
{
	int i = blockIdx.x;
    int j = threadIdx.x;

	float temp = 0;
	for (int k = 0; k < col; ++k)
		temp += a[i*col+k] * b[k*row+j];
	result[i*col+j] = temp;
}

float* createTestData( int row, int col )
{
	float* data = new float[row*col];
	for (int i = 0; i < row; ++i)
		for (int j = 0; j < col; ++j)
			data[i*col+j] = (float)(i+1) / (j+1);
	return data;
}

void main()
{
	const int row = 1024;
	const int col = 1024;

	float* a = createTestData(row, col);
	float* b = createTestData(col, row);

	float* result = new float[row*row];

	unsigned __int64 start1 = GetTickCount64();

    float* dev_a = 0;
    float* dev_b = 0;
    float* dev_result = 0;

    cudaMalloc((void**)&dev_a, row*col*sizeof(float));
    cudaMalloc((void**)&dev_b, col*row*sizeof(float));
    cudaMalloc((void**)&dev_result, row*row*sizeof(float));
    cudaMemcpy(dev_a, a, row*col*sizeof(float), cudaMemcpyHostToDevice);
    cudaMemcpy(dev_b, b, col*row*sizeof(float), cudaMemcpyHostToDevice);

	unsigned __int64 start2 = GetTickCount64();

    matrixMultiplyKernel<<<row,row>>>(dev_result, dev_a, dev_b, row, col);

    cudaDeviceSynchronize();

	printf( "%d\n", GetTickCount64() - start2 );

	cudaMemcpy(result, dev_result, row*row*sizeof(float), cudaMemcpyDeviceToHost);

	delete[] result;
    cudaFree(dev_result);
    cudaFree(dev_a);
    cudaFree(dev_b);
    
	printf( "%d\n", GetTickCount64() - start1 );

	cudaDeviceReset();

	delete[] a;
	delete[] b;
}
