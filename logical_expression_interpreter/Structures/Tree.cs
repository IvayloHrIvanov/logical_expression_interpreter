using Project_2semester.Validator;

using System;

namespace Project_2semester.Structures
{
    public class Tree
    {
        private static Utils utils = new Utils();

        private const int Count = 5;

        public class TreeNode
        {
            public char Value { get; set; }
            public TreeNode? LeftNode { get; set; }
            public TreeNode? RightNode { get; set; }

            public TreeNode(char value)
            {
                Value = value;
            }
        }

        public TreeNode? Root;

        public Tree()
        {
            Root = null;
        }

        public Tree copyTree()
        {
            Tree newTree = new Tree();
            newTree.Root = copyNode(Root);
            return newTree;
        }

        private TreeNode copyNode(TreeNode? node)
        {
            if (node != null)
            {
                TreeNode newNode = new TreeNode(node.Value);
                newNode.LeftNode = copyNode(node.LeftNode);
                newNode.RightNode = copyNode(node.RightNode);
                return newNode;
            }

            return null;
        }

        public void printTree()
        {
            render(Root, 0);
        }

        private void render(TreeNode? root, int currentSpaces)
        {
            if (root == null)
            {
                return;
            }

            currentSpaces += Count;
            render(root.RightNode, currentSpaces);

            for (int i = Count; i < currentSpaces; i++)
            {
                Console.Write(" ");
            }

            Console.Write(root.Value + "\n");
            render(root.LeftNode, currentSpaces);
        }

        public void buildTree(string postfix)
        {
            Stack stack = new Stack();

            TreeNode treeNode;
            TreeNode negatedNode;

            TreeNode node;
            TreeNode right;
            TreeNode left;

            foreach (char character in postfix)
            {
                if (utils.validateLettersAndNumbers(character))
                {
                    if (stack.Count() > 0 && stack.top().Value == '!' && stack.top().RightNode == null)
                    {
                        negatedNode = new TreeNode(stack.extract().Value);
                        negatedNode.RightNode = new TreeNode(character);
                        stack.insert(negatedNode);
                        continue;
                    }

                    treeNode = new TreeNode(character);
                    stack.insert(treeNode);
                }
                else if (character == '!')
                {
                    treeNode = new TreeNode(character);
                    stack.insert(treeNode);
                }
                else if (character != ' ')
                {
                    right = stack.extract();
                    left = stack.extract();

                    node = new TreeNode(character)
                    {
                        LeftNode = left,
                        RightNode = right
                    };

                    stack.insert(node);
                }
            }

            Root = stack.extract();
        }

        public string treeToPostfix()
        {
            return treeToPostfix(Root);
        }

        public string treeToPostfix(TreeNode? root)
        {
            string left = "";
            string right = "";
            char current;

            if (root != null)
            {
                left = treeToPostfix(root.LeftNode);
                right = treeToPostfix(root.RightNode);
                current = root.Value;

                if (root.Value == '!')
                {
                    return current + right;
                }

                return left + right + current;
            }

            return "";
        }

        public string treeToString()
        {
            return treeToString(Root);
        }

        private string treeToString(TreeNode? root)
        {
            string left = "";
            string right = "";
            char current;

            if (root != null)
            {
                left = treeToString(root.LeftNode);
                right = treeToString(root.RightNode);
                current = root.Value;

                if (root.Value == '!')
                {
                    return current + right;
                }

                return left + current + right;
            }

            return "";
        }

    }
}