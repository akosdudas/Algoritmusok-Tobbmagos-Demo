#include <stdio.h>
#include <omp.h>

int main2()
{
	const int size = 1000;
	int* vector = new int[size];
	for( int i = 0; i < size; ++i ) vector[i] = i;
	int max = 0;

	#pragma omp parallel for
    for (int i = 0; i < size; i++) 
    {
        if (vector[i] > max)
        {
            #pragma omp critical
            {
                if (vector[i] > max)
                    max = vector[i];
            }
        }
    }
	
	printf( "%d\n", max );
	delete[] vector;
	return 0;
}