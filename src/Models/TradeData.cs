using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace com.rossbrigoli.Yana
{
    [JsonConverter(typeof(KrakenJsonConverter<TradeDataMapper>))]
    public class TradeData
    {
        public string PairName { get; set; }
        public IEnumerable<Trade> Trades { get; set; }
    }
    public class Trade
    {
        public decimal Price { get; set; }
        public decimal Volume { get; set; }
        public DateTime Time { get; set; }
        public TradeType TradeType { get; set; }
        public MarketOrLimit MarketOrLimit { get; set; }
        public string Miscellaneous { get; set; }
    }
}