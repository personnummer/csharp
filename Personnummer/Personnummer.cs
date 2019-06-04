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
            public string decade;
            public string month;
            public string day;
            public string digits;

            public int Decade => Int32.Parse(decade);

            public int Month => Int32.Parse(month);

            public int Day => Int32.Parse(day);
            
            public string Check => digits.Substring(3, 1);

            public bool Parsed => !string.IsNullOrEmpty(digits);
        }

        private static readonly Regex regex;
        private static readonly CultureInfo cultureInfo;

        static Personnummer()
        {
            cultureInfo = new CultureInfo("sv-SE");
            regex       = new Regex(@"^(\d{2}){0,1}(\d{2})(\d{2})(\d{2})([-|+]{0,1})?(\d{3})(\d{0,1})$");
        }

        /// <summary>
        /// Calculates the checksum value of a given digit-sequence as string by using the luhn/mod10 algorithm.
        /// </summary>
        /// <param name="value">Sequense of digits as a string.</param>
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
        /// Function to make sure that the passed year, month and day is parseable to a date.
        /// </summary>
        /// <param name="year">Years as string.</param>
        /// <param name="month">Month as string.</param>
        /// <param name="day">Day as string.</param>
        /// <returns>Result.</returns>
        private static bool TestDate(string year, string month, string day)
        {
            try
            {
                DateTime dt = new DateTime(cultureInfo.Calendar.ToFourDigitYear(int.Parse(year)), int.Parse(month), int.Parse(day));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validate Swedish social security number.
        /// </summary>
        /// <param name="value">Value as string.</param>
        /// <returns>Result.</returns>
        public static bool Valid(string value)
        {
            ParsedPersonnummer parsed = Parse(value);

            if (!parsed.Parsed)
            {
                return false;
            }


            bool valid = Luhn($"{parsed.decade}{parsed.month}{parsed.day}{parsed.digits}") == 0;
            return valid && (TestDate(parsed.decade, parsed.month, parsed.day) || TestDate(parsed.day, parsed.month, (int.Parse(parsed.day) - 60).ToString()));
        }


        private static ParsedPersonnummer Parse(string value)
        {
            MatchCollection matches = regex.Matches(value);

            if (matches.Count < 1 || matches[0].Groups.Count < 7)
            {
                return new ParsedPersonnummer();
            }

            GroupCollection groups = matches[0].Groups;

            try
            {
                return new ParsedPersonnummer()
                {
                    decade = (groups[2].Value.Length == 4) ? groups[2].Value.Substring(2) : groups[2].Value,
                    month  = groups[3].Value,
                    day    = groups[4].Value,
                    digits = $"{groups[6]}{groups[7]}"
                };
            }
            catch
            {
                // Could not parse. So invalid.
                return new ParsedPersonnummer();
            }
        }

        /// <summary>
        /// Validate Swedish social security number.
        /// </summary>
        /// <param name="value">Value as long.</param>
        /// <returns>Result.</returns>
        public static bool Valid(long value)
        {
            return Valid(value.ToString());
        }
    }
}
