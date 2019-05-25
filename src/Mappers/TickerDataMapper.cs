using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace com.rossbrigoli.Yana
{
    public class TickerDataMapper : IDataMapper<TickerData>
    {
        /// <summary>
        /// This method is invoked by KrakenJsonConverter to map the fields of the "result" portion of KrakenResponse
        /// </summary>
        /// <param name="jo"></param>
        /// <returns></returns>
        public TickerData MapFields(JObject jo)
        {
            var obj = jo.ToObject<Dictionary<string, Dictionary<string, object>>>(); 

            var td = new TickerData();
            var assetPair = new Dictionary<string, TickerEntry>();
            td.AssetPairs = assetPair;

            foreach(var pair in obj)
            {
                var tickerEntry = new TickerEntry();
                assetPair.Add(pair.Key, tickerEntry);

                var mapAsk = ((IEnumerable) pair.Value["a"])
                    .Cast<object>().Select(x => x == null ? x : x.ToString())
                    .ToArray();
                tickerEntry.Ask = 
                (
                    Price: decimal.Parse(mapAsk[0].ToString()), 
                    WholeLotVolume: long.Parse(mapAsk[1].ToString()),
                    LotVolume: decimal.Parse(mapAsk[2].ToString())
                );

                var mapBid = ((IEnumerable) pair.Value["b"])
                    .Cast<object>().Select(x => x == null ? x : x.ToString())
                    .ToArray();
                tickerEntry.Bid = 
                (
                    Price: decimal.Parse(mapBid[0].ToString()), 
                    WholeLotVolume: long.Parse(mapBid[1].ToString()),
                    LotVolume: decimal.Parse(mapBid[2].ToString())
                );

                var mapLTC = ((IEnumerable) pair.Value["c"])
                    .Cast<object>().Select(x => x == null ? x : x.ToString())
                    .ToArray();
                tickerEntry.LastTradeClosed =
                (
                    Price: decimal.Parse(mapLTC[0].ToString()),
                    LotVolume: decimal.Parse(mapLTC[1].ToString())
                );

                var mapV = ((IEnumerable) pair.Value["v"])
                    .Cast<object>().Select(x => x == null ? x : x.ToString())
                    .ToArray();
                tickerEntry.Volume = 
                (
                    Today: decimal.Parse(mapV[0].ToString()),
                    Last24H: decimal.Parse(mapV[1].ToString())
                );

                var mapVWAP = ((IEnumerable) pair.Value["p"])
                    .Cast<object>().Select(x => x == null ? x : x.ToString())
                    .ToArray();
                tickerEntry.VWAP =
                (
                    Today: decimal.Parse(mapVWAP[0].ToString()),
                    Last24H: decimal.Parse(mapVWAP[1].ToString())
                );

                var mapNOT = ((IEnumerable) pair.Value["t"])
                    .Cast<object>().Select(x => x == null ? x : x.ToString())
                    .ToArray();
                tickerEntry.NumberOfTrades =
                (
                    Today: int.Parse(mapNOT[0].ToString()),
                    Last24H: int.Parse(mapNOT[1].ToString())
                );

                var mapLow = ((IEnumerable) pair.Value["l"])
                    .Cast<object>().Select(x => x == null ? x : x.ToString())
                    .ToArray();
                tickerEntry.Low =
                (
                    Today: decimal.Parse(mapLow[0].ToString()),
                    Last24H: decimal.Parse(mapLow[1].ToString())
                );

                var mapHigh = ((IEnumerable) pair.Value["h"])
                    .Cast<object>().Select(x => x == null ? x : x.ToString())
                    .ToArray();
                tickerEntry.High =
                (
                    Today: decimal.Parse(mapHigh[0].ToString()),
                    Last24H: decimal.Parse(mapHigh[1].ToString())
                );
                
                tickerEntry.TodaysOpeningPrice = decimal.Parse(pair.Value["o"].ToString());
            }
            return td;
            
        }
    }
}