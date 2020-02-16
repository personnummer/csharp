using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Personnummer.Exceptions;

namespace Personnummer
{
    public struct PersonnummerOptions
    {
    }


    public class Personnummer
    {

        #region Static internal.
        private static CultureInfo cultureInfo;
        private static Regex regex;

        static Personnummer()
        {
            cultureInfo = CultureInfo.CreateSpecificCulture("sv-SE");
            regex       = new Regex(@"^(\d{2}){0,1}(\d{2})(\d{2})(\d{2})([\+\-\s]?)(\d{3})(\d)$");
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
            int[] t   = value.ToCharArray().Select(d => d - 48).ToArray();
            int   sum = 0;
            int   temp;
            for (int i = t.Length; i-- > 0;)
            {
                temp = t[i];
                sum += (i % 2 == t.Length % 2)
                    ? ((temp * 2) % 10) + temp / 5
                    : temp;
            }

            return sum % 10;
        }
        #endregion

        public string Age
        {
            get;
            private set;
        }
        
        public string Century
        {
            get;
            private set;
        }
        public string FullYear
        {
            get;
            private set;
        }
        public string Year
        {
            get;
            private set;
        }
        public string Month
        {
            get;
            private set;
        }
        public string Day
        {
            get;
            private set;
        }
        public string Separator
        {
            get;
            private set;
        }
        public string Numbers
        {
            get;
            private set;
        }
        public string ControlNumber
        {
            get;
            private set;
        }
        public bool IsCoordinationNumber
        {
            get;
            private set;
        }
        public bool IsFemale
        {
            get;
            private set;
        }
        public bool IsMale
        {
            get;
            private set;
        }

        public Personnummer(string ssn, PersonnummerOptions options)
        {

            MatchCollection matches;
            try
            {
                matches = regex.Matches(ssn);
            }
            catch
            {
                throw new PersonnummerException("Failed to parse personnummer. Invalid input.");
            }

            if (matches.Count < 1 || matches[0].Groups.Count < 7)
            {
                throw new PersonnummerException("Invalid personnummer. Invalid input.");
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

            // Create datetime object.
            DateTime date = new DateTime(int.Parse($"{century}{decade}"), int.Parse(groups[3].Value), int.Parse(groups[4].Value), 0, 0, 0);


            this.Century = century;
            this.Year = decade;
            this.FullYear = century + decade;
            this.Month = groups[3].Value;
            this.Day = groups[4].Value;
            this.Numbers = groups[6].Value + groups[7].Value;
            this.ControlNumber = groups[7].Value;
        }

        public string Format(bool longFormat)
        {
            return "";
        }
    }
}
