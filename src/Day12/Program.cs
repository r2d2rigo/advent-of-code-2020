using System;
using System.IO;
using System.Linq;

namespace Day12
{
    public enum Direction : int
    {
        East = 0,
        South = 1,
        West = 2,
        North = 3
    }

    public class FerryShip
    {
        private static readonly int[] X_DELTAS = new[]
        {
            1,
            0,
            -1,
            0,
        };

        private static readonly int[] Y_DELTAS = new[]
        {
            0,
            -1,
            0,
            1,
        };

        private int _rotation = 0;

        public FerryShip()
        {
            X = 0;
            Y = 0;
        }

        public int X { get; private set; }

        public int Y { get; private set; }

        public Direction FacingDirection { get; private set; }

        public void Move(Direction direction, int distance)
        {
            X += X_DELTAS[(int)direction] * distance;
            Y += Y_DELTAS[(int)direction] * distance;
        }

        public void MoveForward(int distance)
        {
            Move(FacingDirection, distance);
        }

        public void RotateRight(int degrees)
        {
            _rotation += degrees;

            if (_rotation >= 360)
            {
                _rotation = _rotation - 360;
            }

            FacingDirection = (Direction)(_rotation / 90);
        }

        public void RotateLeft(int degrees)
        {
            _rotation -= degrees;

            if (_rotation < 0)
            {
                _rotation = _rotation + 360;
            }

            FacingDirection = (Direction)(_rotation / 90);
        }

        public void MoveTowardsWaypoint(Waypoint waypoint, int times)
        {
            X += waypoint.X * times;
            Y += waypoint.Y * times;
        }
    }

    public class Waypoint
    {
        private static readonly int[] X_DELTAS = new[]
        {
            1,
            0,
            -1,
            0,
        };

        private static readonly int[] Y_DELTAS = new[]
        {
            0,
            -1,
            0,
            1,
        };

        public Waypoint()
        {
            X = 0;
            Y = 0;
        }

        public int X { get; private set; }

        public int Y { get; private set; }

        public void Move(Direction direction, int distance)
        {
            X += X_DELTAS[(int)direction] * distance;
            Y += Y_DELTAS[(int)direction] * distance;
        }

        public void RotateRight(int degrees)
        {
            var radians = -degrees * (Math.PI / 180.0);

            var newX = X * (int)Math.Cos(radians) - Y * (int)Math.Sin(radians);
            var newY = X * (int)Math.Sin(radians) + Y * (int)Math.Cos(radians);

            X = newX;
            Y = newY;
        }

        public void RotateLeft(int degrees)
        {
            var radians = degrees * (Math.PI / 180.0);

            var newX = X * (int)Math.Cos(radians) - Y * (int)Math.Sin(radians);
            var newY = X * (int)Math.Sin(radians) + Y * (int)Math.Cos(radians);

            X = newX;
            Y = newY;
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
            var ship = new FerryShip();

            using (var inputFile = File.OpenRead("input.txt"))
            {
                using (var reader = new StreamReader(inputFile))
                {
                    while (!reader.EndOfStream)
                    {
                        var currentLine = reader.ReadLine();

                        var command = currentLine.First();
                        var value = int.Parse(currentLine.Substring(1));

                        switch (command)
                        {
                            case 'N':
                                ship.Move(Direction.North, value);
                                break;
                            case 'E':
                                ship.Move(Direction.East, value);
                                break;
                            case 'S':
                                ship.Move(Direction.South, value);
                                break;
                            case 'W':
                                ship.Move(Direction.West, value);
                                break;
                            case 'L':
                                ship.RotateLeft(value);
                                break;
                            case 'R':
                                ship.RotateRight(value);
                                break;
                            case 'F':
                                ship.MoveForward(value);
                                break;
                        }
                    }
                }
            }

            Console.WriteLine(Math.Abs(ship.X) + Math.Abs(ship.Y));
        }

        private static void PartTwo()
        {
            var ship = new FerryShip();
            var waypoint = new Waypoint();
            waypoint.Move(Direction.East, 10);
            waypoint.Move(Direction.North, 1);

            using (var inputFile = File.OpenRead("input.txt"))
            {
                using (var reader = new StreamReader(inputFile))
                {
                    while (!reader.EndOfStream)
                    {
                        var currentLine = reader.ReadLine();

                        var command = currentLine.First();
                        var value = int.Parse(currentLine.Substring(1));

                        switch (command)
                        {
                            case 'N':
                                waypoint.Move(Direction.North, value);
                                break;
                            case 'E':
                                waypoint.Move(Direction.East, value);
                                break;
                            case 'S':
                                waypoint.Move(Direction.South, value);
                                break;
                            case 'W':
                                waypoint.Move(Direction.West, value);
                                break;
                            case 'L':
                                waypoint.RotateLeft(value);
                                break;
                            case 'R':
                                waypoint.RotateRight(value);
                                break;
                            case 'F':
                                ship.MoveTowardsWaypoint(waypoint, value);
                                break;
                        }
                    }
                }
            }

            Console.WriteLine(Math.Abs(ship.X) + Math.Abs(ship.Y));
        }
    }
}
