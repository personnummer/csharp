using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace Personnummer.Tests
{
    public struct PersonnummerData
    {
        public long Integer { get; set; }
        [JsonProperty("long_format")]
        public string LongFormat { get; set; }
        [JsonProperty("short_format")]
        public string ShortFormat { get; set; }
        [JsonProperty("separated_format")]
        public string SeparatedFormat { get; set; }
        [JsonProperty("separated_long")]
        public string SeparatedLong { get; set; }
        [JsonProperty("valid")]
        public bool Valid { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("isMale")]
        public bool IsMale { get; set; }
        [JsonProperty("isFemale")]
        public bool IsFemale { get; set; }
    }

    public class DataProvider: IEnumerable<object[]>
    {
        protected static readonly HttpClient webClient = new HttpClient();
        protected static List<PersonnummerData> Data { get; }

        static DataProvider()
        {
            var response = webClient.GetStringAsync("https://raw.githubusercontent.com/personnummer/meta/master/testdata/list.json").Result;
            Data = JsonConvert.DeserializeObject<List<PersonnummerData>>(response);
        }

        /// <inheritdoc />
        public virtual IEnumerator<object[]> GetEnumerator()
        {
            return AsObject(o => true).GetEnumerator();
        }

        protected IList<object[]> AsObject(Func<PersonnummerData, bool> filter)
        {
            return Data.Where<PersonnummerData>(filter).Select<PersonnummerData, object[]>(o =>
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
        protected static readonly HttpClient webClient = new HttpClient();
        protected static List<PersonnummerData> Data { get; }

        static OrgNumberDataProvider()
        {
            var response = webClient.GetStringAsync("https://raw.githubusercontent.com/personnummer/meta/master/testdata/orgnumber.json").Result;
            Data = JsonConvert.DeserializeObject<List<PersonnummerData>>(response);
        }

        /// <inheritdoc />
        public virtual IEnumerator<object[]> GetEnumerator()
        {
            return AsObject(o => true).GetEnumerator();
        }

        protected IList<object[]> AsObject(Func<PersonnummerData, bool> filter)
        {
            return Data.Where<PersonnummerData>(filter).Select<PersonnummerData, object[]>(o =>
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

    public class ValidFormatSsnDataProvider : SsnDataProvider
    {
        public override IEnumerator<object[]> GetEnumerator()
        {
            var json = @"[{        ""integer"": 199305269996,        ""long_format"": ""199305269996"",        ""short_format"": ""9305269996"",        ""separated_format"": ""930526-9996"",        ""separated_long"": ""19930526-9996"",        ""valid"": true,        ""type"": ""ssn"",        ""isMale"": true,        ""isFemale"": false    }]";

            var data = JsonConvert.DeserializeObject<List<PersonnummerData>>(json);

            return data.Select(x => new object[] {x}).GetEnumerator();
        }
    }

    public class InvalidFormatSsnDataProvider : SsnDataProvider
    {
        public override IEnumerator<object[]> GetEnumerator()
        {
            var json = @"[{        ""integer"": 199305269996,        ""long_format"": ""9305269996"",        ""short_format"": ""199305269996"",        ""separated_format"": ""19930526-9996"",        ""separated_long"": ""930526-9996"",        ""valid"": true,        ""type"": ""ssn"",        ""isMale"": true,        ""isFemale"": false    }]";

            var data = JsonConvert.DeserializeObject<List<PersonnummerData>>(json);

            return data.Select(x => new object[] { x }).GetEnumerator();
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
