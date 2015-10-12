#include <stdio.h>
#include <stdlib.h>
#include <algorithm>

#define NOMINMAX
#include <windows.h>


__int64 startTimer()
{
	__int64 timerStart;
	QueryPerformanceCounter((LARGE_INTEGER*)&timerStart); // gets the processor tick counter at the time of query
	return timerStart;
}

double endTimer(__int64 timerStart)
{
	__int64 endTime;
	__int64 procFreq;
	double interval;
	QueryPerformanceCounter((LARGE_INTEGER*)&endTime); // gets the processor tick counter at the time of query
	QueryPerformanceFrequency((LARGE_INTEGER*)&procFreq); // processor frequency (number of ticks per second)
	interval = endTime - timerStart; // number of elapses ticks
	interval = (interval * 1000) / procFreq; // result will be milliseconds
	return interval;
}

void ilp()
{
	// prepare input data
	const int size = 2000000;
	int* t = new int[size];
	for (int i = 0; i < size; ++i)
		t[i] = rand();

	int max = 0;
	__int64 timerStart;
	double elapsedMilliseconds;



	// 1. no ILP
	printf("ILP: no ILP\n");
	timerStart = startTimer();
	for( int r = 0; r < 1000; ++r )
	{
		max = t[0];
		for (int i = 1; i < size; ++i)
			max = std::max(max, t[i]);
	}
	elapsedMilliseconds = endTimer(timerStart);
	printf("exec time: %lf ms\n", elapsedMilliseconds);
	printf("max: %d\n", max);


	// 2. good ILP
	printf("ILP: good ILP\n");
	timerStart = startTimer();
	for (int r = 0; r < 1000; ++r)
	{
		int m1 = t[0];
		int m2 = t[1];
		for (int i = 2; i < size - 1; i += 2)
		{
			m1 = std::max(m1, t[i]);
			m2 = std::max(m2, t[i]);
		}
		max = std::max(m1, m2);
	}
	elapsedMilliseconds = endTimer(timerStart);
	printf("exec time: %lf ms\n", elapsedMilliseconds);
	printf("max: %d\n", max);


	delete[] t;
}

void branch()
{
	// prepare input data
	const int size = 2000000;
	int* t = new int[size];
	for (int i = 0; i < size; ++i)
		t[i] = rand();

	int num1 = 0, num2 = 0, num3 = 0;
	__int64 timerStart;
	double elapsedMilliseconds;



	// 3. good branch prediction
	printf("branch: good branch prediction\n");
	timerStart = startTimer();
	for (int r = 0; r < 500; ++r)
	{
		for (int i = 0; i < size; ++i)
		{
			if (t[i] < 1000000)
				++num1;
			if (t[i] > 0)
				++num2;
			if (t[i] < 2000000)
				++num3;
		}
	}
	printf("%d %d %d\n", num1, num2, num3);
	elapsedMilliseconds = endTimer(timerStart);
	printf("exec time: %lf ms\n", elapsedMilliseconds);
	

	// 4. bad branch prediction
	printf("branch: bad branch prediction\n");
	for (int r = 0; r < 500; ++r)
	{
		for (int i = 0; i < size; ++i)
		{
			if (t[i] < 324)
				++num1;
			if (t[i] > 8273)
				++num2;
			if (t[i] < 22321)
				++num3;
		}
	}
	printf("%d %d %d\n", num1, num2, num3);
	elapsedMilliseconds = endTimer(timerStart);
	printf("exec time: %lf ms\n", elapsedMilliseconds);


	delete[] t;
}

void cache()
{
	__int64 timerStart;
	double elapsedMilliseconds;

	const int size = 2000;
	int** t = new int*[size];
	for (int i = 0; i < size; ++i)
	{
		t[i] = new int[size];
		for (int j = 0; j < size; ++j)
			t[i][j] = i + j % (i + 1);
	}

	int sum = 0;


	// 5. bad cache behavior
	printf("cache: bad cache\n");
	timerStart = startTimer();
	for (int r = 0; r < 100; ++r)
	{
		for (int i = 0; i < size; ++i)
		{
			for (int j = 0; j < size; ++j)
				sum += t[j][i];
		}
	}
	printf("%d\n", sum);
	elapsedMilliseconds = endTimer(timerStart);
	printf("exec time: %lf ms\n", elapsedMilliseconds);

	// 6. good cache behavior
	printf("cache: good cache\n");
	timerStart = startTimer();
	for (int r = 0; r < 100; ++r)
	{
		for (int i = 0; i < size; ++i)
		{
			for (int j = 0; j < size; ++j)
				sum += t[i][j];
		}
	}
	printf("%d\n", sum);
	elapsedMilliseconds = endTimer(timerStart);
	printf("exec time: %lf ms\n", elapsedMilliseconds);


	for (int i = 0; i < size; ++i)
		delete[] t[i];
	delete[] t;
}

int main()
{
	//ilp();
	//branch();
	cache();
}