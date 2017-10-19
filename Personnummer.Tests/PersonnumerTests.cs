using System;
using Xunit;
using System.Collections.Generic;
using Xunit.Extensions;

namespace Personnummer.Tests
{
    public class PersonnumerTests
    {
        [Theory]
        [InlineData("510818-9167")]
        [InlineData("19900101-0017")]
        [InlineData("19130401+2931")]
        [InlineData("196408233234")]
        [InlineData("0001010107")]
        [InlineData("000101-0107")]
        public void TestPersonnummerWithControlDigitWithString(string value)
        {
            Assert.True(Personnummer.Valid(value));
        }


        [Theory]
        [InlineData(6403273813)]
        [InlineData(5108189167)]
        [InlineData(0001010107)]
        [InlineData(199001010007)]
        public void TestPersonnummerWithControlDigitWithInt(int value)
        {
            Assert.True(Personnummer.Valid(value));
        }

        [Theory]
        [InlineData("640327-381")]
        [InlineData("510818-916")]
        [InlineData("19900101-001")]
        [InlineData("100101+001")]
        public void TestPersonnummerWithoutControlDigitString(string value)
        {
            Assert.False(Personnummer.Valid(value));
        }


        [Theory]
        [InlineData(640327381)]
        [InlineData(510818916)]
        [InlineData(19900101001)]
        [InlineData(100101001)]
        public void TestPersonnummerWithoutControlDigitInt(int value)
        {
            Assert.False(Personnummer.Valid(value));
        }

        [Theory]
        [InlineData("701063-2391")]
        [InlineData("640883-3231")]
        public void TexstCoOrdinationNumbersString(string value)
        {
            Assert.True(Personnummer.Valid(value));
        }

        [Theory]
        [InlineData(7010632391)]
        [InlineData(6408833231)]
        public void TexstCoOrdinationNumbersInt(int value)
        {
            Assert.True(Personnummer.Valid(value));
        }

        [Theory]
        [InlineData("900161-0017")]
        [InlineData("640893-3231")]
        public void TestWrongCoOrdinationNumbersString(string value)
        {
            Assert.False(Personnummer.Valid(value));
        }

        [Theory]
        [InlineData(9001610017)]
        [InlineData(6408933231)]
        public void TestWrongCoOrdinationNumbersInt(int value)
        {
            Assert.False(Personnummer.Valid(value));
        }
    }
}
