using System;

namespace LocksSync
{
    class PettersonLock
    {
        bool[] flag = new bool[2] { false, false };
        int turn;

        void thread0()
        {
            flag[0] = true;
            turn = 1;
            while( flag[1] && turn == 1 ) { /*busy-wait*/ }
            // critical section
            flag[0] = false;
        }

        void thread1()
        {
            flag[1] = true;
            turn = 0;
            while( flag[0] && turn == 0 ) { /*busy-wait*/ }
            // critical section
            flag[1] = false;
        }
    }
}
