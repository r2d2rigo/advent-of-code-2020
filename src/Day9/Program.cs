using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day9
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
            var numbers = new List<long>();

            using (var inputFile = File.OpenRead("input.txt"))
            {
                using (var reader = new StreamReader(inputFile))
                {
                    while (!reader.EndOfStream)
                    {
                        numbers.Add(long.Parse(reader.ReadLine()));
                    }
                }
            }

            var preambleSize = 25;

            for (int i = preambleSize; i < numbers.Count; i++)
            {
                var isValid = false;
                var currentNumber = numbers[i];

                var preambleNumbers = numbers
                    .Skip(i - preambleSize)
                    .Take(preambleSize)
                    .ToList();

                for (int j = 0; j < preambleNumbers.Count; j++)
                {
                    if (preambleNumbers.Contains(currentNumber - preambleNumbers[j]))
                    {
                        isValid = true;
                    }
                }

                if (!isValid)
                {
                    Console.WriteLine(currentNumber);
                    return;
                }
            }
        }

        private static void PartTwo()
        {
            var numbers = new List<long>();

            using (var inputFile = File.OpenRead("input.txt"))
            {
                using (var reader = new StreamReader(inputFile))
                {
                    while (!reader.EndOfStream)
                    {
                        numbers.Add(long.Parse(reader.ReadLine()));
                    }
                }
            }

            var targetNumber = 258585477;

            for (int i = 0; i < numbers.Count; i++)
            {
                var sum = 0L;
                var candidates = new List<long>();

                for (int j = i; j < numbers.Count; j++)
                {
                    sum += numbers[j];
                    candidates.Add(numbers[j]);

                    if (sum > targetNumber)
                    {
                        break;
                    }
                    else if (sum == targetNumber)
                    {
                        candidates.Sort();
                        Console.WriteLine(candidates.First() + candidates.Last());

                        return;
                    }
                }
            }
        }
    }
}
