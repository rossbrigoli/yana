using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace com.rossbrigoli.Yana
{
    public class TradeDataMapper : IDataMapper<TradeData>
    {
        /// <summary>
        /// This method is invoked by KrakenJsonConverter to map the fields of the "result" portion of KrakenResponse
        /// </summary>
        /// <param name="jo"></param>
        /// <returns></returns>
        public TradeData MapFields(JObject jo)
        {
            var jp = jo.Properties().First();
            var data  = jp.Value.ToObject<object[][]>();

            var td = new TradeData();
            td.PairName = jp.Name;

            var entries = new List<Trade>();
            foreach( var trade in data)
            {
                var entry = new Trade();
                entry.Price = decimal.Parse(trade[0].ToString());
                entry.Volume = decimal.Parse(trade[1].ToString());
                entry.Time = UnixTime.Create(double.Parse(trade[2].ToString()));
                entry.TradeType = EnumHelper.GetValueFromDesc<TradeType>(trade[3].ToString());
                entry.MarketOrLimit = EnumHelper.GetValueFromDesc<MarketOrLimit>(trade[4].ToString());
                entry.Miscellaneous = trade[5].ToString();
                entries.Add(entry);
            }
            td.Trades = entries;
            return td;
        }
    }
}