using System;
using System.Collections.Generic;

namespace Project_2semester.Validator
{
    public class InputValidator
    {
        private Utils utils = new Utils();

        public static List<string> functionVariables = new List<string>();
        public static List<string> operationVariables = new List<string>();

        internal static int countOpeningParenthesis = 0;
        internal static int countClosingParenthesis = 0;
        internal static int countDoubleQuotes = 0;
        internal static int countColon = 0;

        internal static string command = "";
        public static string functionName = "";

        public string basicValidation(string input)
        {
            int startIndex = 0;

            try
            {
                if (input.Equals(null) || input.Equals(""))
                {
                    throw new ArgumentNullException("input", "The input is empty");
                }

                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i] != ' ')
                    {
                        startIndex = i;
                        break;
                    }
                }

                string reworkedInput = "";
                bool isDuplicationChecked = false;

                while (startIndex < input.Length)
                {
                    if (utils.validateAllSymbols(input[startIndex]))
                    {
                        switch (input[startIndex])
                        {
                            case '(':
                                countOpeningParenthesis++;
                                break;
                            case ')':
                                countClosingParenthesis++;
                                break;
                            case '"':
                                countDoubleQuotes++;
                                break;
                            case ':':
                                countColon++;
                                break;
                        }

                        if (input[startIndex] == ' ' && isDuplicationChecked == false)
                        {
                            isDuplicationChecked = true;
                            reworkedInput += input[startIndex];
                        }
                        else if (input[startIndex] != ' ')
                        {
                            isDuplicationChecked = false;
                            reworkedInput += input[startIndex];
                        }
                    }
                    else
                    {
                        throw new FormatException("Wrong elements in the input!");
                    }

                    startIndex++;
                }

