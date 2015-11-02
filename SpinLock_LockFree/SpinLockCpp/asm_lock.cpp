#include <stdio.h>

int main()
{
	char mutex = 0xff;
	char* ptrMutex = &mutex;
	bool lockFail;
	do
	{
		__asm
		{
			mov lockFail, 1
			mov eax, 0 // bit index
			mov ebx, ptrMutex // address of mutex
			lock btr [ebx], eax // bit test and reset
			jnc end // no carry -> bit was reset -> no go
			mov lockFail, 0 // success
		end: // empty label
		}
	} while( lockFail );

	// critical section

	__asm
	{
		mov eax, 0 // bit index
		mov ebx, ptrMutex // address of mutex
		lock bts [ebx], eax // set bit
	}

	return 0;
}