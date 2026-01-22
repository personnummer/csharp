using System;
using System.Globalization;
using Personnummer.Exceptions;
using Xunit;

namespace Personnummer.Tests
{
    public class CoordinationNumberTests
    {

        [Theory]
        [ClassData(typeof(ValidCnDataProvider))]
        public void TestRealDay(PersonnummerData ssn)
        {
            var day = int.Parse(ssn.ShortFormat.Substring(4, 2)) - 60;
            Assert.Equal(
                $"{day}",
                new Personnummer(ssn.LongFormat, new Personnummer.Options() { AllowCoordinationNumber = true }).RealDay
            );
            Assert.Equal(
                $"{day}",
                new Personnummer(ssn.ShortFormat, new Personnummer.Options() { AllowCoordinationNumber = true }).RealDay
            );
            Assert.Equal(
                $"{day}",
                new Personnummer(ssn.SeparatedFormat, new Personnummer.Options() { AllowCoordinationNumber = true }).RealDay
            );
            Assert.Equal(
                $"{day}",
                new Personnummer(ssn.SeparatedLong, new Personnummer.Options() { AllowCoordinationNumber = true }).RealDay
            );
        }

        [Theory]
        [ClassData(typeof(ValidCnDataProvider))]
        public void TestCtrCn(PersonnummerData ssn)
        {
            Assert.IsType<Personnummer>(new Personnummer(ssn.LongFormat, new Personnummer.Options
            {
                AllowCoordinationNumber = true
            }));
            Assert.IsType<Personnummer>(new Personnummer(ssn.ShortFormat, new Personnummer.Options
            {
                AllowCoordinationNumber = true
            }));
            Assert.IsType<Personnummer>(new Personnummer(ssn.SeparatedFormat, new Personnummer.Options
            {
                AllowCoordinationNumber = true
            }));
            Assert.IsType<Personnummer>(new Personnummer(ssn.SeparatedLong, new Personnummer.Options
            {
                AllowCoordinationNumber = true
            }));
        }


        [Theory]
        [ClassData(typeof(InvalidSsnDataProvider))]
        [ClassData(typeof(ValidCnDataProvider))]
        [ClassData(typeof(InvalidCnDataProvider))]
        public void TestCtrInvalid(PersonnummerData ssn)
        {
            Assert.Throws<PersonnummerException>(() => new Personnummer(ssn.LongFormat, new Personnummer.Options
            {
                AllowCoordinationNumber = false
            }));
            Assert.Throws<PersonnummerException>(() => new Personnummer(ssn.ShortFormat, new Personnummer.Options
            {
                AllowCoordinationNumber = false
            }));
            Assert.Throws<PersonnummerException>(() => new Personnummer(ssn.SeparatedFormat, new Personnummer.Options
            {
                AllowCoordinationNumber = false
            }));
            Assert.Throws<PersonnummerException>(() => new Personnummer(ssn.SeparatedLong, new Personnummer.Options
            {
                AllowCoordinationNumber = false
            }));
        }

        [Theory]
        [ClassData(typeof(ValidCnDataProvider))]
        public void TestParseCn(PersonnummerData ssn)
        {
            Assert.IsType<Personnummer>(Personnummer.Parse(ssn.LongFormat, new Personnummer.Options
            {
                AllowCoordinationNumber = true
            }));
            Assert.IsType<Personnummer>(Personnummer.Parse(ssn.ShortFormat, new Personnummer.Options
            {
                AllowCoordinationNumber = true
            }));
            Assert.IsType<Personnummer>(Personnummer.Parse(ssn.SeparatedFormat, new Personnummer.Options
            {
                AllowCoordinationNumber = true
            }));
            Assert.IsType<Personnummer>(Personnummer.Parse(ssn.SeparatedLong, new Personnummer.Options
            {
                AllowCoordinationNumber = true
            }));
        }

        [Theory]
        [ClassData(typeof(InvalidCnDataProvider))]
        public void TestParseInvalidCn(PersonnummerData ssn)
        {
            Assert.Throws<PersonnummerException>(() => Personnummer.Parse(ssn.LongFormat, new Personnummer.Options
            {
                AllowCoordinationNumber = true
            }));
            Assert.Throws<PersonnummerException>(() => Personnummer.Parse(ssn.ShortFormat, new Personnummer.Options
            {
                AllowCoordinationNumber = true
            }));
            Assert.Throws<PersonnummerException>(() => Personnummer.Parse(ssn.SeparatedFormat, new Personnummer.Options
            {
                AllowCoordinationNumber = true
            }));
            Assert.Throws<PersonnummerException>(() => Personnummer.Parse(ssn.SeparatedLong, new Personnummer.Options
            {
                AllowCoordinationNumber = true
            }));
        }

