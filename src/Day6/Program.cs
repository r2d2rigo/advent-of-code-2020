using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            // PartOne();
            PartTwo();
        }

        private static void PartOne()
        {
            var totalYesAnswers = 0;

            using (var inputFile = File.OpenRead("input.txt"))
            {
                using (var reader = new StreamReader(inputFile))
                {
                    var lineBuffer = new List<char>();

                    while (!reader.EndOfStream)
                    {
                        var currentLine = reader.ReadLine().Trim();

                        if (string.IsNullOrWhiteSpace(currentLine))
                        {
                            totalYesAnswers += CountUniqueAnswers(lineBuffer);
                            lineBuffer.Clear();
                        }
                        else
                        {
                            lineBuffer.AddRange(currentLine);
                        }
                    }

                    totalYesAnswers += CountUniqueAnswers(lineBuffer);
                }
            }

            Console.WriteLine(totalYesAnswers);
        }

        private static void PartTwo()
        {
            var totalYesAnswers = 0;

            using (var inputFile = File.OpenRead("input.txt"))
            {
                using (var reader = new StreamReader(inputFile))
                {
                    int groupSize = 0;
                    var lineBuffer = new List<char>();

                    while (!reader.EndOfStream)
                    {
                        var currentLine = reader.ReadLine().Trim();

                        if (string.IsNullOrWhiteSpace(currentLine))
                        {
                            totalYesAnswers += CountGroupAnswers(lineBuffer, groupSize);
                            lineBuffer.Clear();
                            groupSize = 0;
                        }
                        else
                        {
                            groupSize++;
                            lineBuffer.AddRange(currentLine);
                        }
                    }

                    totalYesAnswers += CountGroupAnswers(lineBuffer, groupSize);
                }
            }

            Console.WriteLine(totalYesAnswers);
        }

        private static int CountUniqueAnswers(List<char> answers)
        {
            return answers.Distinct().Count();
        }

        private static int CountGroupAnswers(List<char> answers, int groupSize)
        {
            return answers
                .Distinct()
                .Select(a => answers.Count(c => c == a))
                .Where(c => c == groupSize)
                .Select(c => 1)
                .Sum();
        }
    }
}
