using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace Personnummer.Tests
{
    public class PersonnumerTests
    {
        [Theory]
        [ClassData(typeof(PersonnummerDataProvider))]
        public void TestValid(PersonnummerData data)
        {
            Assert.Equal(Personnummer.Valid(data.ShortFormat, false), data.Valid);
            Assert.Equal(Personnummer.Valid(data.LongFormat, false), data.Valid);
            Assert.Equal(Personnummer.Valid(data.SeparatedFormat, false), data.Valid);
            Assert.Equal(Personnummer.Valid(data.SeparatedLong, false), data.Valid);
            Assert.Equal(Personnummer.Valid(data.Integer, false), data.Valid);
        }

        [Theory]
        [ClassData(typeof(CoordinationNumberProvider))]
        public void TestValidCoOrdinationNumbers(PersonnummerData data)
        {
            Assert.Equal(Personnummer.Valid(data.ShortFormat), data.Valid);
            Assert.Equal(Personnummer.Valid(data.LongFormat), data.Valid);
            Assert.Equal(Personnummer.Valid(data.SeparatedFormat), data.Valid);
            Assert.Equal(Personnummer.Valid(data.SeparatedLong), data.Valid);
            Assert.Equal(Personnummer.Valid(data.Integer), data.Valid);
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
        [ClassData(typeof(CoordinationNumberProvider))]
        public void TestNotAcceptingCoordinationNumbers(PersonnummerData data)
        {
            Assert.False(Personnummer.Valid(data.Integer, false));
            Assert.False(Personnummer.Valid(data.LongFormat, false));
            Assert.False(Personnummer.Valid(data.SeparatedFormat, false));
            Assert.False(Personnummer.Valid(data.SeparatedLong, false));
            Assert.False(Personnummer.Valid(data.ShortFormat, false));
        }


        [Theory]
        [ClassData(typeof(DataProvider))]
        public void TestFormatInteger(PersonnummerData data)
        {
            if (data.Valid)
            {
                Assert.Equal(data.SeparatedFormat, Personnummer.Format(data.Integer, false));
                Assert.Equal(data.LongFormat, Personnummer.Format(data.Integer, true));
            }
            else
            {
                Assert.Throws<ValidationException>(() => Personnummer.Format(data.Integer));
            }
        }
    }
}
