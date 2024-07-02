namespace Project_2semester.Structures
{
    public class HashTable
    {
        private int size;
        private Bucket[] buckets;

        private class Bucket
        {
            public LinkedList Value;

            public Bucket()
            {
                Value = new LinkedList();
            }
        }

        public HashTable(int size = 29)
        {
            this.size = size < 29 ? 29 : size;
            buckets = new Bucket[this.size];
        }

        private int hash(string key)
        {
            int hash = 0;

            foreach (int character in key)
            {
                hash += ((hash * 32) + character) % size;
            }

            return hash % size;
        }

        private Bucket getEntry(string key)
        {
            int index = hash(key);
            return buckets[index] ?? (buckets[index] = new Bucket());
        }

        public Tree? get(string key)
        {
            LinkedList list = getEntry(key).Value;
            return list.get(key);
        }

        public bool contains(string key)
        {
            LinkedList list = getEntry(key).Value;
            return list.contains(key);
        }

        public string[]? getArguments(string key)
        {
            LinkedList list = getEntry(key).Value;
            return list.getArguments(key);
        }

        public int getArgumentsCount(string key)
        {
            LinkedList list = getEntry(key).Value;
            return list.getArgumentsCount(key);
        }

        public void add(string key, string[]? arguments, Tree value)
        {
            LinkedList list = getEntry(key).Value;
            list.addLast(key, arguments, value);
        }

    }
}