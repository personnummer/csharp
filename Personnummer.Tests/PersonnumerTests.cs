using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace Personnummer.Tests
{
    public class PersonnumerTests
    {

        /*
         VALID NUMBERS:
            - 8507099805 (nummer)
            - 198507099805
            - 198507099813
            - 850709-9813
            - 196411139808
        INVALID NUMBERS:
            - 19850709980
            - 19850709981
            - 19641113980
        VALID COORD NUMBERS:
            - 198507699802
            - 850769-9802
            - 198507699810
            - 850769-9810
        INVALID COORD NUMBERS
            - 198567099805

         */


        [Theory]
        [InlineData("198507099805", true)]
        [InlineData("198507099813", true)]
        [InlineData("850709-9813", true)]
        [InlineData("196411139808", true)]
        [InlineData("8507099805", true)]
        [InlineData("19850709980", false)]
        [InlineData("19850709981", false)]
        [InlineData("19641113980", false)]
        public void TestPersonnummerString(string value, bool expected)
        {
            Assert.Equal(Personnummer.Valid(value, false), expected);
        }


        [Theory]
        [InlineData(198507099805, true)]
        [InlineData(198507099813, true)]
        [InlineData(8507099813, true)]
        [InlineData(196411139808, true)]
        [InlineData(8507099805, true)]
        [InlineData(19850709980, false)]
        [InlineData(19850709981, false)]
        [InlineData(19641113980, false)]
        public void TestPersonnummerInt(long value, bool expected)
        {

            Assert.Equal(Personnummer.Valid(value), expected);
        }

        [Theory]
        [InlineData("198507699802", true)]
        [InlineData("850769-9802", true)]
        [InlineData("198507699810", true)]
        [InlineData("850769-9810", true)]
        [InlineData("198567099805", false)]
        public void TestCoOrdinationNumbersString(string value, bool expected)
        {
            Assert.Equal(Personnummer.Valid(value), expected);
        }

        [Theory]
        [InlineData(198507699802, true)]
        [InlineData(8507699802, true)]
        [InlineData(198507699810, true)]
        [InlineData(8507699810, true)]
        [InlineData(198567099805, false)]
        public void TestCoOrdinationNumbersInt(long value, bool expected)
        {
            Assert.Equal(Personnummer.Valid(value), expected);
        }

        [Theory]
        [InlineData("198507099805", 8507099805)]
        [InlineData("198507099813", 8507099813)]
        [InlineData("850709-9813",  8507099813)]
        [InlineData("196411139808", 6411139808)]
        [InlineData("8507099805", 8507099805)]
        public void TestParseStringWithoutCentury(string value, long expected)
        {
            Assert.Equal(Personnummer.Format(value), expected);
        }

        [Theory]
        [InlineData("198507099805", 198507099805)]
        [InlineData("198507099813", 198507099813)]
        [InlineData("850709-9813", 198507099813)]
        [InlineData("196411139808", 196411139808)]
        [InlineData("8507099805", 198507099805)]
        public void TestParseStringWithCentury(string value, long expected)
        {
            Assert.Equal(Personnummer.Format(value, true), expected);
        }


        [Theory]
        [InlineData(8507099805, "850709-9805")]
        [InlineData(198507099813, "850709-9813")]
        [InlineData(8507099813, "850709-9813")]
        [InlineData(6411139808, "641113-9808")]
        [InlineData(198507099805, "850709-9805")]
        public void TestParseLongWithoutCentury(long value, string expected)
        {
            Assert.Equal(Personnummer.Format(value), expected);
        }

        [Theory]
        [InlineData(8507099805, "198507099805")]
        [InlineData(198507099813, "198507099813")]
        [InlineData(8507099813, "198507099813")]
        [InlineData(6411139808, "196411139808")]
        [InlineData(198507099805, "198507099805")]
        public void TestParseLongWithCentury(long value, string expected)
        {
            Assert.Equal(Personnummer.Format(value, true), expected);
        }

        [Theory]
        [InlineData("dfsafdsadfs")]
        [InlineData(6408933231)]
        [InlineData('a')]
        [InlineData(null)]
        [InlineData(123123)]
        public void TestParseInvalidThrows(dynamic value)
        {
            Assert.Throws<ValidationException>(() => Personnummer.Format(value));
        }


        [Theory]
        [InlineData("198507099805", true)]
        [InlineData("198507099813", true)]
        [InlineData("850709-9813", true)]
        [InlineData("196411139808", true)]
        [InlineData("8507099805", true)]
        [InlineData("198507699802", false)]
        [InlineData("850769-9802", false)]
        [InlineData("198507699810", false)]
        [InlineData("850769-9810", false)]
        public void TestNotAcceptingCoordinationNumbersString(string value, bool expected)
        {
            Assert.Equal(Personnummer.Valid(value, false), expected);
        }

        [Theory]
        [InlineData(198507099805, true)]
        [InlineData(198507099813, true)]
        [InlineData(8507099813, true)]
        [InlineData(196411139808, true)]
        [InlineData(8507099805, true)]
        [InlineData(198507699802, false)]
        [InlineData(8507699802, false)]
        [InlineData(198507699810, false)]
        [InlineData(8507699810, false)]
        public void TestNotAcceptingCoordinationNumbersLong(long value, bool expected)
        {
            Assert.Equal(Personnummer.Valid(value, false), expected);
        }

    }
}
