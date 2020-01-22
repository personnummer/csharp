using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Personnummer
{
    /// <summary>
    /// Class used to verify Swedish social security numbers.
    /// </summary>
    public static class Personnummer
    {
        private struct ParsedPersonnummer
        {
            public string Century;
            public string Decade;
            public string Month;
            public string Day;
            public string Digits;

            public bool Parsed => !string.IsNullOrEmpty(Digits);
        }

        private static readonly Regex regex;
        private static readonly CultureInfo cultureInfo;

        static Personnummer()
        {
            cultureInfo = CultureInfo.CreateSpecificCulture("sv-SE");
            regex = new Regex(@"^(\d{2}){0,1}(\d{2})(\d{2})(\d{2})([\+\-\s]?)(\d{3})(\d)$");
        }

        /// <summary>
        /// Calculates the checksum value of a given digit-sequence as string by using the luhn/mod10 algorithm.
        /// </summary>
        /// <param name="value">Sequence of digits as a string.</param>
        /// <returns>Resulting checksum value.</returns>
        private static int Luhn(string value)
        {
            // Luhm algorithm doubles every other number in the value.
            // To get the correct checksum digit we aught to append a 0 on the sequence.
            // If the result becomes a two digit number, subtract 9 from the value.
            // If the total sum is not a 0, the last checksum value should be subtracted from 10.
            // The resulting value is the check value that we use as control number.

            // The value passed is a string, so we aught to get the actual integer value from each char (i.e., subtract '0' which is 48).
            int[] t = value.ToCharArray().Select(d => d - 48).ToArray();
            int sum = 0;
            int temp;
            for (int i = t.Length; i -->0; )
            {
                temp = t[i];
                sum += (i % 2 == t.Length % 2)
                    ? ((temp * 2) % 10) + temp / 5
                    : temp;
            }

            return sum % 10;
        }

        /// <summary>
        /// Parse a social security number into a ParsedPersonnummer structure.
        /// </summary>
        /// <param name="value">Value as string to parse.</param>
        /// <returns>Struct.</returns>
        private static ParsedPersonnummer Parse(string value)
        {
            MatchCollection matches;
            try
            {
                matches = regex.Matches(value);
            }
            catch
            {
                return new ParsedPersonnummer();
            }

            if (matches.Count < 1 || matches[0].Groups.Count < 7)
            {
                return new ParsedPersonnummer();
            }

            GroupCollection groups = matches[0].Groups;
            string century;
            string decade = groups[2].Value;

            if (!string.IsNullOrEmpty(groups[1].Value))
            {
                century = groups[1].Value;
            }
            else
            {
                int born = DateTime.Now.Year - int.Parse(decade);
                if (groups[5].Value.Length != 0 && groups[5].Value == "+")
                {
                    born -= 100;
                }

                century = born.ToString().Substring(0, 2);
            }

            return new ParsedPersonnummer()
            {
                Century = century,
                Decade = decade,
                Month = groups[3].Value,
                Day = groups[4].Value,
                Digits = $"{groups[6]}{groups[7]}"
            };

        }

        /// <summary>
        /// Function to make sure that the passed year, month and day is parseable to a date.
        /// </summary>
        /// <param name="century">Century as string.</param>
        /// <param name="decade">Decade as string.</param>
        /// <param name="month">Month as string.</param>
        /// <param name="day">Day as string.</param>
        /// <returns>Result.</returns>
        private static bool TestDate(string century, string decade, string month, string day)
        {
            return DateTime.TryParse($"{century}{decade}/{month}/{day}", cultureInfo, DateTimeStyles.None, out _);
        }

        /// <summary>
        /// Validate Swedish social security number using a string value type
        /// </summary>
        /// <remarks>
        /// The accepted input formats are:
        ///
        /// YYYYMMDDXXXX
        /// YYMMDDXXXX
        /// YYYYMMDD-XXXX
        /// YYYYMMDD+XXXX
        /// YYMMDD-XXXX
        /// YYMMDD+XXXX
        /// </remarks>
        /// <param name="value">Value as string.</param>
        /// <param name="includeCoordinationNumber">If set to false, the method will return false if the value is a valid coordination number but not a social security number.</param>
        /// <returns>Result.</returns>
        public static bool Valid(string value, bool includeCoordinationNumber = true) => Valid(Parse(value), includeCoordinationNumber);

        /// <summary>
        /// Validate Swedish social security number using a Int64/Long value type.
        /// </summary>
        /// <remarks>
        /// The accepted input formats are:
        ///
        /// YYYYMMDDXXXX
        /// </remarks>
        /// <param name="value">Social security number as Int64/Long.</param>
        /// <param name="includeCoordinationNumber">If set to false, the method will return false if the value is a valid coordination number but not a social security number.</param>
        /// <returns>Result.</returns>
        public static bool Valid(long value, bool includeCoordinationNumber = true) => Valid(value.ToString(), includeCoordinationNumber);

        /// <summary>
        /// Validated a parsed social security number.
        /// </summary>
        /// <param name="parsed">Parsed value to validate.</param>
        /// <param name="includeCoordinationNumber">If set to false, the method will return false if the value is a valid coordination number but not a social security number.</param>
        /// <returns></returns>
        private static bool Valid(ParsedPersonnummer parsed, bool includeCoordinationNumber = true)
        {
            if (!parsed.Parsed)
            {
                return false;
            }

            bool valid = TestDate(parsed.Century, parsed.Decade, parsed.Month, parsed.Day);
            if (!valid && includeCoordinationNumber)
            {
                valid = TestDate(parsed.Century, parsed.Decade, parsed.Month, (int.Parse(parsed.Day) - 60).ToString());
            }

            return valid && Luhn($"{parsed.Decade}{parsed.Month}{parsed.Day}{parsed.Digits}") == 0;

        }

        /// <summary>
        /// Format a Swedish social security number in long format to a string format.
        /// </summary>
        /// <param name="value">Value as long.</param>
        /// <param name="withCentury">Include century instead of +/-.</param>
        /// <returns>Swedish social security number as a string.</returns>
        /// <exception cref="ValidationException">On invalid social security or coordination number.</exception>
        public static string Format(long value, bool withCentury = false)
        {
            ParsedPersonnummer parsed = Parse(value.ToString());
            if (!parsed.Parsed ||  Luhn($"{parsed.Decade}{parsed.Month}{parsed.Day}{parsed.Digits}") != 0)
            {
                throw new ValidationException($"{value} is not a valid Swedish social security or coordination number.");
            }

            if (withCentury)
            {
                return $"{parsed.Century}{parsed.Decade}{parsed.Month}{parsed.Day}{parsed.Digits}";
            }

            char sign = ((DateTime.Now.Year - int.Parse(parsed.Century + parsed.Decade)) >= 100) ? '+' : '-';
            return $"{parsed.Decade}{parsed.Month}{parsed.Day}{sign}{parsed.Digits}";
        }

        /// <summary>
        /// Format a Swedish social security number in string format to a long format.
        /// </summary>
        /// <param name="value">Value as string.</param>
        /// <param name="withCentury">Include century in output value.</param>
        /// <returns>Swedish social security number as a long.</returns>
        /// <exception cref="ValidationException">On invalid social security or coordination number.</exception>
        public static long Format(string value, bool withCentury = false)
        {
            ParsedPersonnummer parsed = Parse(value);
            if (!parsed.Parsed || Luhn($"{parsed.Decade}{parsed.Month}{parsed.Day}{parsed.Digits}") != 0)
            {
                throw new ValidationException($"{value} is not a valid Swedish social security or coordination number.");
            }

            if (withCentury)
            {
                return long.Parse($"{parsed.Century}{parsed.Decade}{parsed.Month}{parsed.Day}{parsed.Digits}");
            }

            return long.Parse($"{parsed.Decade}{parsed.Month}{parsed.Day}{parsed.Digits}");
        }
    }
}
