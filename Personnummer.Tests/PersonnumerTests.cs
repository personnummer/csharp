using System.Diagnostics;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace Personnummer.Tests
{
    public class PersonnumerTests
    {

        [Theory]
        [InlineData("510818-9167", true)]
        [InlineData("19900101-0017", true)]
        [InlineData("19130401+2931", true)]
        [InlineData("196408233234", true)]
        [InlineData("0001010107", true)]
        [InlineData("000101-0107", true)]
        [InlineData("640327-381", false)]
        [InlineData("510818-916", false)]
        [InlineData("19900101-001", false)]
        [InlineData("100101+001", false)]
        [InlineData("550207-3900", true)]
        [InlineData("9701063-2391", false)]
        public void TestPersonnummerString(string value, bool expected)
        {
            Assert.Equal(Personnummer.Valid(value), expected);
        }


        [Theory]
        [InlineData(6403273813, true)]
        [InlineData(5108189167, true)]
        [InlineData(199001010017, true)]
        [InlineData(640327381, false)]
        [InlineData(510818916, false)]
        [InlineData(19900101001, false)]
        [InlineData(100101001, false)]
        [InlineData(5502073900, true)]
        public void TestPersonnummerInt(long value, bool expected)
        {
            
            Assert.Equal(Personnummer.Valid(value), expected);
        }
        
        [Theory]
        [InlineData("701063-2391", true)]
        [InlineData("640883-3231", true)]
        [InlineData("900161-0017", false)]
        [InlineData("640893-3231", false)]
        [InlineData("550207-3900", true)]
        public void TexstCoOrdinationNumbersString(string value, bool expected)
        {
            Assert.Equal(Personnummer.Valid(value), expected);
        }

        [Theory]
        [InlineData(7010632391, true)]
        [InlineData(6408833231, true)]
        [InlineData(9001610017, false)]
        [InlineData(6408933231, false)]
        [InlineData(5502073900, true)]
        public void TexstCoOrdinationNumbersInt(long value, bool expected)
        {
            Assert.Equal(Personnummer.Valid(value), expected);
        }

        [Theory]
        [InlineData("130401+2931", 1304012931)]
        [InlineData("701063-2391", 7010632391)]
        [InlineData("640883-3231", 6408833231)]
        [InlineData("550207-3900", 5502073900)]
        [InlineData("6408833231", 6408833231)]
        public void TestParseStringWithoutCentury(string value, long expected)
        {
            Assert.Equal(Personnummer.Format(value), expected);
        }

        [Theory]
        [InlineData("187010632391", 187010632391)]
        [InlineData("197010632391", 197010632391)]
        [InlineData("196408833231", 196408833231)]
        [InlineData("6408833231", 196408833231)]
        [InlineData("0001010107", 200001010107)]
        [InlineData("000101-0107", 200001010107)]
        [InlineData("195502073900", 195502073900)]
        public void TestParseStringWithCentury(string value, long expected)
        {
            Assert.Equal(Personnummer.Format(value, true), expected);
        }


        [Theory]
        [InlineData(191304012931, "130401+2931")]
        [InlineData(7010632391, "701063-2391")]
        [InlineData(6408833231, "640883-3231")]
        [InlineData(5502073900, "550207-3900")]
        public void TestParseLongWithoutCentury(long value, string expected)
        {
            Assert.Equal(Personnummer.Format(value), expected);
        }

        [Theory]
        [InlineData(187010632391, "187010632391")]
        [InlineData(197010632391, "197010632391")]
        [InlineData(196408833231, "196408833231")]
        [InlineData(195502073900, "195502073900")]
        public void TestParseLongWithCentury(long value, string expected)
        {
            Assert.Equal(Personnummer.Format(value, true), expected);
        }

        [Theory]
        [InlineData("dfsafdsadfs")]
        [InlineData(6408933231)]
        [InlineData('a')]
        [InlineData(123123)]
        public void TestParseInvalidThrows(dynamic value)
        {
            Assert.Throws<ValidationException>(() => Personnummer.Format(value));
        }

    }
}
