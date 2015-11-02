#include <stdio.h>
#include "tbb\tbb_thread.h"

void threadFunc()
{
	printf( "start\n" );
	Sleep( 1000 );
	printf( "end\n" );
}

int main7()
{
	tbb::tbb_thread myThread( threadFunc );
	myThread.join();
	return 0;
}