using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace com.rossbrigoli.Yana
{
    [JsonConverter(typeof(KrakenJsonConverter<OrderDataMapper>))]
    public class OrderData
    {
        public IEnumerable<OrderInfo> OpenOrders { get; set; }
        public IEnumerable<OrderInfo> ClosedOrders {get; set; }
        public long Count { get; set; }
    }

    public class OrderInfo
    {
        public string ReferenceId { get; set; }
        public long UserReferenceId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ExpireTime { get; set; }
        public OrderDesc DescriptionInfo { get; set; }
        public decimal Volume { get; set; }
        public decimal VolumeExecuted { get ;set; }
        public decimal Cost { get; set; }
        public decimal Fee { get; set; }
        public decimal Price { get; set; }
        public decimal StopPrice { get; set; }
        public decimal LimitPrice { get; set; }
        public string Misc { get; set; }
        public string OrderFlags { get ;set;}
        public IEnumerable<string> Trades { get; set; }
        public DateTime? CloseTime { get; set; }
        public string Reason { get; set; }
    }

    public class OrderDesc
    {
        [JsonProperty("pair")]
        public string PairName { get; set; }
        public TradeTypeOrder Type { get; set; }
        public OrderType OrderType { get; set; }
        public decimal Price { get; set; }
        public decimal Price2 { get; set; }
        public string Leverage { get; set; }
        [JsonProperty("order")]
        public string OrderDescription { get; set; }
        [JsonProperty("close")]
        public string CloseDescription { get; set;}

    }
}