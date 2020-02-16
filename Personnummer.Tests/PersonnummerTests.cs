using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Personnummer.Tests
{
    public class PersonnummerTests
    {
        [Theory]
        [ClassData(typeof(ValidSsnDataProvider))]
        public void TestParse(PersonnummerData ssn) { }

        [Theory]
        [ClassData(typeof(ValidCnDataProvider))]
        public void TestParseCn(PersonnummerData ssn) { }

        [Theory]
        [ClassData(typeof(InvalidSsnDataProvider))]
        public void TestParseInvalid(PersonnummerData ssn) { }

        [Theory]
        [ClassData(typeof(InvalidCnDataProvider))]
        public void TestParseInvalidCn(PersonnummerData ssn) { }

        [Theory]
        [ClassData(typeof(ValidSsnDataProvider))]
        public void TestAge(PersonnummerData ssn) { }
        
        public void TestAgeCn(PersonnummerData ssn) { }

        [Theory]
        [ClassData(typeof(ValidSsnDataProvider))]
        public void TestFormat(PersonnummerData ssn) { }

        [Theory]
        [ClassData(typeof(ValidSsnDataProvider))]

        public void TestFormatLong(PersonnummerData ssn) { }

        [Theory]
        [ClassData(typeof(ValidCnDataProvider))]
        public void TestFormatCn(PersonnummerData ssn) { }

        [Theory]
        [ClassData(typeof(ValidCnDataProvider))]
        public void TestFormatCnLong(PersonnummerData ssn) { }

        [Theory]
        [ClassData(typeof(ValidSsnDataProvider))]
        [ClassData(typeof(ValidCnDataProvider))]
        public void TestFormatInvalid(PersonnummerData ssn) { }

        [Theory]
        [ClassData(typeof(ValidSsnDataProvider))]
        [ClassData(typeof(ValidCnDataProvider))]
        public void TestSeparator(PersonnummerData ssn) { }
    }
}
