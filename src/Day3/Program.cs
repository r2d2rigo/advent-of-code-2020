using System;
using System.Collections.Generic;
using System.IO;

namespace Day3
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
            var treeCount = 0;
            var lines = new List<string>();

            using (var inputFile = File.OpenRead("input.txt"))
            {
                using (var reader = new StreamReader(inputFile))
                {
                    while (!reader.EndOfStream)
                    {
                        lines.Add(reader.ReadLine().Trim());
                    }
                }
            }

            var currentX = 0;
            var currentY = 0;

            while (currentY < lines.Count - 1)
            {
                currentX += 3;
                currentY += 1;

                var currentLine = lines[currentY];
                var clampedX = currentX % currentLine.Length;

                if (currentLine[clampedX] == '#')
                {
                    treeCount++;
                }
            }

            Console.WriteLine(treeCount);
        }

        static void PartTwo()
        {
            var treeCount = 0;
            var lines = new List<string>();

            using (var inputFile = File.OpenRead("input.txt"))
            {
                using (var reader = new StreamReader(inputFile))
                {
                    while (!reader.EndOfStream)
                    {
                        lines.Add(reader.ReadLine().Trim());
                    }
                }
            }

            long treesRight1Down1 = CheckSlope(lines, 1, 1);
            long treesRight3Down1 = CheckSlope(lines, 3, 1);
            long treesRight5Down1 = CheckSlope(lines, 5, 1);
            long treesRight7Down1 = CheckSlope(lines, 7, 1);
            long treesRight1Down2 = CheckSlope(lines, 1, 2);

            Console.WriteLine(
                treesRight1Down1 *
                treesRight3Down1 *
                treesRight5Down1 *
                treesRight7Down1 *
                treesRight1Down2);
        }

        static int CheckSlope(List<string> lines, int rightCount, int downCount)
        {
            var currentX = 0;
            var currentY = 0;
            var treeCount = 0;

            while (currentY < lines.Count - downCount)
            {
                currentX += rightCount;
                currentY += downCount;

                var currentLine = lines[currentY];
                var clampedX = currentX % currentLine.Length;

                if (currentLine[clampedX] == '#')
                {
                    treeCount++;
                }
            }

            return treeCount;
        }
    }
}
