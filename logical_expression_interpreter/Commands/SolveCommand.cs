using Project_2semester.Structures;
using static Project_2semester.Structures.Tree;
using Project_2semester.Validator;

using System;

namespace Project_2semester.Commands
{
    public class SolveCommand
    {
        private static Utils utils = new Utils();

        public bool solve(string[] variables, string[] values, Tree tree)
        {
            Tree copy = tree.copyTree();

            bool isSolved = false;

            if (tree.Root != null)
            {
                replaceValues(copy.Root!, variables, values);
            }

            isSolved = solveTree(copy.Root!, variables, values, tree);
            return isSolved;
        }

        private bool solveTree(TreeNode node, string[] variables, string[] values, Tree tree)
        {
            Stack stack = new Stack();

            string operationToPostfix = tree.treeToPostfix(node);

            char firstOperand;
            char secondOperand;
            bool result = false;

            char reverseChar;
            bool reverseNext = false;
            bool value = false;

            try
            {
                for (int i = 0; i < operationToPostfix.Length; i++)
                {
                    if (stack.isEmpty())
                    {
                        stack.insert(new TreeNode(operationToPostfix[i]));
                        continue;
                    }
                    else if (reverseNext)
                    {
                        reverseNext = false;
                        firstOperand = stack.extract().Value;
                        stack.extract();

                        if (firstOperand == '0')
                        {
                            reverseChar = '1';
                        }
                        else if (firstOperand == '1')
                        {
                            reverseChar = '0';
                        }
                        else
                        {
                            throw new Exception("Failed to parse char to bool");
                        }

                        stack.insert(new TreeNode(reverseChar));
                    }
                    else if (stack.top().Value == '&')
                    {
                        stack.extract();
                        firstOperand = stack.extract().Value;
                        secondOperand = stack.extract().Value;

                        value = parseCharToBool(firstOperand) && parseCharToBool(secondOperand);
                        stack.insert(new TreeNode(parseBoolToChar(value)));
                    }
                    else if (stack.top().Value == '|')
                    {
                        stack.extract();
                        firstOperand = stack.extract().Value;
                        secondOperand = stack.extract().Value;

                        value = parseCharToBool(firstOperand) || parseCharToBool(secondOperand);
                        stack.insert(new TreeNode(parseBoolToChar(value)));
                    }
                    else if (stack.top().Value == '!')
                    {
                        reverseNext = true;
                    }

                    stack.insert(new TreeNode(operationToPostfix[i]));
                }

                if (stack.Count() == 3)
                {
                    if (stack.top().Value == '&')
                    {
                        stack.extract();
                        firstOperand = stack.extract().Value;
                        secondOperand = stack.extract().Value;

                        result = parseCharToBool(firstOperand) && parseCharToBool(secondOperand);
                    }
                    else if (stack.top().Value == '|')
                    {
                        stack.extract();
                        firstOperand = stack.extract().Value;
                        secondOperand = stack.extract().Value;

                        result = parseCharToBool(firstOperand) || parseCharToBool(secondOperand);
                    }
                }
                else if (stack.Count() == 1)
                {
                    result = parseCharToBool(stack.extract().Value);
                }
                else
                {
                    throw new Exception("Failed to solve tree");
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nAn exception occurred: " + ex.Message);
                return false;
            }
        }

        private void replaceValues(TreeNode treeNode, string[] variables, string[] values)
        {
            if (treeNode != null)
            {
                for (int i = 0; i < variables.Length; i++)
                {
                    if (treeNode.Value == variables[i][0])
                    {
                        treeNode.Value = values[i][0];
                    }
                }

                replaceValues(treeNode.LeftNode, variables, values);
                replaceValues(treeNode.RightNode, variables, values);
            }

            return;
        }

        private bool parseCharToBool(char input)
        {
            switch (input)
            {
                case '1':
                    return true;
                case '0':
                    return false;
                default:
                    throw new Exception("Invalid character");
            }
        }

        private char parseBoolToChar(bool input)
        {
            if (input)
            {
                return '1';
            }

            return '0';
        }

    }
}