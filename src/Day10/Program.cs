using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace Day10
{
    public class Adapter : IComparable<Adapter>
    {
        public int Joltage { get; private set; }
        public bool IsEssential { get; set; }

        public Adapter(int joltage)
        {
            Joltage = joltage;
            IsEssential = false;
        }

        public int CompareTo([AllowNull] Adapter other)
        {
            if (other == null)
            {
                return 1;
            }

            return Joltage - other.Joltage;
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
            var adapterList = new List<int>();

            using (var inputFile = File.OpenRead("input.txt"))
            {
                using (var reader = new StreamReader(inputFile))
                {
                    while (!reader.EndOfStream)
                    {
                        adapterList.Add(int.Parse(reader.ReadLine()));
                    }
                }
            }

            var currentAdapter = 0;
            var oneJoltIncrementCount = 0;
            var threeJoltIncrementCount = 1;
            adapterList.Sort();

            while (currentAdapter < adapterList.Max())
            {
                var candidates = adapterList
                    .Where(a => a > currentAdapter && a <= (currentAdapter + 3));

                var nextAdapter = candidates.First();

                if (nextAdapter - currentAdapter == 1)
                {
                    oneJoltIncrementCount++;
                }
                else if (nextAdapter - currentAdapter == 3)
                {
                    threeJoltIncrementCount++;
                }

                currentAdapter = nextAdapter;
            }

            Console.WriteLine(oneJoltIncrementCount * threeJoltIncrementCount);
        }

        private static void PartTwo()
        {
            var adapterList = new List<Adapter>();
            Adapter previousAdapter = null;

            using (var inputFile = File.OpenRead("input.txt"))
            {
                using (var reader = new StreamReader(inputFile))
                {
                    while (!reader.EndOfStream)
                    {
                        adapterList.Add(new Adapter(int.Parse(reader.ReadLine())));
                    }
                }
            }

            adapterList.Sort();

            for (int i = 0; i < adapterList.Count; i++)
            {
                var currentAdapter = adapterList[i];

                if (i == adapterList.Count - 1)
                {
                    currentAdapter.IsEssential = true;
                }
                else
                {
                    var nextAdapter = adapterList[i + 1];
                    
                    if (nextAdapter.Joltage - currentAdapter.Joltage == 3)
                    {
                        nextAdapter.IsEssential = true;
                        currentAdapter.IsEssential = true;
                    }
                }
            }

            var result = 1L;
            var currentGroupSize = 0;

            for (int i = 0; i < adapterList.Count; i++)
            {
                var currentAdapter = adapterList[i];

                if (currentAdapter.IsEssential)
                {
                    result *= (long)Math.Pow(2, currentGroupSize) - (currentGroupSize / 3);

                    currentGroupSize = 0;
                }
                else
                {
                    currentGroupSize++;
                }
            }

            Console.WriteLine(result);
        }
    }
}
