using System.Collections.Generic;
using Newtonsoft.Json;

namespace com.rossbrigoli.Yana
{
    [JsonConverter(typeof(OrderBookJsonConverter))]
    public class OrderBookResponse : IKrakenResponse
    {
        public string AssetPair { get; set; }
        public IEnumerable<OrderBookEntry> Asks { get; set; }
        public IEnumerable<OrderBookEntry> Bids { get; set; }
        public string[] Error { get; set; }
    }

    public class OrderBookEntry
    {
        public decimal Price { get; set; }
        public decimal Volume { get; set; }
        public long TimeStamp { get; set; }
    }
}