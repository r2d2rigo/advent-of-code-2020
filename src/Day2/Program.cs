using System;
using System.IO;
using System.Linq;

namespace Day2
{
    class Program
    {
        public class PasswordPolicy
        {
            public PasswordPolicy(int lowestCount, int highestCount, char letter)
            {
                LowestCount = lowestCount;
                HighestCount = highestCount;
                Letter = letter;
            }

            public int LowestCount { get; }
            public int HighestCount { get; }
            public char Letter { get; }
        }

        static void Main(string[] args)
        {
            int matchCount = 0;

            using (var inputFile = File.OpenRead("input.txt"))
            {
                using (var reader = new StreamReader(inputFile))
                {
                    while (!reader.EndOfStream)
                    {
                        var lineString = reader.ReadLine();

                        var passwordPolicy = ParsePolicy(lineString);
                        var password = ParsePassword(lineString);

                        // if (PasswordMatchesPartOne(passwordPolicy, password))
                        if (PasswordMatchesPartTwo(passwordPolicy, password))
                        {
                            matchCount++;
                        }
                    }
                }
            }

            Console.WriteLine(matchCount.ToString());
        }

        private static PasswordPolicy ParsePolicy(string entryLine)
        {
            var policy = entryLine
                .Split(':')
                .First()
                .Trim();

            var policyParts = policy
                .Split(' ');

            var policyLetter = policyParts
                .Skip(1)
                .First()
                .First();

            var policyRange = policyParts
                .First()
                .Split('-');

            var policyLowest = int.Parse(policyRange.First());
            var policyHighest = int.Parse(policyRange.Skip(1).First());

            return new PasswordPolicy(policyLowest, policyHighest, policyLetter);
        }

        private static string ParsePassword(string entryLine)
        {
            return entryLine
                .Split(':')
                .Skip(1)
                .First()
                .Trim();
        }

        static bool PasswordMatchesPartOne(PasswordPolicy policy, string password)
        {
            var replaced = password.Trim().Replace(policy.Letter.ToString(), string.Empty);
            var occurrenceCount = password.Trim().Length - replaced.Length;

            return (occurrenceCount >= policy.LowestCount && occurrenceCount <= policy.HighestCount);
        }

        static bool PasswordMatchesPartTwo(PasswordPolicy policy, string password)
        {
            var trimmedPassword = password.Trim();
            var matchesFirst = trimmedPassword[policy.LowestCount - 1] == policy.Letter;
            var matchesSecond = trimmedPassword[policy.HighestCount - 1] == policy.Letter;

            return (matchesFirst || matchesSecond) && !(matchesFirst && matchesSecond);
        }
    }
}
