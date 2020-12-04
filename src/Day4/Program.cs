using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day4
{
    public class Passport
    {
        public string BirthYear { get; set; } = string.Empty;
        public string IssueYear { get; set; } = string.Empty;
        public string ExpirationYear { get; set; } = string.Empty;
        public string Height { get; set; } = string.Empty;
        public string HairColor { get; set; } = string.Empty;
        public string EyeColor { get; set; } = string.Empty;
        public string PassportId { get; set; } = string.Empty;
        public string CountryId { get; set; } = string.Empty;

        public bool IsValidPartOne
        {
            get => !string.IsNullOrEmpty(BirthYear) &&
                !string.IsNullOrEmpty(IssueYear) &&
                !string.IsNullOrEmpty(ExpirationYear) &&
                !string.IsNullOrEmpty(Height) &&
                !string.IsNullOrEmpty(HairColor) &&
                !string.IsNullOrEmpty(EyeColor) &&
                !string.IsNullOrEmpty(PassportId);
        }

        public bool IsValidPartTwo
        {
            get
            {
                return IsValidBirthYear() &&
                    IsValidIssueYear() &&
                    IsValidExpirationYear() &&
                    IsValidHeight() &&
                    IsValidHairColor() &&
                    IsValidEyeColor() &&
                    IsValidPassportId();
            }
        }

        private bool IsValidBirthYear()
        {
            var numericValue = 0;

            if (BirthYear.Length != 4)
            {
                return false;
            }

            if (!int.TryParse(BirthYear, out numericValue))
            {
                return false;
            }

            if (numericValue < 1920 || numericValue > 2002)
            {
                return false;
            }

            return true;
        }
    
        private bool IsValidIssueYear()
        {
            var numericValue = 0;

            if (IssueYear.Length != 4)
            {
                return false;
            }

            if (!int.TryParse(IssueYear, out numericValue))
            {
                return false;
            }

            if (numericValue < 2010 || numericValue > 2020)
            {
                return false;
            }

            return true;
        }
    
        private bool IsValidExpirationYear()
        {
            var numericValue = 0;

            if (ExpirationYear.Length != 4)
            {
                return false;
            }

            if (!int.TryParse(ExpirationYear, out numericValue))
            {
                return false;
            }

            if (numericValue < 2020 || numericValue > 2030)
            {
                return false;
            }

            return true;
        }

        private bool IsValidHeight()
        {
            if (Height.EndsWith("cm"))
            {
                var centimeters = int.Parse(Regex.Match(Height, "^[0-9]*").Value);

                return centimeters >= 150 && centimeters <= 193;
            }
            else if (Height.EndsWith("in"))
            {
                var inches = int.Parse(Regex.Match(Height, "^[0-9]*").Value);

                return inches >= 59 && inches <= 76;
            }

            return false;
        }

        private bool IsValidHairColor()
        {
            return Regex.IsMatch(HairColor, "^#[0-9a-f]{6}$");
        }

        private bool IsValidEyeColor()
        {
            switch (EyeColor)
            {
                case "amb":
                case "blu":
                case "brn":
                case "gry":
                case "grn":
                case "hzl":
                case "oth":
                    return true;
                default:
                    return false;
            }
        }
        
        private bool IsValidPassportId()
        {
            return Regex.IsMatch(PassportId, "^[0-9]{9}$");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // PartOne();
            PartTwo();
        }

        static void PartOne()
        {
            var validPassportCount = 0;
            var passport = new Passport();

            using (var inputFile = File.OpenRead("input.txt"))
            {
                using (var reader = new StreamReader(inputFile))
                {
                    while (!reader.EndOfStream)
                    {
                        var currentLine = reader.ReadLine().Trim();

                        if (string.IsNullOrWhiteSpace(currentLine))
                        {
                            if (passport.IsValidPartOne)
                            {
                                validPassportCount++;
                            }

                            passport = new Passport();

                            continue;
                        }

                        ParsePassportLine(passport, currentLine);
                    }

                    if (passport.IsValidPartOne)
                    {
                        validPassportCount++;
                    }
                }
            }

            Console.WriteLine(validPassportCount);
        }

        static void PartTwo()
        {
            var validPassportCount = 0;
            var passport = new Passport();

            using (var inputFile = File.OpenRead("input.txt"))
            {
                using (var reader = new StreamReader(inputFile))
                {
                    while (!reader.EndOfStream)
                    {
                        var currentLine = reader.ReadLine().Trim();

                        if (string.IsNullOrWhiteSpace(currentLine))
                        {
                            if (passport.IsValidPartTwo)
                            {
                                validPassportCount++;
                            }

                            passport = new Passport();

                            continue;
                        }

                        ParsePassportLine(passport, currentLine);
                    }

                    if (passport.IsValidPartTwo)
                    {
                        validPassportCount++;
                    }
                }
            }

            Console.WriteLine(validPassportCount);
        }

        static void ParsePassportLine(Passport passport, string line)
        {
            var parts = line.Split(' ');

            foreach (var part in parts)
            {
                var partProperties = part.Split(':');
                var fieldValue = partProperties.Skip(1).First();

                switch (partProperties.First())
                {
                    case "ecl":
                        passport.EyeColor = fieldValue;
                        break;
                    case "pid":
                        passport.PassportId = fieldValue;
                        break;
                    case "iyr":
                        passport.IssueYear = fieldValue;
                        break;
                    case "eyr":
                        passport.ExpirationYear = fieldValue;
                        break;
                    case "hcl":
                        passport.HairColor = fieldValue;
                        break;
                    case "cid":
                        passport.CountryId = fieldValue;
                        break;
                    case "byr":
                        passport.BirthYear = fieldValue;
                        break;
                    case "hgt":
                        passport.Height = fieldValue;
                        break;
                }
            }
        }
    }
}
