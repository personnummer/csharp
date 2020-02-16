using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace Personnummer.Tests
{
    public class PersonnumerTestsOld
    {
        [Theory]
        [ClassData(typeof(SsnDataProvider))]
        public void TestValid(PersonnummerData data)
        {
            Assert.Equal(PersonnummerOld.Valid(data.ShortFormat, false), data.Valid);
            Assert.Equal(PersonnummerOld.Valid(data.LongFormat, false), data.Valid);
            Assert.Equal(PersonnummerOld.Valid(data.SeparatedFormat, false), data.Valid);
            Assert.Equal(PersonnummerOld.Valid(data.SeparatedLong, false), data.Valid);
            Assert.Equal(PersonnummerOld.Valid(data.Integer, false), data.Valid);
        }

        [Theory]
        [ClassData(typeof(CnDataProvider))]
        public void TestValidCoOrdinationNumbers(PersonnummerData data)
        {
            Assert.Equal(PersonnummerOld.Valid(data.ShortFormat), data.Valid);
            Assert.Equal(PersonnummerOld.Valid(data.LongFormat), data.Valid);
            Assert.Equal(PersonnummerOld.Valid(data.SeparatedFormat), data.Valid);
            Assert.Equal(PersonnummerOld.Valid(data.SeparatedLong), data.Valid);
            Assert.Equal(PersonnummerOld.Valid(data.Integer), data.Valid);
        }

        [Theory]
        [InlineData("dfsafdsadfs")]
        [InlineData(6408933231)]
        [InlineData('a')]
        [InlineData(null)]
        [InlineData(123123)]
        public void TestParseInvalidThrows(dynamic value)
        {
            Assert.Throws<ValidationException>(() => PersonnummerOld.Format(value));
        }


        [Theory]
        [ClassData(typeof(CnDataProvider))]
        public void TestNotAcceptingCoordinationNumbers(PersonnummerData data)
        {
            Assert.False(PersonnummerOld.Valid(data.Integer, false));
            Assert.False(PersonnummerOld.Valid(data.LongFormat, false));
            Assert.False(PersonnummerOld.Valid(data.SeparatedFormat, false));
            Assert.False(PersonnummerOld.Valid(data.SeparatedLong, false));
            Assert.False(PersonnummerOld.Valid(data.ShortFormat, false));
        }


        [Theory]
        [ClassData(typeof(DataProvider))]
        public void TestFormatInteger(PersonnummerData data)
        {
            if (data.Valid)
            {
                Assert.Equal(data.SeparatedFormat, PersonnummerOld.Format(data.Integer, false));
                Assert.Equal(data.LongFormat, PersonnummerOld.Format(data.Integer, true));
            }
            else
            {
                Assert.Throws<ValidationException>(() => PersonnummerOld.Format(data.Integer));
            }
        }
    }
}