                return reworkedInput;
            }
            catch (ArgumentNullException nullEx)
            {
                Console.WriteLine("An expected exception occurred: " + nullEx.Message);
                return "null";
            }
            catch (FormatException formatEx)
            {
                Console.WriteLine("An expected exception occurred: " + formatEx.Message);
                return "formatError";
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred: " + ex.Message);
                return "error";
            }
        }

        public string commandValidation(string input)
        {
            try
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i] == ' ')
                    {
                        break;
                    }

                    command += input[i];
                }

                switch (command[0])
                {
                    case 'D':
                        if (command.Equals("DEFINE"))
                        {
                            if (countOpeningParenthesis != countClosingParenthesis || countOpeningParenthesis < 1 || countClosingParenthesis < 1 || countDoubleQuotes % 2 != 0 || countDoubleQuotes != 2 || countColon != 1)
                            {
                                throw new FormatException("Missing or too much elements in the input!");
                            }

                            utils.resetCounters();
                            return command;
                        }
                        else
                        {
                            throw new FormatException("Wrong command input!");
                        }

                    case 'S':
                        if (command.Equals("SOLVE"))
                        {
                            if (countOpeningParenthesis != 1 || countClosingParenthesis != 1 || countDoubleQuotes != 0 || countColon != 0)
                            {
                                throw new FormatException("Missing, too much or invalid elements in the input!");
                            }

                            utils.resetCounters();
                            return command;
                        }
                        else
                        {
                            throw new FormatException("Wrong command input!");
                        }

                    case 'F':
                        if (command.Equals("FIND"))
                        {
                            utils.resetCounters();
                            return command;
                        }
                        else
                        {
                            throw new FormatException("Wrong command input!");
                        }

                    case 'A':
                        if (command.Equals("ALL"))
                        {
                            if (countOpeningParenthesis != 0 || countClosingParenthesis != 0 || countDoubleQuotes != 0 || countColon != 0)
                            {
                                throw new FormatException("Missing, too much or invalid elements in the input!");
                            }

                            utils.resetCounters();
                            return command;
                        }
                        else
                        {
                            throw new FormatException("Wrong command input");
                        }

                    case 'Q':
                        if (command.Equals("QUIT"))
                        {
                            return "stop";
                        }
                        else
                        {
                            throw new FormatException("Wrong command input");
                        }

                    default:
                        throw new FormatException("Wrong command input!");
                }


            }
            catch (FormatException formatEx)
            {
                Console.WriteLine("An expected exception occurred: " + formatEx.Message);
                return "formatError";
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred: " + ex.Message);
                return "error";
            }
        }

        public string functionValidation(string input)
        {
            string function = "";
            int functionStartIndex = 0;
            bool checkWhitespace = false;

            int startOfVariables = 0;

            bool checkIfNumber = false;
            int saveNumber = 0;

            functionName = "";

            try
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (checkWhitespace == true && input[i] != ' ')
                    {
                        functionStartIndex = i;
                        break;
                    }

                    if (input[i] == ' ')
                    {
                        checkWhitespace = true;
                    }
                }

                switch (command)
                {
                    case "DEFINE":
                        while (functionStartIndex < input.Length && input[functionStartIndex] != ':')
                        {
                            function += input[functionStartIndex];
                            functionStartIndex++;
                        }

                        if (input[functionStartIndex - 1] != ')')
                        {
                            throw new FormatException("Wrong function input! (wrong ':' or ')' placement)");
                        }

                        utils.resetCounters();

                        foreach (char symbol in function)
                        {
                            switch (symbol)
                            {
                                case '(':
                                    countOpeningParenthesis++;
                                    break;
                                case ')':
                                    countClosingParenthesis++;
                                    break;
                            }
                        }

                        if (countOpeningParenthesis != countClosingParenthesis || countOpeningParenthesis > 1 || countClosingParenthesis > 1)
                        {
                            throw new FormatException("Wrong function input! (wrong '(' and ')' )");
                        }

                        for (int i = 0; i < function.Length; i++)
                        {
                            if (function[i] == '(')
                            {
                                startOfVariables = i + 1;
                                break;
                            }

                            if (!utils.validateLettersAndNumbers(function[i]))
                            {
                                throw new FormatException("Wrong function input! (wrong name symbol)");
                            }

                            functionName += function[i];
                        }

                        if (functionName == null || functionName.Length == 0 || functionName.Equals(""))
                        {
                            throw new ArgumentNullException("no function name", "Wrong function input!");
                        }

                        if (!utils.validateFunctionVariables(startOfVariables, function))
                        {
                            return "formatError";
                        }

                        function = utils.function;

                        foreach (string functionVariable in functionVariables)
                        {
                            checkIfNumber = int.TryParse(functionVariable, out saveNumber);

                            if (checkIfNumber)
                            {
                                throw new FormatException("Wrong function input! (variables can't be numbers)");
                            }
                        }

                        break;

                    case "SOLVE":
                        while (functionStartIndex < input.Length && input[functionStartIndex - 1] != ')')
                        {
                            function += input[functionStartIndex];
                            functionStartIndex++;
                        }

                        if (functionStartIndex != input.Length)
                        {
                            Console.WriteLine("WARNING: Invalid elements after the function input. They will be ignored!");
                        }

                        if (!utils.validateLettersAndNumbers(input[functionStartIndex - 2]))
                        {
                            throw new FormatException("Wrong function input!");
                        }

                        for (int i = 0; i < function.Length; i++)
                        {
                            if (function[i] == '(')
                            {
                                startOfVariables = i + 1;
                                break;
                            }

                            if (!utils.validateLettersAndNumbers(function[i]))
                            {
                                throw new FormatException("Wrong function input! (wrong name symbol)");
                            }

                            functionName += function[i];
                        }

                        if (functionName.Equals(null) || functionName.Equals(""))
                        {
                            throw new ArgumentNullException("no function name", "Wrong function input!");
                        }

                        if (!utils.validateFunctionVariables(startOfVariables, function))
                        {
                            return "formatError";
                        }

                        if (functionVariables.Count != Program.hashTable.getArgumentsCount(functionName))
                        {
                            throw new FormatException("Wrong function input! (invalid number of variables)");
                        }

                        foreach (string functionVariable in functionVariables)
                        {
                            checkIfNumber = int.TryParse(functionVariable, out saveNumber);

                            if (!checkIfNumber)
                            {
                                throw new FormatException("Wrong function input! (variables can only be numbers)");
                            }

                            if (saveNumber != 0 && saveNumber != 1 || functionVariable.Length != 1)
                            {
                                throw new FormatException("Wrong function input! (variables can only be 0 and 1)");
                            }

                            Utils.solveValues.Add(saveNumber.ToString());
                        }

                        break;

                    case "ALL":
                        while (functionStartIndex < input.Length)
                        {
                            function += input[functionStartIndex];
                            functionStartIndex++;
                        }

                        for (int i = 0; i < function.Length; i++)
                        {
                            if (!utils.validateLettersAndNumbers(function[i]))
                            {
                                throw new FormatException("Wrong function input! (wrong name symbol)");
                            }

                            functionName += function[i];
                        }

                        if (functionName.Equals(null) || functionName.Equals(""))
                        {
                            throw new ArgumentNullException("no function name", "Wrong function input!");
                        }

                        break;
                }

                utils.resetCounters();
                return function;
            }
            catch (ArgumentNullException nullEx)
            {
                Console.WriteLine("An expected exception occurred: " + nullEx.Message);
                return "null";
            }
            catch (FormatException formatEx)
            {
                Console.WriteLine("An expected exception occurred: " + formatEx.Message);
                return "formatError";
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred: " + ex.Message);
                return "error";
            }
        }

        public string operationValidation(string input)
        {
            string operation = "";

            int operationStartIndex = 0;
            int checkColon = 0;

            bool checkNoSymbols = false;

            try
            {
                while (checkColon < input.Length && input[checkColon] != ':')
                {
                    checkColon++;
                }

                if (input[checkColon + 1] != '"' && input[checkColon + 2] != '"')
                {
                    throw new FormatException("Wrong operation input! (wrong '\"' placement)");
                }
                else if (input[checkColon + 1] != '"' && input[checkColon + 2] == '"')
                {
                    if (input[checkColon + 1] != 32)
                    {
                        throw new FormatException("Wrong operation input! (invalid element before '\"')");
                    }
                }

                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i] == '"')
                    {
                        operationStartIndex = i + 1;
                        break;
                    }
                }

                while (operationStartIndex < input.Length && input[operationStartIndex] != '"')
                {
                    if (input[operationStartIndex] != ' ')
                    {
                        operation += input[operationStartIndex];
                    }

                    operationStartIndex++;
                }

                for (int i = 0; i < operation.Length; i++)
                {
                    if (!utils.validateAllOperationSymbols(operation[i]) && !utils.validateLettersAndNumbers(operation[i]))
                    {
                        throw new FormatException("Wrong operation input! (unallowed symbol)");
                    }

                    if (utils.validateOperationSymbols(operation[i]) && utils.validateOperationSymbols(operation[i + 1]))
                    {
                        throw new FormatException("Wrong operation input! (duplication of symbol)");
                    }

                    if (utils.validateOperationSymbols(operation[i]))
                    {
                        checkNoSymbols = true;
                    }
                }

                if (!checkNoSymbols)
                {
                    throw new FormatException("Wrong operation input! (no operation symbols)");
                }

                for (int i = 0; i < operation.Length; i++)
                {
                    if (operation[i] == '!')
                    {
                        if (i == operation.Length - 1)
                        {
                            throw new FormatException("Wrong operation input! (invalid placement of '!')");
                        }
                        else if (!utils.validateLettersAndNumbers(operation[i + 1]))
                        {
                            throw new FormatException("Wrong operation input! (invalid placement of '!')");
                        }
                    }

                    if (operation[i] == '(')
                    {
                        if (i == operation.Length - 1)
                        {
                            throw new FormatException("Wrong operation input! (invalid placement of '(' )"); //invalid placement of operation '('
                        }
                        else if (utils.validateOperationSymbols(operation[i + 1]) || operation[i + 1] == ')')
                        {
                            throw new FormatException("Wrong operation input! (invalid placement of '(' )"); //invalid placement of operation '('
                        }
                    }

                    if (operation[i] == ')')
                    {
                        if (i == 0)
                        {
                            throw new FormatException("Wrong operation input! (invalid placement of ')' )"); //invalid placement of operation ')'
                        }
                        else if (utils.validateOperationSymbols(operation[i - 1]) && i != operation.Length - 1)
                        {
                            throw new FormatException("Wrong operation input! (invalid placement of ')' )"); //invalid placement of operation ')'
                        }
                    }

                    if (utils.validateOperationSymbols(operation[i]))
                    {
                        if (i == operation.Length - 1 || i == 0)
                        {
                            throw new FormatException("Wrong operation input! (invalid placement of operation symbol)"); //invalid placement of operation symbol
                        }

                        if (!utils.validateLettersAndNumbers(operation[i - 1]))
                        {
                            if (i == 1)
                            {
                                throw new FormatException("Wrong operation input! (invalid placement of operation symbol)"); //invalid placement of operation symbol
                            }

                            if (operation[i - 1] == ')')
                            {
                                if (!utils.validateLettersAndNumbers(operation[i - 2]))
                                {
                                    throw new FormatException("Wrong operation input! (invalid placement of operation symbol)"); //invalid placement of operation symbol
                                }
                            }
                            else
                            {
                                throw new FormatException("Wrong operation input! (invalid placement of operation symbol)"); //invalid placement of operation symbol
                            }
                        }

                        if (!utils.validateLettersAndNumbers(operation[i + 1]))
                        {
                            if (i == operation.Length - 2)
                            {
                                throw new FormatException("Wrong operation input! (invalid placement of operation symbol)"); //invalid placement of operation symbol
                            }

                            switch (operation[i + 1])
                            {
                                case '!':
                                    if (!utils.validateLettersAndNumbers(operation[i + 2]))
                                    {
                                        throw new FormatException("Wrong operation input! (invalid placement of operation symbol)"); //invalid placement of operation symbol
                                    }
                                    break;
                                case '(':
                                    if (!utils.validateLettersAndNumbers(operation[i + 2]) && operation[i + 2] != '!')
                                    {
                                        throw new FormatException("Wrong operation input! (invalid placement of operation symbol)"); //invalid placement of operation symbol
                                    }
                                    break;
                                default:
                                    throw new FormatException("Wrong operation input! (invalid placement of operation symbol)"); //invalid placement of operation symbol
                            }
                        }
                    }
                }

                string variable = "";
                string functionHashVariable = "";
                string functionHashArguments = "";
                string functionHashArgumentsVariable = "";
                string functionValidationError = "";

                int variableStart = 0;
                int variableEnd = 0;
                int hashVariableEnd = 0;
                int functionHashVariableEnd = 0;

                bool foundClosingParenthesis = false;
                bool checkVariables = false;

                for (int i = 0; i < operation.Length; i++)
                {
                    if (utils.validateLettersAndNumbers(operation[i]))
                    {
                        variableStart = i;

                        while (i < operation.Length)
                        {
                            if (utils.validateLettersAndNumbers(operation[i]))
                            {
                                variable += operation[i];
                            }
                            else
                            {
                                variableEnd = i - 1;
                                break;
                            }

                            i++;
                        }

                        if (i == operation.Length)
                        {
                            variableEnd = i;
                        }

                        if (variableStart > 0 && !utils.validateOperationSymbols(operation[variableStart - 1]) || variableEnd < operation.Length && !utils.validateOperationSymbols(operation[variableEnd + 1]))
                        {
                            if (variableStart > 0 && operation[variableStart - 1] == '!')
                            {
                                if (variableStart > 1 && !utils.validateOperationSymbols(operation[variableStart - 2]) && operation[variableStart - 2] != '(')
                                {
                                    throw new FormatException("Wrong operation input! (missing operator before or after variable)"); //missing operand before or after variable
                                }
                            }
                        }

                        if (variableEnd < operation.Length && operation[variableEnd + 1] == '!')
                        {
                            throw new FormatException("Wrong operation input! (invalid placement of symbol '!')"); //wrong placement of symbol '!'
                        }

                        if (Program.hashTable.contains(variable))
                        {
                            Console.WriteLine("\nAlready writen function is detected in the operation...");
                            hashVariableEnd = variableEnd;
                            functionHashVariable = variable;

                            if (hashVariableEnd != operation.Length - 1)
                            {
                                if (operation[hashVariableEnd + 1] != '(')
                                {
                                    throw new FormatException("Wrong operation input! (invalid operation function input)"); //invalid function input
                                }
                            }
                            else
                            {
                                throw new FormatException("Wrong operation input! (invalid operation function input)");
                            }

                            functionHashVariable += operation[i];
                            i++;

                            while (i < operation.Length)
                            {
                                if (operation[i] == ')')
                                {
                                    functionHashVariable += operation[i];
                                    functionHashVariableEnd = i + 1;
                                    foundClosingParenthesis = true;
                                    i++;
                                    break;
                                }

                                functionHashVariable += operation[i];
                                i++;
                            }

                            if (!foundClosingParenthesis)
                            {
                                throw new FormatException("Wrong operation input! (invalid operation function input)");
                            }

                            functionValidationError = functionValidation(" " + functionHashVariable);

                            Console.WriteLine(functionName);

                            if (functionValidationError.Equals("null") || functionValidationError.Equals("formatError") || functionValidationError.Equals("error"))
                            {
                                return "error";
                            }

                            functionHashArguments = string.Join(",", Program.hashTable.getArguments(functionName));

                            Console.WriteLine("Function arguments: " + functionHashArguments);
                            Console.WriteLine("Function arguments count: " + Program.hashTable.getArgumentsCount(functionName));

                            if (functionVariables.Count != Program.hashTable.getArgumentsCount(functionName))
                            {
                                throw new FormatException("Wrong function input! (invalid operation function arguments input)");
                            }

                            int j = 0;

                            foreach (string functionVariable in functionVariables)
                            {
                                while (j != functionHashArguments.Length && functionHashArguments[j] != ',')
                                {
                                    functionHashArgumentsVariable += functionHashArguments[j];
                                    j++;
                                }

                                if (functionHashArgumentsVariable != functionVariable)
                                {
                                    throw new FormatException("Wrong function input! (invalid operation function arguments input)");
                                }

                                functionHashArgumentsVariable = "";
                                j++;
                            }

                            foreach (string functionVariable in functionVariables)
                            {
                                foreach (string operationVariable in operationVariables)
                                {
                                    if (functionVariable == operationVariable)
                                    {
                                        checkVariables = true;
                                        break;
                                    }
                                }

                                if (!checkVariables)
                                {
                                    operationVariables.Add(functionVariable);
                                }

                                checkVariables = false;
                            }

                            operation = operation.Remove(variableStart, functionHashVariableEnd - variableStart).Insert(variableStart, $"({Program.hashTable.get(functionName).treeToString()})");
                            i = Program.hashTable.get(functionName).treeToString().Length + 2;

                            Console.WriteLine($"Changed operation: {operation}\n");

                            variable = "";
                            functionVariables = new List<string>();
                            continue;
                        }

                        checkVariables = false;

                        foreach (string operationVariable in operationVariables)
                        {
                            if (operationVariable == variable)
                            {
                                checkVariables = true;
                                break;
                            }
                        }

                        if (!checkVariables)
                        {
                            operationVariables.Add(variable);
                        }

                        checkVariables = false;
                        variable = "";
                    }
                }

                for (int i = 0; i < operation.Length; i++)
                {
                    if (operation[i] == ',')
                    {
                        throw new FormatException("Wrong operation input! (previously unentered operation function is detected)");
                    }
                }

                bool checkIfNumber = false;
                int saveNumber = 0;

                foreach (string operationVariables in operationVariables)
                {
                    checkIfNumber = int.TryParse(operationVariables, out saveNumber);

                    if (checkIfNumber)
                    {
                        throw new FormatException("Wrong operation input! (variables can't be numbers)"); //variables can't be numbers
                    }
                }

                if (operationVariables.Count > Utils.functionVariables.Count)
                {
                    throw new FormatException("Wrong input! (missing variables in the function or operation)"); //missing variables in the function or operation
                }

                checkVariables = false;

                foreach (string operationVariable in operationVariables)
                {
                    foreach (string functionVariable in Utils.functionVariables)
                    {
                        if (operationVariable == functionVariable)
                        {
                            checkVariables = true;
                            break;
                        }
                    }

                    if (!checkVariables)
                    {
                        throw new FormatException("Wrong operation input! (missing variable)"); //missing variable
                    }

                    checkVariables = false;
                }

                return operation;
            }
            catch (ArgumentNullException nullEx)
            {
                Console.WriteLine("An expected exception occurred: " + nullEx.Message);
                return "null";
            }
            catch (FormatException formatEx)
            {
                Console.WriteLine("An expected exception occurred: " + formatEx.Message);
                return "formatError";
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred: " + ex.Message);
                return "error";
            }
        }

    }
}