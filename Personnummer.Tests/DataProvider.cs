using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        public bool Valid { get; set; }
        public string Type { get; set; }
    }

    public class DataProvider: IEnumerable<object[]>
    {
        protected static List<PersonnummerData> Data { get; }

        static DataProvider()
        {
            WebRequest request = WebRequest.Create("https://raw.githubusercontent.com/personnummer/meta/master/testdata/list.json");
            string result = (new StreamReader(request.GetResponse().GetResponseStream())).ReadToEnd();
            Data = JsonConvert.DeserializeObject<List<PersonnummerData>>(result);
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

    public class PersonnummerDataProvider : DataProvider
    {
        public override IEnumerator<object[]> GetEnumerator()
        {
            return AsObject(o => o.Type == "ssn").GetEnumerator();
        }
    }

    public class CoordinationNumberProvider : DataProvider
    {
        public override IEnumerator<object[]> GetEnumerator()
        {
            return AsObject(o => o.Type != "ssn").GetEnumerator();
        }
    }
}
