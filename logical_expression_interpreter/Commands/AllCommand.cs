using System.Collections.Generic;

namespace Project_2semester.Commands
{
    public class AllCommand
    {
        public List<string> generateCombinations(int arguments)
        {
            List<string> combinations = new List<string>();
            recursiveCombinationGenerator("", arguments, combinations);
            return combinations;
        }

        private void recursiveCombinationGenerator(string current, int arguments, List<string> combinations)
        {
            if (current.Length == arguments)
            {
                combinations.Add(current);
            }
            else
            {
                recursiveCombinationGenerator(current + "0", arguments, combinations);
                recursiveCombinationGenerator(current + "1", arguments, combinations);
            }
        }

    }
}