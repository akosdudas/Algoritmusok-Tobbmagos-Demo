using System;

namespace LocksSync
{
    class LamportLock
    {
        int n;
        bool[] flag;
        int[] label;

        void locka(int threadId)
        {
            flag[threadId] = true;
            label[threadId] = max(label) + 1;
            for (int i = 0; i < n; i++) {
                while (label[i] != 0 && flag[i]  && label[i] < label[threadId] && i < threadId)
                { /*busy-wait*/ }
            }
        }

        void unlock(int threadId)
        {
            flag[threadId] = false;
        }


        private int max(int[] label) { throw new NotImplementedException(); }
    }
}
