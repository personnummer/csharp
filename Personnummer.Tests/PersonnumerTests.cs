using System.Diagnostics;
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
        [InlineData("070776-7604", true)]
        [InlineData("070797-7604", false)]
        [InlineData("070798-7604", false)]
        [InlineData("6102802424", false)]
        [InlineData("6102202425", false)]
        [InlineData("890302-4529", false)]
        [InlineData("890362-4528", false)]
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
        [InlineData("701063-2391", false)]
        [InlineData("640883-3231", false)]
        [InlineData("900161-0017", false)]
        [InlineData("6102802424", false)]
        [InlineData("510818-9167", true)]
        [InlineData("19900101-0017", true)]
        [InlineData("19130401+2931", true)]
        [InlineData("6102202425", false)]
        [InlineData("890302-4529", false)]
        [InlineData("890362-4528", false)]
        [InlineData("0001010107", true)]
        [InlineData("000101-0107", true)]
        [InlineData("640327-381", false)]
        [InlineData("510818-916", false)]
        public void TestNotAcceptingCoordinationNumbersString(string value, bool expected)
        {
            Assert.Equal(Personnummer.Valid(value, false), expected);
        }

        [Theory]
        [InlineData(7010632391, false)]
        [InlineData(6408833231, false)]
        [InlineData(9001610017, false)]
        [InlineData(6408933231, false)]
        [InlineData(6403273813, true)]
        [InlineData(5108189167, true)]
        [InlineData(199001010017, true)]
        [InlineData(640327381, false)]
        [InlineData(510818916, false)]
        public void TestNotAcceptingCoordinationNumbersLong(long value, bool expected)
        {
            Assert.Equal(Personnummer.Valid(value, false), expected);
        }

    }
}
