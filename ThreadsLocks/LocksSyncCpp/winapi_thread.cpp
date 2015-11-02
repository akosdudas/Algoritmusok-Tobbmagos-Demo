#include <stdio.h>
#include <Windows.h>
#include <process.h>

void threadFunc1( void* param )
{
	printf("begin\n");
	Sleep(1000);
	printf("end\n");
}

void main()
{
	HANDLE hThread[3];
	for( int i = 0; i < 3; ++i )
		hThread[i] = (HANDLE) _beginthread( &threadFunc1, 0, NULL );
	WaitForMultipleObjects( 3, hThread, true, INFINITE );
	for( int i = 0; i < 3; ++i )
       CloseHandle( hThread[i] );
}