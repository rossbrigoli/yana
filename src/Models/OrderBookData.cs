using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace com.rossbrigoli.Yana
{
    [JsonConverter(typeof(KrakenJsonConverter<OrderBookDataMapper>))]
    public class OrderBookData
    {
        public string PairName { get; set; }
        public IEnumerable<OrderBookEntry> Asks { get; set; }
        public IEnumerable<OrderBookEntry> Bids { get; set; }
    }

    public class OrderBookEntry
    {
        public decimal Price { get; set; }
        public decimal Volume { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}