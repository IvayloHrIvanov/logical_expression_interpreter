using Project_2semester.Validator;
using System;
using System.IO;

namespace Project_2semester
{
    public class ProgramExit
    {
        public void consoleProgramStop(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("\nProgram is stopping...");

            string fileName = "Logical_expression_interpreter.txt";
            string folderPath = "C:\\Users\\halo3\\Desktop\\";

            if (File.Exists(Path.Combine(folderPath, fileName)))
            {
                int counter = 1;
                string baseFileName = Path.GetFileNameWithoutExtension(fileName);
                string fileExtension = Path.GetExtension(fileName);

                while (File.Exists(Path.Combine(folderPath, fileName)))
                {
                    fileName = $"{baseFileName}{counter}{fileExtension}";
                    counter++;
                }
            }

            string filePath = Path.Combine(folderPath, fileName);
            File.Create(filePath).Close();

            string functionArguments = "";

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (string functionName in Utils.functionNames)
                {
                    functionArguments = string.Join(",", Program.hashTable.getArguments(functionName));

                    writer.WriteLine($"Function name: {functionName}");
                    writer.WriteLine($"Hashtable tree: {Program.hashTable.get(functionName).treeToString()}");
                    writer.WriteLine($"Function arguments: {functionArguments}\n");
                }
            }

            Console.WriteLine($"The data is saved in a file with name: {fileName}. Bye!");
        }

        public void onProcessExit(object sender, EventArgs e)
        {
            string fileName = "Logical_expression_interpreter.txt";
            string folderPath = "C:\\Users\\halo3\\Desktop\\";

            if (File.Exists(Path.Combine(folderPath, fileName)))
            {
                int counter = 1;
                string baseFileName = Path.GetFileNameWithoutExtension(fileName);
                string fileExtension = Path.GetExtension(fileName);

                while (File.Exists(Path.Combine(folderPath, fileName)))
                {
                    fileName = $"{baseFileName}{counter}{fileExtension}";
                    counter++;
                }
            }

            string filePath = Path.Combine(folderPath, fileName);
            File.Create(filePath).Close();

            string functionArguments = "";

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (string functionName in Utils.functionNames)
                {
                    functionArguments = string.Join(",", Program.hashTable.getArguments(functionName));

                    writer.WriteLine($"Function name: {functionName}");
                    writer.WriteLine($"Hashtable tree: {Program.hashTable.get(functionName).treeToString()}");
                    writer.WriteLine($"Function arguments: {functionArguments}\n");
                }
            }

            Console.WriteLine($"The data is saved in a file with name: {fileName}. Bye!");
        }

    }
}