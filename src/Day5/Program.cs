using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day5
{
    public class Seat
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int Id => (Row * 8) + Column;
    }

    public class Partition
    {
        public int LowerBound { get; private set; }
        public int UpperBound { get; private set; }

        public Partition(int lowerBound, int upperBound)
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;
        }

        public Partition LowerHalf()
        {
            var range = (UpperBound - LowerBound) + 1;

            return new Partition(LowerBound, UpperBound - (range / 2));
        }

        public Partition UpperHalf()
        {
            var range = (UpperBound - LowerBound) + 1;

            return new Partition(LowerBound + (range / 2), UpperBound);
        }

        public override string ToString()
        {
            return $"[{LowerBound}, {UpperBound}]";
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
            var highestId = 0;

            using (var inputFile = File.OpenRead("input.txt"))
            {
                using (var reader = new StreamReader(inputFile))
                {
                    while (!reader.EndOfStream)
                    {
                        var inputLine = reader.ReadLine();

                        var seat = new Seat()
                        {
                            Row = ParseSeatRow(inputLine),
                            Column = ParseSeatColumn(inputLine),
                        };

                        if (seat.Id > highestId)
                        {
                            highestId = seat.Id;
                        }
                    }
                }
            }

            Console.Write(highestId);
        }

        private static void PartTwo()
        {
            var seatIds = new List<int>();

            using (var inputFile = File.OpenRead("input.txt"))
            {
                using (var reader = new StreamReader(inputFile))
                {
                    while (!reader.EndOfStream)
                    {
                        var inputLine = reader.ReadLine();

                        var seat = new Seat()
                        {
                            Row = ParseSeatRow(inputLine),
                            Column = ParseSeatColumn(inputLine),
                        };

                        seatIds.Add(seat.Id);
                    }
                }
            }

            seatIds.Sort();
            var previousId = seatIds.First();

            foreach (var id in seatIds)
            {
                if (id - previousId > 1)
                {
                    Console.WriteLine(id - 1);
                }

                previousId = id;
            }
        }

        private static int ParseSeatRow(string partitionString)
        {
            var rowPartition = new Partition(0, 127);
            var rowString = partitionString.Substring(0, 7);

            if (rowString.Length != 7)
            {
                throw new InvalidOperationException();
            }

            foreach (var position in rowString)
            {
                if (position == 'F')
                {
                    rowPartition = rowPartition.LowerHalf();
                }
                else if (position == 'B')
                {
                    rowPartition = rowPartition.UpperHalf();
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            if (rowPartition.LowerBound != rowPartition.UpperBound)
            {
                throw new InvalidOperationException();
            }

            return rowPartition.LowerBound;
        }

        private static int ParseSeatColumn(string partitionString)
        {
            var columnPartition = new Partition(0, 7);
            var rowString = partitionString.Substring(7, 3);

            if (rowString.Length != 3)
            {
                throw new InvalidOperationException();
            }

            foreach (var position in rowString)
            {
                if (position == 'L')
                {
                    columnPartition = columnPartition.LowerHalf();
                }
                else if (position == 'R')
                {
                    columnPartition = columnPartition.UpperHalf();
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            if (columnPartition.LowerBound != columnPartition.UpperBound)
            {
                throw new InvalidOperationException();
            }

            return columnPartition.LowerBound;
        }
    }
}
