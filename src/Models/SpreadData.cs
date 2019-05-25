using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace com.rossbrigoli.Yana
{
    [JsonConverter(typeof(KrakenJsonConverter<SpreadDataMapper>))]
    public class SpreadData
    {
        public string PairName { get; set; }
        public IEnumerable<SpreadEntry> Data { get; set; }
    }

    public class SpreadEntry
    {
        public DateTime Time { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
    }
}