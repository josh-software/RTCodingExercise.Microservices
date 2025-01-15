namespace Catalog.API.Helpers
{
    public class LettersToNumbers
    {
        // Sourced from https://www.regtransfers.co.uk/personalised-number-plates
        private static Dictionary<string, List<string>> letterLookup = new()
            {
                { "O", new List<string> { "0", "6", "8", "9" } },
                { "D", new List<string> { "0" } },
                { "I", new List<string> { "1" } },
                { "L", new List<string> { "1" } },
                { "R", new List<string> { "2", "12" } },
                { "Z", new List<string> { "2" } },
                { "E", new List<string> { "3" } },
                { "A", new List<string> { "4", "8" } },
                { "S", new List<string> { "5" } },
                { "G", new List<string> { "6", "9" } },
                { "B", new List<string> { "6", "8", "13" } },
                { "T", new List<string> { "7" } },
                { "Y", new List<string> { "7" } },
                { "H", new List<string> { "11" } },
                { "N", new List<string> { "11" } },
                { "U", new List<string> { "11" } }
            };

        public static List<string> GenerateCombinations(string input)
        {
            List<string> RecursiveFunc(string prefix, string remaining)
            {
                if (remaining.Length == 0)
                    return new List<string> { prefix };

                string currentString = remaining.Substring(0, 1).ToUpper();
                string rest = remaining.Substring(1);

                var results = new List<string>();

                // Add the current string as is
                results.AddRange(RecursiveFunc(prefix + currentString, rest));

                // Add replacements if the string has mappings
                if (letterLookup.ContainsKey(currentString))
                {
                    foreach (var replacement in letterLookup[currentString])
                    {
                        results.AddRange(RecursiveFunc(prefix + replacement, rest));
                    }
                }

                return results;
            }

            return RecursiveFunc("", input);
        }
    }
}
