using System;
using System.Linq;
using System.Text.RegularExpressions;
using Personnummer.Exceptions;

namespace Personnummer
{
    public class Personnummer
    {
        public struct Options
        {
            public bool AllowCoordinationNumber { get; set; }
        }

        #region Fields and Properties

        private readonly int realDay;

        public int Age => (DateTime.Now.Year - (new DateTime(int.Parse($"{FullYear}"), int.Parse(Month), realDay, 0, 0, 0)).Year);
        public string Separator => Age >= 100 ? "+" : "-";

        public string Century { get; }

        public string FullYear { get; }

        public string Year { get; }

        public string Month { get; }

        public string Day { get; }

        public string Numbers { get; }

        public string ControlNumber { get; }

        public bool IsCoordinationNumber { get; } = false;

        public bool IsFemale { get; } = false;

        public bool IsMale { get; } = false;

        #endregion

        /// <summary>
        /// Create a new Personnummber object from a string.
        ///
        /// In case options is not passed, they will default to accept any personal and coordination numbers.
        /// </summary>
        /// <exception cref="PersonnummerException">On parse error.</exception>
        /// <param name="ssn">Personal identity number - as string - to create from.</param>
        /// <param name="options">Options object.</param>
        public Personnummer(string ssn, Options? options = null)
        {
            options ??= new Options() { AllowCoordinationNumber = true };
            MatchCollection matches;
            try
            {
                matches = regex.Matches(ssn);
                if (matches.Count < 1 || matches[0].Groups.Count < 7)
                {
                    throw new Exception(); // Just to enter the catch, as it is invalid.
                }
            }
            catch
            {
                throw new PersonnummerException("Failed to parse personal identity number. Invalid input.");
            }

            GroupCollection groups = matches[0].Groups;
            string          century;
            string          decade = groups[2].Value;

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

            // Create date time object.
            int day = int.Parse(groups[4].Value);
            if (options.Value.AllowCoordinationNumber)
            {
                IsCoordinationNumber = day > 60;
                day = IsCoordinationNumber ? day - 60 : day;
            }
            else if (day > 60)
            {
                throw new PersonnummerException("Invalid personal identity number.");
            }

            realDay       = day;
            Century       = century;
            Year          = decade;
            FullYear      = century + decade;
            Month         = groups[3].Value;
            Day           = groups[4].Value;
            Numbers       = groups[6].Value + groups[7].Value;
            ControlNumber = groups[7].Value;

            IsMale = int.Parse(Numbers[2].ToString()) % 2 == 1;
            IsFemale = !IsMale;

            // Try parse date-time to make sure it's actually a real date.
            if (!DateTime.TryParse($"{FullYear}-{Month:00}-{realDay:00}", out _))
            {
                throw new PersonnummerException("Invalid personal identity number.");
            }

            if (Luhn($"{Year}{Month}{Day}{groups[6].Value}") != int.Parse(ControlNumber))
            {
                throw new PersonnummerException("Invalid personal identity number.");
            }
        }

        /// <summary>
        /// Format the personal identity number into a valid string (YYMMDD-/+XXXX)
        /// If longFormat is true, it will include the century (YYYYMMDD-/+XXXX)
        /// </summary>
        /// <param name="longFormat">If century should be included.</param>
        /// <param name="ignoreSeparator">Whether the separator should be ignored.</param>
        /// <returns>Formatted personal identity number.</returns>
        public string Format(bool longFormat = false, bool ignoreSeparator = false)
        {
            return ignoreSeparator ? $"{(longFormat ? FullYear : Year)}{Month}{Day}{Numbers}" : $"{(longFormat ? FullYear : Year)}{Month}{Day}{Separator}{Numbers}";
        }

        /// <summary>
        /// Parse a personal identity number - as string - into a Personnummer object.
        ///
        /// In case options is not passed, they will default to accept any personal identity and coordination numbers.
        /// </summary>
        /// <param name="ssn">Personal identity number to parse.</param>
        /// <param name="options">Options object.</param>
        /// <exception cref="PersonnummerException">On invalid personal identity number.</exception>
        /// <returns>Personal identity number as a Personnummer object.</returns>
        public static Personnummer Parse(string ssn, Options? options = null)
        {
            return new Personnummer(ssn, options);
        }

        /// <summary>
        /// Test a personal identity number to see if it is valid or not.
        /// </summary>
        /// <param name="ssn">Personal identity number to test.</param>
        /// <returns>True if valid, else false.</returns>
        public static bool Valid(string ssn)
        {
            try
            {
                Parse(ssn);
                return true;
            }
            catch(PersonnummerException)
            {
                return false;
            }
        }

        /// <summary>
        /// Test a personal identity number to see if it is valid or not, and also checks if passed personal identity number is according to specified format
        /// </summary>
        /// <param name="ssn">Personal identity number to test.</param>
        /// <param name="longFormat">If century should be included.</param>
        /// <param name="ignoreSeparator">Whether the separator should be ignored.</param>
        /// <returns>True if valid format, else false.</returns>
        public static bool ValidFormat(string ssn, bool longFormat = false, bool ignoreSeparator = false)
        {
            try
            {
                return true && Parse(ssn).Format(longFormat,ignoreSeparator) == ssn;
            }
            catch (PersonnummerException)
            {
                return false;
            }
        }

        #region Static internal.
        private static readonly Regex regex = new Regex(@"^(\d{2}){0,1}(\d{2})(\d{2})(\d{2})([\+\-]?)((?!000)\d{3})(\d)$");

        private static int Luhn(string value)
        {
            // Luhm algorithm doubles every other number in the value.
            // To get the correct checksum digit we aught to append a 0 on the sequence.
            // If the result becomes a two digit number, subtract 9 from the value.
            // If the total sum is not a 0, the last checksum value should be subtracted from 10.
            // The resulting value is the check value that we use as control number.

            // The value passed is a string, so we aught to get the actual integer value from each char (i.e., subtract '0' which is 48).
            int[] t   = value.ToCharArray().Select(d => d - 48).ToArray();
            int   sum = 0;
            int   temp;
            for (int i = 0; i < t.Length; i++)
            {
                temp = t[i];
                temp *= 2 - (i % 2);
                if (temp > 9)
                {
                    temp -= 9;
                }
                sum += temp;
            }

            return ((int)Math.Ceiling(sum / 10.0)) * 10 - sum;
        }

        #endregion
    }
}
