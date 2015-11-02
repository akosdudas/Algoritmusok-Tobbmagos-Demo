#include <stdio.h>
#include <Windows.h>

int main6()
{
	HANDLE mutex = CreateMutex( NULL, false, NULL );

	WaitForSingleObject( mutex, 1000 );
	// ..
	ReleaseMutex( mutex );
	
	CloseHandle( mutex );

	

	CRITICAL_SECTION cs;
	InitializeCriticalSection( &cs );
	
	EnterCriticalSection( &cs );
	// ..
	LeaveCriticalSection( &cs );

	DeleteCriticalSection( &cs );





	SRWLOCK rwlock;
	InitializeSRWLock( &rwlock );

	AcquireSRWLockShared( &rwlock );
	//AcquireSRWLockExclusive( &rwlock );
	// ...
	ReleaseSRWLockShared( &rwlock );
	//ReleaseSRWLockExclusive( &rwlock );

	return 0;
}