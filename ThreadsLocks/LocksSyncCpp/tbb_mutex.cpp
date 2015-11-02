#include <stdio.h>
#include "tbb\mutex.h"

class TBBSync
{
private:
	tbb::mutex myMutex;
public:
	void Foo()
	{
		{
			tbb::mutex::scoped_lock lock( myMutex );
			// critial ...
		}
		// other
		printf( "foo\n" );
	}
};

int main8()
{
	TBBSync x;
	x.Foo();

	return 0;
}