
using System.Collections.Generic;
using Newtonsoft.Json;

namespace com.rossbrigoli.Yana
{
    [JsonConverter(typeof(KrakenJsonConverter<TickerDataMapper>))]
    public class TickerData
    {
        public Dictionary<string, TickerEntry> AssetPairs { get; set; }
    }

    public class TickerEntry
    {
        public (decimal Price, long WholeLotVolume, decimal LotVolume) Ask { get; set; }
        public (decimal Price, long WholeLotVolume, decimal LotVolume) Bid { get; set; }
        public (decimal Price, decimal LotVolume) LastTradeClosed { get; set; }
        public (decimal Today, decimal Last24H) Volume { get; set; }
        public (decimal Today, decimal Last24H) VWAP { get; set; }
        public (int Today, int Last24H) NumberOfTrades { get; set; }
        public (decimal Today, decimal Last24H) Low { get; set; }
        public (decimal Today, decimal Last24H) High { get; set; }
        public decimal TodaysOpeningPrice { get; set; }
    }
}