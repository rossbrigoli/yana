
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace com.rossbrigoli.Yana
{
    public class OrderBookDataMapper : IDataMapper<OrderBookData>
    {

        public OrderBookData MapFields(JObject jo)
        {
            var jp = jo.Properties().First();
            var data  = jp.Value.ToObject<Dictionary<string, object[][]>>();
            var asks = data["asks"];
            var bids = data["bids"];
            var obd = new OrderBookData();
            obd.PairName = jp.Name;

            var askEntries = new List<OrderBookEntry>();
            foreach(var askEntry in asks)
            {
                var item = new OrderBookEntry();
                item.Price = decimal.Parse(askEntry[0].ToString());
                item.Volume = decimal.Parse(askEntry[1].ToString());
                item.TimeStamp = long.Parse(askEntry[2].ToString());
                askEntries.Add(item);
            }

            obd.Asks = askEntries;

            var bidEntries = new List<OrderBookEntry>();
            foreach(var bidEntry in bids)
            {
                var item = new OrderBookEntry();
                item.Price = decimal.Parse(bidEntry[0].ToString());
                item.Volume = decimal.Parse(bidEntry[1].ToString());
                item.TimeStamp = long.Parse(bidEntry[2].ToString());
                bidEntries.Add(item);
            }

            obd.Bids = bidEntries;

            return obd;
        }
    }
}