        [Theory]
        [ClassData(typeof(ValidCnDataProvider))]
        public void TestAgeCn(PersonnummerData ssn)
        {
            var timeProvider = new TestTimeProvider();
            var localNow = timeProvider.GetLocalNow();

            int day = int.Parse(ssn.LongFormat.Substring(ssn.LongFormat.Length - 6, 2)) - 60;
            string strDay = day < 10 ? $"0{day}" : day.ToString();

            DateTime dt = DateTime.ParseExact(ssn.LongFormat.Substring(0, ssn.LongFormat.Length - 6) + strDay, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
            var years = localNow.Year - dt.Year;
            if (!(localNow.Month > dt.Month || (localNow.Month == dt.Month && localNow.Day >= dt.Day)))
            {
                years--;
            }

            Assert.Equal(years, Personnummer.Parse(ssn.SeparatedLong, new Personnummer.Options { AllowCoordinationNumber = true, TimeProvider = timeProvider }).Age);
            Assert.Equal(years, Personnummer.Parse(ssn.SeparatedFormat, new Personnummer.Options { AllowCoordinationNumber = true, TimeProvider = timeProvider }).Age);
            Assert.Equal(years, Personnummer.Parse(ssn.LongFormat, new Personnummer.Options { AllowCoordinationNumber = true, TimeProvider = timeProvider }).Age);
            // Du to age not being possible to fetch from >100 year short format without separator, we aught to check this here.
            Assert.Equal(years > 99 ? years - 100 : years, Personnummer.Parse(ssn.ShortFormat, new Personnummer.Options { AllowCoordinationNumber = true, TimeProvider = timeProvider }).Age);
        }

        [Theory]
        [ClassData(typeof(ValidCnDataProvider))]
        public void TestFormat(PersonnummerData ssn)
        {
            Assert.Equal(ssn.SeparatedFormat, Personnummer.Parse(ssn.SeparatedLong, new Personnummer.Options { AllowCoordinationNumber = true }).Format());
            Assert.Equal(ssn.SeparatedFormat, Personnummer.Parse(ssn.SeparatedFormat, new Personnummer.Options { AllowCoordinationNumber = true }).Format());
            Assert.Equal(ssn.SeparatedFormat, Personnummer.Parse(ssn.LongFormat, new Personnummer.Options { AllowCoordinationNumber = true }).Format());
            // Short format will always guess that it's latest century.
            Assert.Equal(ssn.SeparatedFormat.Replace("+", "-"), Personnummer.Parse(ssn.ShortFormat, new Personnummer.Options { AllowCoordinationNumber = true }).Format());
        }

        [Theory]
        [ClassData(typeof(InvalidCnDataProvider))]
        public void TestValidFail(PersonnummerData ssn)
        {
            Assert.False(Personnummer.Valid(ssn.LongFormat));
            Assert.False(Personnummer.Valid(ssn.SeparatedLong));
            Assert.False(Personnummer.Valid(ssn.SeparatedFormat));
            Assert.False(Personnummer.Valid(ssn.ShortFormat));
        }

        [Theory]
        [ClassData(typeof(ValidCnDataProvider))]
        public void TestValid(PersonnummerData ssn)
        {
            Assert.True(Personnummer.Valid(ssn.LongFormat));
            Assert.True(Personnummer.Valid(ssn.SeparatedLong));
            Assert.True(Personnummer.Valid(ssn.SeparatedFormat));
            Assert.True(Personnummer.Valid(ssn.ShortFormat));
        }

        [Theory]
        [ClassData(typeof(ValidCnDataProvider))]
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
        [ClassData(typeof(ValidCnDataProvider))]
        public void TestSeparator(PersonnummerData ssn)
        {
            string sep = ssn.SeparatedFormat.Contains('+') ? "+" : "-";
            Assert.Equal(sep, Personnummer.Parse(ssn.LongFormat, new Personnummer.Options { AllowCoordinationNumber = true }).Separator);
            Assert.Equal(sep, Personnummer.Parse(ssn.SeparatedLong, new Personnummer.Options { AllowCoordinationNumber = true }).Separator);
            Assert.Equal(sep, Personnummer.Parse(ssn.SeparatedFormat, new Personnummer.Options { AllowCoordinationNumber = true }).Separator);
            // Getting the separator from a short formatted none-separated person number is not actually possible if it is intended to be a +.
        }

        [Theory]
        [ClassData(typeof(ValidCnDataProvider))]
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
        [ClassData(typeof(ValidCnDataProvider))]
        public void TestDateCn(PersonnummerData data)
        {
            var expect = data.LongFormat[..4];
            expect += data.LongFormat.Substring(4, 2);
            expect += (int.Parse(data.LongFormat.Substring(6, 2)) - 60).ToString("00");

            var pn = new Personnummer(data.LongFormat);
            Assert.Equal(expect, $"{pn.Date.Year:0000}{pn.Date.Month:00}{pn.Date.Day:00}");
        }
    }
}
