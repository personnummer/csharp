using System;
using System.Globalization;
using Personnummer.Exceptions;
using Xunit;

namespace Personnummer.Tests
{
    public class PersonnummerTests
    {
        [Theory]
        [ClassData(typeof(ValidSsnDataProvider))]
        public void TestCtr(PersonnummerData ssn)
        {
            Assert.IsType<Personnummer>(new Personnummer(ssn.LongFormat, new Personnummer.Options
            {
                AllowCoordinationNumber = false
            }));
            Assert.IsType<Personnummer>(new Personnummer(ssn.ShortFormat, new Personnummer.Options
            {
                AllowCoordinationNumber = false
            }));
            Assert.IsType<Personnummer>(new Personnummer(ssn.SeparatedFormat, new Personnummer.Options
            {
                AllowCoordinationNumber = false
            }));
            Assert.IsType<Personnummer>(new Personnummer(ssn.SeparatedLong, new Personnummer.Options
            {
                AllowCoordinationNumber = false
            }));
        }

        [Theory]
        [ClassData(typeof(ValidSsnDataProvider))]
        public void TestParse(PersonnummerData ssn)
        {
            Assert.IsType<Personnummer>(Personnummer.Parse(ssn.LongFormat, new Personnummer.Options
            {
                AllowCoordinationNumber = false
            }));
            Assert.IsType<Personnummer>(Personnummer.Parse(ssn.ShortFormat, new Personnummer.Options
            {
                AllowCoordinationNumber = false
            }));
            Assert.IsType<Personnummer>(Personnummer.Parse(ssn.SeparatedFormat, new Personnummer.Options
            {
                AllowCoordinationNumber = false
            }));
            Assert.IsType<Personnummer>(Personnummer.Parse(ssn.SeparatedLong, new Personnummer.Options
            {
                AllowCoordinationNumber = false
            }));
        }

        [Theory]
        [ClassData(typeof(InvalidSsnDataProvider))]
        [ClassData(typeof(ValidCnDataProvider))]
        [ClassData(typeof(InvalidCnDataProvider))]
        public void TestParseInvalid(PersonnummerData ssn)
        {
            Assert.Throws<PersonnummerException>(() => Personnummer.Parse(ssn.LongFormat, new Personnummer.Options
            {
                AllowCoordinationNumber = false
            }));
            Assert.Throws<PersonnummerException>(() => Personnummer.Parse(ssn.ShortFormat, new Personnummer.Options
            {
                AllowCoordinationNumber = false
            }));
            Assert.Throws<PersonnummerException>(() => Personnummer.Parse(ssn.SeparatedFormat, new Personnummer.Options
            {
                AllowCoordinationNumber = false
            }));
            Assert.Throws<PersonnummerException>(() => Personnummer.Parse(ssn.SeparatedLong, new Personnummer.Options
            {
                AllowCoordinationNumber = false
            }));
        }


        [Theory]
        [ClassData(typeof(ValidSsnDataProvider))]
        public void TestAge(PersonnummerData ssn)
        {
            var clock = new TestClock();
            var localNow = clock.GetNow();
            DateTime dt = DateTime.ParseExact(ssn.LongFormat.Substring(0, ssn.LongFormat.Length - 4), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);

            var years = localNow.Year - dt.Year;
            if (!(localNow.Month > dt.Month || (localNow.Month == dt.Month && localNow.Day >= dt.Day)))
            {
                years--;
            }

            Assert.Equal(years, Personnummer.Parse(ssn.SeparatedLong, new Personnummer.Options { AllowCoordinationNumber = false, Now = clock.GetNow }).Age);
            Assert.Equal(years, Personnummer.Parse(ssn.SeparatedFormat, new Personnummer.Options { AllowCoordinationNumber = false, Now = clock.GetNow }).Age);
            Assert.Equal(years, Personnummer.Parse(ssn.LongFormat, new Personnummer.Options { AllowCoordinationNumber = false, Now = clock.GetNow }).Age);
            // Du to age not being possible to fetch from >100 year short format without separator, we aught to check this here.
            Assert.Equal(years > 99 ? years - 100 : years, Personnummer.Parse(ssn.ShortFormat, new Personnummer.Options { AllowCoordinationNumber = false, Now = clock.GetNow }).Age);
        }

