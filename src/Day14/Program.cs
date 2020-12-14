using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day14
{
    public class BitMask
    {
        private bool?[] _bits;

        public bool? this[int index]
        {
            get => _bits[index];
            set => _bits[index] = value;
        }

        public BitMask()
        {
            _bits = new bool?[36];
        }

        public BitMask(long value) : this()
        {
            for (int i = 0; i < 36; i++)
            {
                this[i] = (value >> i) % 2 == 1;
            }
        }

        public BitMask(string mask) : this()
        {
            if (mask.Length != 36)
            {
                throw new ArgumentException("Length must be 36", nameof(mask));
            }

            var position = 35;

            foreach (var bit in mask)
            {
                bool? bitValue = null;

                switch (bit)
                {
                    case '1':
                        bitValue = true;
                        break;
                    case '0':
                        bitValue = false;
                        break;
                    case 'X':
                        bitValue = null;
                        break;
                    default:
                        throw new InvalidOperationException();
                }

                this[position] = bitValue;
                position--;
            }
        }

        public BitMask Clone()
        {
            var result = new BitMask();

            for (int i = 0; i < 36; i++)
            {
                result[i] = this[i];
            }

            return result;
        }

        public long ToLong()
        {
            var longValue = 0L;

            for (int i = 0; i < 36; i++)
            {
                if (this[i] == null)
                {
                    throw new InvalidOperationException();
                }

                if (this[i] == true)
                {
                    longValue += (long)Math.Pow(2, i);
                }
            }

            return longValue;
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
            var memoryMap = new Dictionary<int, long>();
            BitMask currentMask = null;

            using (var inputFile = File.OpenRead("input.txt"))
            {
                using (var reader = new StreamReader(inputFile))
                {
                    while (!reader.EndOfStream)
                    {
                        var currentLine = reader.ReadLine();
                        var lineParts = currentLine.Split('=');

                        if (lineParts.First().StartsWith("mask"))
                        {
                            currentMask = new BitMask(lineParts.Last().Trim());
                        }
                        else if (lineParts.First().StartsWith("mem"))
                        {
                            var address = int.Parse(Regex.Match(lineParts.First().Trim(), "mem\\[([0-9]+)\\]").Groups[1].Value);
                            var decimalValue = long.Parse(lineParts.Last().Trim());
                            var memoryValue = 0L;

                            for (int i = 0; i < 36; i++)
                            {
                                if (currentMask[i].HasValue)
                                {
                                    if (currentMask[i].Value == true)
                                    {
                                        memoryValue |= 1L << i;
                                    }
                                    else
                                    {
                                        memoryValue &= ~(1L << i);
                                    }
                                }
                                else
                                {
                                    memoryValue |= decimalValue & 1L << i;
                                }
                            }

                            memoryMap[address] = memoryValue;
                        }
                    }
                }
            }

            var result = 0L;

            foreach (var entry in memoryMap)
            {
                result += entry.Value;
            }

            Console.WriteLine(result);
        }


        private static void PartTwo()
        {
            var memoryMap = new Dictionary<long, long>();
            BitMask memoryMask = null;

            using (var inputFile = File.OpenRead("input.txt"))
            {
                using (var reader = new StreamReader(inputFile))
                {
                    while (!reader.EndOfStream)
                    {
                        var currentLine = reader.ReadLine();
                        var lineParts = currentLine.Split('=');

                        if (lineParts.First().StartsWith("mask"))
                        {
                            memoryMask = new BitMask(lineParts.Last().Trim());
                        }
                        else if (lineParts.First().StartsWith("mem"))
                        {
                            var address = int.Parse(Regex.Match(lineParts.First().Trim(), "mem\\[([0-9]+)\\]").Groups[1].Value);
                            var decimalValue = long.Parse(lineParts.Last().Trim());
                            var addressMask = new BitMask(address);

                            for (int i = 0; i < 36; i++)
                            {
                                if (memoryMask[i].HasValue)
                                {
                                    if (memoryMask[i].Value == true)
                                    {
                                        addressMask[i] = true;
                                    }
                                }
                                else
                                {
                                    addressMask[i] = null;
                                }
                            }

                            var floatingAddresses = CalculateFloatingBitMasks(addressMask);

                            foreach (var finalAddress in floatingAddresses)
                            {
                                memoryMap[finalAddress.ToLong()] = decimalValue;
                            }
                        }
                    }
                }
            }

            var result = 0L;

            foreach (var entry in memoryMap)
            {
                result += entry.Value;
            }

            Console.WriteLine(result);
        }

        private static List<BitMask> CalculateFloatingBitMasks(BitMask bitmask)
        {
            var result = new List<BitMask>();

            for (int i = 0; i < 36; i++)
            {
                if (bitmask[i] == null)
                {
                    var falseMask = bitmask.Clone();
                    var trueMask = bitmask.Clone();
                    falseMask[i] = false;
                    trueMask[i] = true;

                    result.AddRange(CalculateFloatingBitMasks(falseMask));
                    result.AddRange(CalculateFloatingBitMasks(trueMask));

                    break;
                }

                if (i == 35)
                {
                    result.Add(bitmask);
                }
            }

            return result;
        }
    }
}
