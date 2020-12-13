using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day13
{
    class Program
    {
        public class BusSchedule
        {
            public BusSchedule(int busId, int departureOffset)
            {
                BusId = busId;
                DepartureOffset = departureOffset;
            }

            public int BusId { get; private set; }

            public int DepartureOffset { get; private set; }
        }

        static void Main(string[] args)
        {
            PartOne();
        }

        private static void PartOne()
        {
            var earliestDepartureTime = 0;
            var timestamps = new List<int>();

            using (var inputFile = File.OpenRead("input.txt"))
            {
                using (var reader = new StreamReader(inputFile))
                {
                    earliestDepartureTime = int.Parse(reader.ReadLine());

                    var timestampLine = reader.ReadLine();
                    var timestampEntries = timestampLine.Split(',');

                    foreach (var entry in timestampEntries)
                    {
                        var intEntry = 0;

                        if (int.TryParse(entry, out intEntry))
                        {
                            timestamps.Add(intEntry);
                        }
                    }
                }
            }

            var waitTime = int.MaxValue;
            var busId = -1;

            foreach (var timestamp in timestamps)
            {
                var approximation = (int)Math.Ceiling(earliestDepartureTime / (double)timestamp) * timestamp;

                if (approximation < waitTime)
                {
                    waitTime = approximation;
                    busId = timestamp;
                }
            }

            Console.WriteLine(busId * (waitTime - earliestDepartureTime));
        }
    }
}