        [Fact]
        public void TestEdgeCasesAroundBirthday()
        {
            var clock = new TestClock
            {
                Now = DateTimeOffset.Parse("2090-01-09 12:00")
            };

            Assert.Equal(10, new Personnummer("20800108-6670", new Personnummer.Options() {Now = clock.GetNow} ).Age); // Had birthday yesterday
            Assert.Equal(10, new Personnummer("20800109-8287", new Personnummer.Options() {Now = clock.GetNow} ).Age); // Birthday today
            Assert.Equal(9, new Personnummer("20800110-8516", new Personnummer.Options() {Now = clock.GetNow} ).Age); // Upcoming Birthday tomorrow
        }

        [Theory]
        [ClassData(typeof(ValidSsnDataProvider))]
        public void TestFormat(PersonnummerData ssn)
        {
            Assert.Equal(ssn.SeparatedFormat, Personnummer.Parse(ssn.SeparatedLong, new Personnummer.Options { AllowCoordinationNumber = true }).Format());
            Assert.Equal(ssn.SeparatedFormat, Personnummer.Parse(ssn.SeparatedFormat, new Personnummer.Options { AllowCoordinationNumber = true }).Format());
            Assert.Equal(ssn.SeparatedFormat, Personnummer.Parse(ssn.LongFormat, new Personnummer.Options { AllowCoordinationNumber = true }).Format());
            // Short format will always guess that it's latest century.
            Assert.Equal(ssn.SeparatedFormat.Replace("+", "-"), Personnummer.Parse(ssn.ShortFormat, new Personnummer.Options { AllowCoordinationNumber = true }).Format());
        }

        [Theory]
        [ClassData(typeof(ValidSsnDataProvider))]
        [ClassData(typeof(ValidSsnDataProvider))]
        public void TestFormatLong(PersonnummerData ssn)
        {
            Assert.Equal(ssn.SeparatedLong, Personnummer.Parse(ssn.SeparatedLong, new Personnummer.Options { AllowCoordinationNumber = true }).Format(true));
            Assert.Equal(ssn.SeparatedLong, Personnummer.Parse(ssn.SeparatedFormat, new Personnummer.Options { AllowCoordinationNumber = true }).Format(true));
            Assert.Equal(ssn.SeparatedLong, Personnummer.Parse(ssn.LongFormat, new Personnummer.Options { AllowCoordinationNumber = true }).Format(true));
        }

        [Theory]
        [ClassData(typeof(InvalidSsnDataProvider))]
        public void TestValidFail(PersonnummerData ssn)
        {
            Assert.False(Personnummer.Valid(ssn.LongFormat));
            Assert.False(Personnummer.Valid(ssn.SeparatedLong));
            Assert.False(Personnummer.Valid(ssn.SeparatedFormat));
            Assert.False(Personnummer.Valid(ssn.ShortFormat));
        }

        [Theory]
        [ClassData(typeof(ValidSsnDataProvider))]
        public void TestValid(PersonnummerData ssn)
        {
            Assert.True(Personnummer.Valid(ssn.LongFormat));
            Assert.True(Personnummer.Valid(ssn.SeparatedLong));
            Assert.True(Personnummer.Valid(ssn.SeparatedFormat));
            Assert.True(Personnummer.Valid(ssn.ShortFormat));
        }

        [Theory]
        [ClassData(typeof(ValidSsnDataProvider))]
        public void TestMaleFemale(PersonnummerData ssn)
        {
            Assert.Equal(ssn.IsMale, Personnummer.Parse(ssn.LongFormat, new Personnummer.Options { AllowCoordinationNumber = true }).IsMale);
            Assert.Equal(ssn.IsMale, Personnummer.Parse(ssn.SeparatedLong, new Personnummer.Options { AllowCoordinationNumber = true }).IsMale);
            Assert.Equal(ssn.IsMale, Personnummer.Parse(ssn.SeparatedFormat, new Personnummer.Options { AllowCoordinationNumber = true }).IsMale);
            Assert.Equal(ssn.IsMale, Personnummer.Parse(ssn.ShortFormat, new Personnummer.Options { AllowCoordinationNumber = true }).IsMale);

            Assert.Equal(ssn.IsFemale, Personnummer.Parse(ssn.LongFormat, new Personnummer.Options { AllowCoordinationNumber = true }).IsFemale);
            Assert.Equal(ssn.IsFemale, Personnummer.Parse(ssn.SeparatedLong, new Personnummer.Options { AllowCoordinationNumber = true }).IsFemale);
            Assert.Equal(ssn.IsFemale, Personnummer.Parse(ssn.SeparatedFormat, new Personnummer.Options { AllowCoordinationNumber = true }).IsFemale);
            Assert.Equal(ssn.IsFemale, Personnummer.Parse(ssn.ShortFormat, new Personnummer.Options { AllowCoordinationNumber = true }).IsFemale);
        }

