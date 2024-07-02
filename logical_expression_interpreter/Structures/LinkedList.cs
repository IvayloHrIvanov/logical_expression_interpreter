namespace Project_2semester.Structures
{
    public class LinkedList
    {
        private LinkedListNode? head;

        public LinkedList() { head = null; }

        public void addLast(string functionName, string[]? arguments, Tree value)
        {
            LinkedListNode newNode = new LinkedListNode(functionName, arguments, value);

            if (head == null)
            {
                head = newNode;
            }
            else
            {
                LinkedListNode current = head;

                while (current.Next != null)
                {
                    current = current.Next;
                }

                current.Next = newNode;
            }
        }

        public bool contains(string functionName)
        {
            LinkedListNode? current = head;

            while (current != null)
            {
                if (current.Key == functionName)
                {
                    return true;
                }

                current = current.Next;
            }

            return false;
        }

        public Tree? get(string functionName)
        {
            LinkedListNode? current = head;

            while (current != null)
            {
                if (current.Key == functionName)
                {
                    return current.Value;
                }

                current = current.Next;
            }

            return null;
        }

        public string[]? getArguments(string functionName)
        {
            LinkedListNode? current = head;

            while (current != null)
            {
                if (current.Key == functionName)
                {
                    if (current.Parameters != null)
                        return current.Parameters;
                }

                current = current.Next;
            }

            return null;
        }

        public int getArgumentsCount(string functionName)
        {
            LinkedListNode? current = head;

            while (current != null)
            {
                if (current.Key == functionName)
                {
                    if (current.Parameters != null)
                        return current.Parameters.Length;
                }

                current = current.Next;
            }

            return 0;
        }

        public class LinkedListNode
        {
            public string? Key;
            public string[]? Parameters;
            public Tree? Value;
            public LinkedListNode? Next;

            public LinkedListNode(string? functionName, string[]? parameters, Tree? value)
            {
                Key = functionName;
                Parameters = parameters;
                Value = value;
                Next = null;
            }
        }

    }
}