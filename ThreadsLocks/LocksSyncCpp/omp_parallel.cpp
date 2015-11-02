#include <stdio.h>
#include <omp.h>

int main3()
{
	#pragma omp parallel num_threads(4)
	{
		int i = omp_get_thread_num();
		printf("Hello from thread %d\n", i);
	}
	return 0;
}