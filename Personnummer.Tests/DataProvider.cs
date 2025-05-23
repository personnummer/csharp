using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Personnummer.Tests
{
    public struct PersonnummerData
    {
        public long Integer { get; set; }
        [JsonPropertyName ("long_format")]
        public string LongFormat { get; set; }
        [JsonPropertyName ("short_format")]
        public string ShortFormat { get; set; }
        [JsonPropertyName ("separated_format")]
        public string SeparatedFormat { get; set; }
        [JsonPropertyName ("separated_long")]
        public string SeparatedLong { get; set; }
        [JsonPropertyName ("valid")]
        public bool Valid { get; set; }
        [JsonPropertyName ("type")]
        public string Type { get; set; }
        [JsonPropertyName ("isMale")]
        public bool IsMale { get; set; }
        [JsonPropertyName("isFemale")]
        public bool IsFemale { get; set; }
    }

    public class InterimDataProvider : IEnumerable<object[]>
    {
        protected static List<PersonnummerData> Data { get; }

        static InterimDataProvider()
        {
            var webClient = new HttpClient();;
            var response = webClient.GetStringAsync("https://raw.githubusercontent.com/personnummer/meta/master/testdata/interim.json").Result;
            Data = JsonSerializer.Deserialize<List<PersonnummerData>>(response);
        }

        protected IList<object[]> AsObject(Func<PersonnummerData, bool> filter)
        {
            return Data.Where(filter).Select(o =>
            {
                return new object[]
                {
                    o
                };
                ;
            }).ToList();
        }

        public virtual IEnumerator<object[]> GetEnumerator()
        {
            return AsObject(o => true).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class DataProvider: IEnumerable<object[]>
    {
        protected static readonly HttpClient webClient = new();
        protected static List<PersonnummerData> Data { get; }

        static DataProvider()
        {
            var response = webClient.GetStringAsync("https://raw.githubusercontent.com/personnummer/meta/master/testdata/list.json").Result;
            Data = JsonSerializer.Deserialize<List<PersonnummerData>>(response);
        }

        /// <inheritdoc />
        public virtual IEnumerator<object[]> GetEnumerator()
        {
            return AsObject(o => true).GetEnumerator();
        }

        protected IList<object[]> AsObject(Func<PersonnummerData, bool> filter)
        {
            return Data.Where(filter).Select(o =>
            {
                return new object[]
                {
                    o
                };
                ;
            }).ToList();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class OrgNumberDataProvider : IEnumerable<object[]>
    {
        protected static readonly HttpClient webClient = new();
        protected static List<PersonnummerData> Data { get; }

        static OrgNumberDataProvider()
        {
            var response = webClient.GetStringAsync("https://raw.githubusercontent.com/personnummer/meta/master/testdata/orgnumber.json").Result;
            Data = JsonSerializer.Deserialize<List<PersonnummerData>>(response);
        }

        /// <inheritdoc />
        public virtual IEnumerator<object[]> GetEnumerator()
        {
            return AsObject(o => true).GetEnumerator();
        }

        protected IList<object[]> AsObject(Func<PersonnummerData, bool> filter)
        {
            return Data.Where(filter).Select(o =>
            {
                return new object[]
                {
                    o
                };
                ;
            }).ToList();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class ValidInterimProvider : InterimDataProvider
    {
        public override IEnumerator<object[]> GetEnumerator()
        {
            return AsObject(o => o.Valid).GetEnumerator();
        }
    }

    public class InvalidInterimProvider : InterimDataProvider
    {
        public override IEnumerator<object[]> GetEnumerator()
        {
            return AsObject(o => !o.Valid).GetEnumerator();
        }
    }

    public class SsnDataProvider : DataProvider
    {
        public override IEnumerator<object[]> GetEnumerator()
        {
            return AsObject(o => o.Type == "ssn").GetEnumerator();
        }
    }

    public class CnDataProvider : DataProvider
    {
        public override IEnumerator<object[]> GetEnumerator()
        {
            return AsObject(o => o.Type != "ssn").GetEnumerator();
        }
    }

    public class ValidCnDataProvider : CnDataProvider
    {
        public override IEnumerator<object[]> GetEnumerator()
        {
            return AsObject(o => o.Type != "ssn" && o.Valid).GetEnumerator();
        }
    }

    public class InvalidCnDataProvider : CnDataProvider
    {

        public override IEnumerator<object[]> GetEnumerator()
        {
            return AsObject(o => o.Type != "ssn" && !o.Valid).GetEnumerator();
        }
}

    public class ValidSsnDataProvider : SsnDataProvider
    {

        public override IEnumerator<object[]> GetEnumerator()
        {
            return AsObject(o => o.Type == "ssn" && o.Valid).GetEnumerator();
        }
    }

    public class InvalidSsnDataProvider : SsnDataProvider
    {
        public override IEnumerator<object[]> GetEnumerator()
        {
            return AsObject(o => o.Type == "ssn" && !o.Valid).GetEnumerator();
        }
    }
}
