using Project_2semester.Commands;
using Project_2semester.Structures;
using Project_2semester.Validator;

using System;
using System.Collections.Generic;

namespace Project_2semester
{
    class Program
    {
        public static HashTable hashTable = new HashTable();

        public static void Main(string[] args)
        {
            ProgramExit programExit = new ProgramExit();

            AppDomain.CurrentDomain.ProcessExit += programExit.onProcessExit;
            Console.CancelKeyPress += new ConsoleCancelEventHandler(programExit.consoleProgramStop);

            InputValidator inputValidation = new InputValidator();
            Utils utils = new Utils(); 
            SolveCommand solve = new SolveCommand();
            AllCommand all = new AllCommand();

            List<string> allCombinations = new List<string>();

            string input = "";

            string basicValidation = "";
            string commandValidation = "";
            string functionValidation = "";
            string operationValidation = "";

            do
            {
                Tree tree = new Tree();

                Console.Write("Enter a logic function: ");
                input = Console.ReadLine();

                /*Test: 
                    DEFINE func1(a, b, c, d): "((a & b) | c) & !d"
                    DEFINE func2(a, b, c, d, e, f): "(a & (a & b) | c) & !d & !e"
                    DEFINE func3(a, b): "b & !a"
                    DEFINE func4(a, b, c, d, e, f): "func1(a, b, c, d) & func2(a, b, c, d, e, f) & func3(a, b)"
                    DEFINE func5(a, b, c, d): "func1(a, b, c, d) | a & b"
                    DEFINE func6(a, b, c, d): "b | func5(a, b, c, d) & !a | func3(a, b)"
                
                    SOLVE func1(1, 0, 1, 0)
                    SOLVE func3(0, 1)

                    ALL func1
                    ALL func3
                */

                basicValidation = inputValidation.basicValidation(input);

                if (basicValidation.Equals("null") || basicValidation.Equals("formatError") || basicValidation.Equals("error"))
                {
                    utils.resetAllValidatorVariables();
                    Console.WriteLine();
                    continue;
                }

                Console.WriteLine(basicValidation);

                commandValidation = inputValidation.commandValidation(basicValidation);

                if (commandValidation.Equals("formatError") || commandValidation.Equals("error"))
                {
                    utils.resetAllValidatorVariables();
                    Console.WriteLine();
                    continue;
                }
                else if (commandValidation.Equals("stop"))
                {
                    Console.WriteLine("\nProgram is stopping...");
                    return;
                }

                Console.WriteLine("Command: " + commandValidation);

                functionValidation = inputValidation.functionValidation(basicValidation);

                if (functionValidation.Equals("null") || functionValidation.Equals("formatError") || functionValidation.Equals("error"))
                {
                    utils.resetAllValidatorVariables();
                    Console.WriteLine();
                    continue;
                }

                foreach (string functionVariables in InputValidator.functionVariables)
                {
                    Utils.functionVariables.Add(functionVariables);
                }

                Utils.functionName = InputValidator.functionName;

                InputValidator.functionVariables = new List<string>();
                Console.WriteLine("Function: " + functionValidation);

                if (commandValidation.Equals("DEFINE"))
                {
                    try
                    {
                        if (hashTable.contains(Utils.functionName))
                        {
                            throw new FormatException("Wrong function input! (already entered function name)\n");
                        }

                        operationValidation = inputValidation.operationValidation(basicValidation);

                        if (operationValidation.Equals("null") || operationValidation.Equals("formatError") || operationValidation.Equals("error"))
                        {
                            utils.resetAllValidatorVariables();
                            Console.WriteLine();
                            continue;
                        }

                        Console.WriteLine($"Operation: {operationValidation}\n");

                        tree = new Tree();

                        string postfix = utils.convertToPostfix(operationValidation);
                        tree.buildTree(postfix);

                        hashTable.add(Utils.functionName, Utils.functionVariables.ToArray(), tree);
                        hashTable.get(Utils.functionName).printTree();

                        Utils.functionNames.Add(Utils.functionName);

                        utils.resetAllValidatorVariables();
                        Console.WriteLine();
                    }
                    catch (FormatException formatEx)
                    {
                        utils.resetAllValidatorVariables();
                        Console.WriteLine("\nAn expected exception occurred: " + formatEx.Message);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        utils.resetAllValidatorVariables();
                        Console.WriteLine("\nAn exception occurred: " + ex.Message);
                        continue;
                    }
                }
                else if (commandValidation.Equals("SOLVE"))
                {
                    try
                    {
                        if (!hashTable.contains(Utils.functionName))
                        {
                            throw new FormatException("Wrong function input! (this function was not defined)\n");
                        }

                        tree = hashTable.get(Utils.functionName);

                        string[] variables = hashTable.getArguments(Utils.functionName);

                        Console.WriteLine($"\nThe answer is: {solve.solve(variables, Utils.solveValues.ToArray(), tree)}\n");

                        utils.resetAllValidatorVariables();
                    }
                    catch (FormatException formatEx)
                    {
                        utils.resetAllValidatorVariables();
                        Console.WriteLine("\nAn expected exception occurred: " + formatEx.Message);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        utils.resetAllValidatorVariables();
                        Console.WriteLine("\nAn exception occurred: " + ex.Message);
                        continue;
                    }
                }
                else if (commandValidation.Equals("ALL"))
                {
                    try
                    {
                        if (!hashTable.contains(Utils.functionName))
                        {
                            throw new FormatException("Wrong function input! (this function was not defined)\n");
                        }

                        string[] variables = hashTable.getArguments(Utils.functionName);
                        string[] combinationArray;

                        tree = hashTable.get(Utils.functionName);

                        allCombinations = all.generateCombinations(hashTable.getArgumentsCount(Utils.functionName));

                        Console.WriteLine();

                        foreach (string combination in allCombinations)
                        {
                            combinationArray = new string[combination.Length];

                            for (int i = 0; i < combinationArray.Length; i++)
                            {
                                combinationArray[i] = combination[i].ToString();
                            }

                            Console.WriteLine($"The answer for '{combination}' combination is: {solve.solve(variables, combinationArray, tree)}");
                        }

                        Console.WriteLine();
                        utils.resetAllValidatorVariables();
                    }
                    catch (FormatException formatEx)
                    {
                        utils.resetAllValidatorVariables();
                        Console.WriteLine("\nAn expected exception occurred: " + formatEx.Message);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        utils.resetAllValidatorVariables();
                        Console.WriteLine("\nAn exception occurred: " + ex.Message);
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("Sorry we have not finished this functionality!");
                    utils.resetAllValidatorVariables();
                }

            } while (true);

        }
    }
}