        [Theory]
        [ClassData(typeof(ValidSsnDataProvider))]
        public void TestSeparator(PersonnummerData ssn)
        {
            string sep = ssn.SeparatedFormat.Contains('+') ? "+" : "-";
            Assert.Equal(sep, Personnummer.Parse(ssn.LongFormat, new Personnummer.Options { AllowCoordinationNumber = true }).Separator);
            Assert.Equal(sep, Personnummer.Parse(ssn.SeparatedLong, new Personnummer.Options { AllowCoordinationNumber = true }).Separator);
            Assert.Equal(sep, Personnummer.Parse(ssn.SeparatedFormat, new Personnummer.Options { AllowCoordinationNumber = true }).Separator);
            // Getting the separator from a short formatted none-separated person number is not actually possible if it is intended to be a +.
        }

        [Theory]
        [ClassData(typeof(ValidSsnDataProvider))]
        public void TestIgnoreSeparatorWhenFormatting(PersonnummerData ssn)
        {
            var separators = "+-".ToCharArray();

            var ps1 = Personnummer.Parse(ssn.LongFormat, new Personnummer.Options { AllowCoordinationNumber = true });
            var ps2 = Personnummer.Parse(ssn.SeparatedLong, new Personnummer.Options { AllowCoordinationNumber = true });
            var ps3 = Personnummer.Parse(ssn.SeparatedFormat,
                new Personnummer.Options { AllowCoordinationNumber = true });

            Assert.True(ps1.Format(false, true).IndexOfAny(separators) == -1);
            Assert.True(ps2.Format(false, true).IndexOfAny(separators) == -1);
            Assert.True(ps3.Format(false, true).IndexOfAny(separators) == -1);

            Assert.True(ps1.Format().IndexOfAny(separators) >= 0);
            Assert.True(ps2.Format().IndexOfAny(separators) >= 0);
            Assert.True(ps3.Format().IndexOfAny(separators) >= 0);
        }

        [Theory]
        [ClassData(typeof(OrgNumberDataProvider))]
        public void TestOrgNumber(PersonnummerData orgnr)
        {
            Assert.Throws<PersonnummerException>(() => Personnummer.Parse(orgnr.SeparatedFormat, new Personnummer.Options
            {
                AllowCoordinationNumber = true
            }));

            Assert.Throws<PersonnummerException>(() => Personnummer.Parse(orgnr.SeparatedFormat, new Personnummer.Options
            {
                AllowCoordinationNumber = false
            }));

            Assert.Throws<PersonnummerException>(() => Personnummer.Parse(orgnr.ShortFormat, new Personnummer.Options
            {
                AllowCoordinationNumber = true
            }));

            Assert.Throws<PersonnummerException>(() => Personnummer.Parse(orgnr.ShortFormat, new Personnummer.Options
            {
                AllowCoordinationNumber = false
            }));
        }

        [Theory]
        [ClassData(typeof(ValidSsnDataProvider))]
        public void TestDate(PersonnummerData data)
        {
            var pn = new Personnummer(data.LongFormat);
            Assert.Equal(data.LongFormat.Substring(0, 8), $"{pn.Date.Year:0000}{pn.Date.Month:00}{pn.Date.Day:00}");
        }

        [Fact]
        public void TestParseTooLong()
        {
            Assert.Equal(
                "Input string too long",
                Assert.Throws<PersonnummerException>(() =>
                {
                    Personnummer.Parse("11111111-11111"); // 14 greater than 13.
                }).Message
            );
        }

        [Fact]
        public void TestParseTooShort()
        {
            Assert.Equal(
                "Input string too short",
                Assert.Throws<PersonnummerException>(() =>
                {
                    Personnummer.Parse("111111111"); // 9 less than 10
                }).Message
            );
        }
    }
}
