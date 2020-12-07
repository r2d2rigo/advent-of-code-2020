using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day7
{
    public class MagicBag
    {
        public string Name { get; private set; }

        public List<MagicBag> Parents { get; private set; }

        public List<MagicBag> Children { get; private set; }

        public MagicBag(string name)
        {
            Name = name;
            Parents = new List<MagicBag>();
            Children = new List<MagicBag>();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // PartOne();
            PartTwo();
        }

        private static void PartOne()
        {
            var allBags = new List<MagicBag>();

            using (var inputFile = File.OpenRead("input.txt"))
            {
                using (var reader = new StreamReader(inputFile))
                {
                    while (!reader.EndOfStream)
                    {
                        var currentLine = reader.ReadLine();

                        ParseLine(currentLine, allBags);
                    }
                }
            }

            var shinyGoldBag = allBags.Single(b => b.Name == "shiny gold");
            var bagsToCheck = new Queue<MagicBag>();
            bagsToCheck.Enqueue(shinyGoldBag);
            var results = new List<MagicBag>();

            FindBagParents(bagsToCheck, results);

            Console.WriteLine(results.Count);
        }

        private static void PartTwo()
        {
            var allBags = new List<MagicBag>();

            using (var inputFile = File.OpenRead("input.txt"))
            {
                using (var reader = new StreamReader(inputFile))
                {
                    while (!reader.EndOfStream)
                    {
                        var currentLine = reader.ReadLine();

                        ParseLine(currentLine, allBags);
                    }
                }
            }

            var shinyGoldBag = allBags.Single(b => b.Name == "shiny gold");
            var bagsToCheck = new Queue<MagicBag>();
            bagsToCheck.Enqueue(shinyGoldBag);

            var result = CountBagChildren(bagsToCheck);

            Console.WriteLine(result);
        }

        private static void ParseLine(string line, List<MagicBag> allBags)
        {
            var bagNameRegex = new Regex("([a-z ]+) bags contain");
            var bagContentsRegex = new Regex("(([0-9]) ([a-z ]+) bag[s]?[,.]?)+");

            var bagName = bagNameRegex.Match(line).Groups[1].Value;

            var existingBag = allBags.FirstOrDefault(b => b.Name == bagName);

            if (existingBag == null)
            {
                existingBag = new MagicBag(bagName);
                allBags.Add(existingBag);
            }

            var bagContentsMatches = bagContentsRegex.Matches(line);

            for (int i = 0; i < bagContentsMatches.Count; i++)
            {
                var match = bagContentsMatches[i];
                var containedBagCount = int.Parse(match.Groups[2].Value);
                var containedBagName = match.Groups[3].Value;

                var existingContainedBag = allBags.FirstOrDefault(b => b.Name == containedBagName);

                if (existingContainedBag == null)
                {
                    existingContainedBag = new MagicBag(containedBagName);
                    allBags.Add(existingContainedBag);
                }

                existingContainedBag.Parents.Add(existingBag);

                for (int j = 0; j < containedBagCount; j++)
                {
                    existingBag.Children.Add(existingContainedBag);
                }
            }
        }

        private static void FindBagParents(Queue<MagicBag> bagsToCheck, List<MagicBag> results)
        {
            while (bagsToCheck.Count > 0)
            {
                var currentBag = bagsToCheck.Dequeue();

                foreach (var parent in currentBag.Parents)
                {
                    if (!results.Contains(parent))
                    {
                        results.Add(parent);
                    }

                    bagsToCheck.Enqueue(parent);
                }
            }
        }

        private static int CountBagChildren(Queue<MagicBag> bagsToCheck)
        {
            var result = 0;

            while (bagsToCheck.Count > 0)
            {
                var currentBag = bagsToCheck.Dequeue();

                foreach (var child in currentBag.Children)
                {
                    result++;

                    bagsToCheck.Enqueue(child);
                }
            }

            return result;
        }
    }
}
