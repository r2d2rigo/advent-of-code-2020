using System;
using System.Collections.Generic;
using System.IO;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            // PartOne();
            PartTwo();
        }

        static void PartOne()
        {
            var lines = new List<int>();

            using (var inputFile = File.OpenRead("input.txt"))
            {
                using (var reader = new StreamReader(inputFile))
                {
                    while (!reader.EndOfStream)
                    {
                        var lineString = reader.ReadLine();
                        lines.Add(int.Parse(lineString));
                    }
                }
            }

            for (int i = 0; i < lines.Count - 1; ++i)
            {
                var currentNumber = lines[i];

                for (int j = i + 1; j < lines.Count; ++j)
                {
                    var nextNumber = lines[j];

                    if (currentNumber + nextNumber == 2020)
                    {
                        Console.WriteLine((currentNumber * nextNumber).ToString());
                        return;
                    }
                }
            }
        }

        static void PartTwo()
        {
            var lines = new List<int>();

            using (var inputFile = File.OpenRead("input.txt"))
            {
                using (var reader = new StreamReader(inputFile))
                {
                    while (!reader.EndOfStream)
                    {
                        var lineString = reader.ReadLine();
                        lines.Add(int.Parse(lineString));
                    }
                }
            }

            for (int i = 0; i < lines.Count - 2; ++i)
            {
                var firstNumber = lines[i];

                for (int j = i + 1; j < lines.Count - 1; ++j)
                {
                    var secondNumber = lines[j];

                    for (int k = j + 1; k < lines.Count; k++)
                    {
                        var thirdNumber = lines[k];

                        if (firstNumber + secondNumber + thirdNumber == 2020)
                        {
                            Console.WriteLine((firstNumber * secondNumber * thirdNumber).ToString());
                            return;
                        }
                    }
                }
            }
        }
    }
}
