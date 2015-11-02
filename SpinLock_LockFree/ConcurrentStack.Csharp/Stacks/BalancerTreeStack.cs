using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConcurrentStack.Csharp
{
    class BalancerTreeStack<T> : IStack<T>
    {
        private abstract class Node
        {
            public abstract void Push(T value);
            public abstract bool Pop(out T value);
        }

        class SwitchNode : Node
        {
            private int direction = 0;
            private Node left;
            private Node right;

            public SwitchNode(Node left, Node right)
            {
                this.left = left;
                this.right = right;
            }

            public override void Push(T value)
            {
                while (true)
                {
                    var myDirection = direction;
                    var newDirection = (myDirection + 1) % 2;
                    if (Interlocked.CompareExchange(ref direction, newDirection, myDirection) == myDirection)
                    {
                        if (myDirection == 0)
                            left.Push(value);
                        else
                            right.Push(value);
                        return;
                    }
                }
            }

            public override bool Pop(out T value)
            {
                while (true)
                {
                    var myDirection = direction;
                    var newDirection = (myDirection + 1) % 2;
                    if (Interlocked.CompareExchange(ref direction, newDirection, myDirection) == myDirection)
                    {
                        if (newDirection == 0)
                            return left.Pop(out value);
                        else
                            return right.Pop(out value);
                    }
                }
            }
        }
        class LeafNode : Node
        {
            private readonly LockFreeStack<T> stack = new LockFreeStack<T>();
            public override void Push(T value)
            {
                stack.Push(value);
            }
            public override bool Pop(out T value)
            {
                return stack.Pop(out value);
            }
        }


        private readonly SwitchNode root;

        public BalancerTreeStack()
        {
            root = new SwitchNode(
                new SwitchNode(new LeafNode(), new LeafNode()),
                new SwitchNode(new LeafNode(), new LeafNode())
                );
        }

        public void Push(T value)
        {
            root.Push(value);
        }

        public bool Pop(out T value)
        {
            return root.Pop(out value);
        }
    }
}
