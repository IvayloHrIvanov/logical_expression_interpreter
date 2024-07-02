using System;

namespace Project_2semester.Structures
{
    public class Stack
    {
        private class StackNode
        {
            public Tree.TreeNode Value;
            public StackNode? Next;
        }

        private StackNode? topNode;
        private int count;

        public Stack()
        {
            topNode = null;
            count = 0;
        }

        public void insert(Tree.TreeNode value)
        {
            StackNode newNode = new StackNode {
                Value = value, Next = topNode 
            };

            topNode = newNode;
            count++;
        }

        public Tree.TreeNode extract()
        {
            if (isEmpty())
            {
                throw new InvalidOperationException("Stack is empty!");
            }

            Tree.TreeNode value = topNode.Value;
            topNode = topNode.Next;
            count--;
            return value;
        }

        public Tree.TreeNode top()
        {
            if (isEmpty())
            {
                throw new InvalidOperationException("Stack is empty!");
            }

            return topNode.Value;
        }

        public bool isEmpty()
        {
            return topNode == null;
        }

        public int Count()
        {
            return count;
        }

    }
}