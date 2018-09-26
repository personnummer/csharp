using System;
using Xunit;

namespace PersonnummerTests
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
        public void TestPersonnummerString(string value, bool expected)
        {
            Assert.Equal(Personnummer.Personnummer.Valid(value), expected);
        }


        [Theory]
        [InlineData(6403273813, true)]
        [InlineData(5108189167, true)]
        [InlineData(199001010017, true)]
        [InlineData(640327381, false)]
        [InlineData(510818916, false)]
        [InlineData(19900101001, false)]
        [InlineData(100101001, false)]
        public void TestPersonnummerInt(long value, bool expected)
        {

            Assert.Equal(Personnummer.Personnummer.Valid(value), expected);
        }

        [Theory]
        [InlineData("701063-2391", true)]
        [InlineData("640883-3231", true)]
        [InlineData("900161-0017", false)]
        [InlineData("640893-3231", false)]
        public void TexstCoOrdinationNumbersString(string value, bool expected)
        {
            Assert.Equal(Personnummer.Personnummer.Valid(value), expected);
        }

        [Theory]
        [InlineData(7010632391, true)]
        [InlineData(6408833231, true)]
        [InlineData(9001610017, false)]
        [InlineData(6408933231, false)]
        public void TexstCoOrdinationNumbersInt(long value, bool expected)
        {
            Assert.Equal(Personnummer.Personnummer.Valid(value), expected);
        }
    }
  
}
