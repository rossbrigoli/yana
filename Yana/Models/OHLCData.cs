using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace com.rossbrigoli.Yana
{
    [JsonConverter(typeof(KrakenJsonConverter<OHLCDataMapper>))]
    public class OHLCData
    {
        public string PairName { get; set; }
        public IEnumerable<OHLCDataEntry> Data { get; set; }
        public long Last { get; set; }
    }

    public class OHLCDataEntry
    {
        public DateTime Time { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal  Close { get; set; }
        public decimal Vwap { get ;set; }
        public decimal Volume { get; set; }
        public long Count { get; set; }
    } 
}