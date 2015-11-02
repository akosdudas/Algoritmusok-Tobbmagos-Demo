#include <stdio.h>
#include <omp.h>

int main4()
{
	const int size = 1000;
	int* vector = new int[size];
	for( int i = 0; i < size; ++i ) vector[i] = 1;
	int sum = 0;

	#pragma omp parallel reduction(+:sum)
	{
		int threadId = omp_get_thread_num();
		int blockSize = size / omp_get_num_threads();
		int start = threadId * blockSize;
		for( int i = start; i < start + blockSize; ++i )
			sum += vector[i];
	}
	printf( "%d\n", sum );
	delete[] vector;
	return 0;
}