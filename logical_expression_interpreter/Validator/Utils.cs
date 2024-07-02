using Project_2semester.Structures;

using System;
using System.Collections.Generic;

namespace Project_2semester.Validator
{
    public class Utils
    {
        private static Stack stack = new Stack();

        public static List<string> functionVariables = new List<string>();
        public static List<string> solveValues = new List<string>();
        public static List<string> functionNames = new List<string>();

        public static string functionName = "";

        public void resetCounters()
        {
            InputValidator.countOpeningParenthesis = 0;
            InputValidator.countClosingParenthesis = 0;
            InputValidator.countDoubleQuotes = 0;
            InputValidator.countColon = 0;
        }

        public void resetAllValidatorVariables()
        {
            InputValidator.functionVariables = new List<string>();
            InputValidator.operationVariables = new List<string>();

            functionVariables = new List<string>();
            solveValues = new List<string>();

            InputValidator.countOpeningParenthesis = 0;
            InputValidator.countClosingParenthesis = 0;
            InputValidator.countDoubleQuotes = 0;
            InputValidator.countColon = 0;

            InputValidator.command = "";
        }

        public bool validateAllSymbols(char symbol)
        {
            if (symbol >= 65 && symbol <= 90 || symbol >= 97 && symbol <= 122 || symbol >= 48 && symbol <= 57 || symbol >= 32 && symbol <= 34
               || symbol == 38 || symbol == 40 || symbol == 41 || symbol == 44 || symbol == 58 || symbol == 124) //symbols: 'a-z', 'A-Z', '0-9', '[space]', '!', '"', '&', '(', ')', ',', ':', '|' in order
            {
                return true;
            }

            return false;
        }

        public bool validateVariablesSymbols(char symbol)
        {
            if (symbol == 33 || symbol == 34 || symbol == 38 || symbol == 58 || symbol == 124) //symbols: '!', '"', '&', ':', '|' //in order
            {
                return true;
            }

            return false;
        }

        public bool validateLettersAndNumbers(char symbol)
        {
            if (symbol >= 65 && symbol <= 90 || symbol >= 97 && symbol <= 122 || symbol >= 48 && symbol <= 57) //symbols: letters and numbers
            {
                return true;
            }

            return false;
        }

        public bool validateAllOperationSymbols(char symbol)
        {
            if (symbol == 33 || symbol == 38 || symbol == 40 || symbol == 41 || symbol == 44 || symbol == 124) //symbols: '!', '&', '(', ')', '|' //in order
            {
                return true;
            }

            return false;
        }

        public bool validateOperationVariablesSymbols(char symbol)
        {
            if (symbol == 33 || symbol == 38 || symbol == 124) //symbols: '!', '&', '|' //in order
            {
                return true;
            }

            return false;
        }

        public bool validateOperationSymbols(char symbol)
        {
            if (symbol == 38 || symbol == 124) //symbols: '&', '|' //in order
            {
                return true;
            }

            return false;
        }

        internal string function = "";

        public bool validateFunctionVariables(int startOfVariables, string function)
        {
            string variable = "";
            bool checkForChar = false;

            try
            {
                while (startOfVariables < function.Length)
                {
                    if (function[startOfVariables] == ')')
                    {
                        if (InputValidator.functionVariables == null || InputValidator.functionVariables.Count == 0)
                        {
                            throw new ArgumentNullException("no variables", "Wrong function input!");
                        }

                        break;
                    }

                    if (validateVariablesSymbols(function[startOfVariables]))
                    {
                        throw new FormatException("Wrong function input! (wrong variables symbol)");
                    }

                    if (function[startOfVariables] == ' ' && checkForChar == true)
                    {
                        function = function.Remove(startOfVariables, 1).Insert(startOfVariables, "");
                    }

                    if (function[startOfVariables] == ',' || function[startOfVariables] == ')')
                    {
                        if (variable == null || variable == "")
                        {
                            throw new FormatException("Wrong function input! (missing variable)");
                        }

                        InputValidator.functionVariables.Add(variable);
                        variable = "";
                        checkForChar = false;
                        startOfVariables++;
                        continue;
                    }

                    if (validateLettersAndNumbers(function[startOfVariables]))
                    {
                        checkForChar = true;
                        variable += function[startOfVariables];
                    }

                    if (function[startOfVariables] == ',' || function[startOfVariables + 1] == ')')
                    {
                        if (variable == null || variable == "")
                        {
                            throw new FormatException("Wrong function input! (missing variable)");
                        }

                        InputValidator.functionVariables.Add(variable);
                        variable = "";
                        checkForChar = false;
                    }

                    startOfVariables++;
                }

                this.function = function;
                return true;
            }
            catch (ArgumentNullException nullEx)
            {
                Console.WriteLine("An expected exception occurred: " + nullEx.Message);
                return false;
            }
            catch (FormatException formatEx)
            {
                Console.WriteLine("An expected exception occurred: " + formatEx.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred: " + ex.Message + ex.StackTrace);
                return false;
            }
        }

        public string convertToPostfix(string expression)
        {
            string postfix = "";
            char character;

            for (int i = 0; i < expression.Length; i++)
            {
                character = expression[i];

                if (validateLettersAndNumbers(character) || character == '!')
                {
                    postfix += character;
                }
                else if (character == '&' || character == '|')
                {
                    while (stack.Count() > 0 && getPriority(stack.top().Value) >= getPriority(character))
                    {
                        postfix += stack.extract().Value;
                    }

                    stack.insert(new Tree.TreeNode(character));
                }
                else if (character == '(')
                {
                    stack.insert(new Tree.TreeNode(character));
                }
                else if (character == ')')
                {
                    while (stack.Count() > 0 && stack.top().Value != '(')
                    {
                        postfix += stack.extract().Value;
                    }
                    stack.extract();
                }
            }

            while (stack.Count() > 0)
            {
                postfix += stack.extract().Value;
            }

            return postfix;
        }

        private int getPriority(char character)
        {
            switch (character)
            {
                case '&':
                    return 2;

                case '|':
                    return 1;

                default:
                    return 0;
            }
        }

    }
}