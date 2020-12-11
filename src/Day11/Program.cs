using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day11
{
    public enum SeatState
    {
        Floor,
        Empty,
        Occupied
    }

    public class SeatingModel
    {
        private SeatState[,] _seats;

        public SeatingModel(int width, int height)
        {
            Width = width;
            Height = height;

            _seats = new SeatState[Width, Height];
        }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public SeatState this[int x, int y]
        {
            get => _seats[x, y];
            set => _seats[x, y] = value;
        }

        public SeatingModel Clone()
        {
            var newSeatingModel = new SeatingModel(Width, Height);

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    newSeatingModel[i, j] = this[i, j];
                }
            }

            return newSeatingModel;
        }

        public int CountOccupiedAdjacents(int x, int y)
        {
            var count = 0;

            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i == x && j == y)
                    {
                        continue;
                    }

                    if (i < 0 || i >= Width || j < 0 || j >= Height)
                    {
                        continue;
                    }

                    if (this[i, j] == SeatState.Occupied)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public int CountOccupiedNonAdjacents(int x, int y)
        {
            var count = 0;

            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i == x && j == y)
                    {
                        continue;
                    }

                    var adjacentState = FindFirstInDirection(x, y, i - x, j - y);

                    if (adjacentState == SeatState.Occupied)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private SeatState FindFirstInDirection(int x, int y, int xIncrement, int yIncrement)
        {
            var currentX = x;
            var currentY = y;

            while (true)
            {
                currentX += xIncrement;
                currentY += yIncrement;

                if (currentX < 0 || currentY < 0 || currentX >= Width || currentY >= Height)
                {
                    break;
                }

                if (this[currentX, currentY] != SeatState.Floor)
                {
                    return this[currentX, currentY];
                }
            }

            return SeatState.Floor;
        }

        public int CountTotalOccupied()
        {
            var occupiedCount = 0;

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (this[i, j] == SeatState.Occupied)
                    {
                        occupiedCount++;
                    }
                }
            }

            return occupiedCount;
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
            var inputLines = new List<string>();

            using (var inputFile = File.OpenRead("input.txt"))
            {
                using (var reader = new StreamReader(inputFile))
                {
                    while (!reader.EndOfStream)
                    {
                        inputLines.Add(reader.ReadLine().Trim());
                    }
                }
            }

            var currentSeating = ParseSeatingModel(inputLines);
            var hasModelChanged = true;

            while (hasModelChanged)
            {
                hasModelChanged = false;
                var nextSeating = currentSeating.Clone();

                for (int i = 0; i < currentSeating.Width; i++)
                {
                    for (int j = 0; j < currentSeating.Height; j++)
                    {
                        var occupiedAdjacents = currentSeating.CountOccupiedAdjacents(i, j);

                        if (currentSeating[i, j] == SeatState.Empty && occupiedAdjacents == 0)
                        {
                            nextSeating[i, j] = SeatState.Occupied;
                            hasModelChanged = true;
                        }
                        else if (currentSeating[i, j] == SeatState.Occupied && occupiedAdjacents >= 4)
                        {
                            nextSeating[i, j] = SeatState.Empty;
                            hasModelChanged = true;
                        }
                    }
                }

                currentSeating = nextSeating;
            }

            Console.WriteLine(currentSeating.CountTotalOccupied());
        }

        private static void PartTwo()
        {
            var inputLines = new List<string>();

            using (var inputFile = File.OpenRead("input.txt"))
            {
                using (var reader = new StreamReader(inputFile))
                {
                    while (!reader.EndOfStream)
                    {
                        inputLines.Add(reader.ReadLine().Trim());
                    }
                }
            }

            var currentSeating = ParseSeatingModel(inputLines);
            var hasModelChanged = true;

            while (hasModelChanged)
            {
                hasModelChanged = false;
                var nextSeating = currentSeating.Clone();

                for (int i = 0; i < currentSeating.Width; i++)
                {
                    for (int j = 0; j < currentSeating.Height; j++)
                    {
                        var occupiedAdjacents = currentSeating.CountOccupiedNonAdjacents(i, j);

                        if (currentSeating[i, j] == SeatState.Empty && occupiedAdjacents == 0)
                        {
                            nextSeating[i, j] = SeatState.Occupied;
                            hasModelChanged = true;
                        }
                        else if (currentSeating[i, j] == SeatState.Occupied && occupiedAdjacents >= 5)
                        {
                            nextSeating[i, j] = SeatState.Empty;
                            hasModelChanged = true;
                        }
                    }
                }

                currentSeating = nextSeating;
            }

            Console.WriteLine(currentSeating.CountTotalOccupied());
        }

        private static SeatingModel ParseSeatingModel(List<string> inputLines)
        {
            var width = inputLines.First().Length;
            var height = inputLines.Count;

            var seatingState = new SeatingModel(width, height);

            var currentLine = 0;

            foreach (var line in inputLines)
            {
                int currentColumn = 0;

                foreach (var seat in line)
                {
                    switch (seat)
                    {
                        case '.':
                            seatingState[currentColumn, currentLine] = SeatState.Floor;
                            break;
                        case 'L':
                            seatingState[currentColumn, currentLine] = SeatState.Empty;
                            break;
                        case '#':
                            seatingState[currentColumn, currentLine] = SeatState.Occupied;
                            break;
                    }

                    currentColumn++;
                }

                currentLine++;
            }

            return seatingState;
        }
    }
}
