using Personnummer.Exceptions;
using Xunit;

namespace Personnummer.Tests
{
    public class InterimNumberTests
    {
        private Personnummer.Options _opts = new Personnummer.Options()
        {
            AllowInterimNumber = true,
        };

        [Theory]
        [ClassData(typeof(ValidInterimProvider))]
        public void TestValidateInterim(PersonnummerData ssn)
        {
            Assert.True(Personnummer.Valid(ssn.LongFormat, _opts));
            Assert.True(Personnummer.Valid(ssn.ShortFormat, _opts));
        }

        [Theory]
        [ClassData(typeof(InvalidInterimProvider))]
        public void TestValidateInvalidInterim(PersonnummerData ssn)
        {
            Assert.False(Personnummer.Valid(ssn.LongFormat, _opts));
            Assert.False(Personnummer.Valid(ssn.ShortFormat, _opts));
        }

        [Theory]
        [ClassData(typeof(ValidInterimProvider))]
        public void TestFormatLongInterim(PersonnummerData ssn)
        {
            var pnr = Personnummer.Parse(ssn.LongFormat, _opts);
            Assert.Equal(pnr.Format(false, false), ssn.SeparatedFormat);
            Assert.Equal(pnr.Format(true, true), ssn.LongFormat);
            Assert.Equal(pnr.Format(true, false), ssn.SeparatedLong);
            Assert.Equal(pnr.Format(false, true), ssn.ShortFormat);
        }

        [Theory]
        [ClassData(typeof(ValidInterimProvider))]
        public void TestFormatShortInterim(PersonnummerData ssn)
        {
            var pnr = Personnummer.Parse(ssn.ShortFormat, _opts);
            Assert.Equal(pnr.Format(false, false), ssn.SeparatedFormat);
            Assert.Equal(pnr.Format(true, true), ssn.LongFormat);
            Assert.Equal(pnr.Format(true, false), ssn.SeparatedLong);
            Assert.Equal(pnr.Format(false, true), ssn.ShortFormat);
        }

        [Theory]
        [ClassData(typeof(InvalidInterimProvider))]
        public void TestInvalidInterimThrows(PersonnummerData ssn)
        {
            Assert.Throws<PersonnummerException>(() => Personnummer.Parse(ssn.LongFormat, _opts));
            Assert.Throws<PersonnummerException>(() => Personnummer.Parse(ssn.ShortFormat, _opts));
            Assert.Throws<PersonnummerException>(() => Personnummer.Parse(ssn.SeparatedLong, _opts));
            Assert.Throws<PersonnummerException>(() => Personnummer.Parse(ssn.SeparatedFormat, _opts));
        }

        [Theory]
        [ClassData(typeof(ValidInterimProvider))]
        public void TestInterimThrowsIfNotActive(PersonnummerData ssn)
        {
            Assert.Throws<PersonnummerException>(() => Personnummer.Parse(ssn.LongFormat));
            Assert.Throws<PersonnummerException>(() => Personnummer.Parse(ssn.ShortFormat));
            Assert.Throws<PersonnummerException>(() => Personnummer.Parse(ssn.SeparatedLong));
            Assert.Throws<PersonnummerException>(() => Personnummer.Parse(ssn.SeparatedFormat));
        }
    }
}
