

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace com.rossbrigoli.Yana
{
    public class OHLCDataMapper : IDataMapper<OHLCData>
    {
        public OHLCData MapFields(JObject jo)
        {
            var jp = jo.Properties().First(c => c.Name.ToLower() != "last");
            var last = jo.Properties().First(c => c.Name == "last");
            var data  = jp.Value.ToObject<object[][]>();

            var ohlcData = new OHLCData();
            ohlcData.PairName = jp.Name;
            ohlcData.Last = last.Value.ToObject<long>();
            
            var entries = new List<OHLCDataEntry>();
            foreach(var item in data)
            {
                var entry = new OHLCDataEntry();
                entry.Time = UnixTime.Create(double.Parse(item[0].ToString()));
                entry.Open = decimal.Parse(item[1].ToString());
                entry.High = decimal.Parse(item[2].ToString());
                entry.Low =  decimal.Parse(item[3].ToString());
                entry.Close = decimal.Parse(item[4].ToString());
                entry.Vwap = decimal.Parse(item[5].ToString());
                entry.Volume = decimal.Parse(item[6].ToString());
                entry.Count = long.Parse(item[7].ToString());
                entries.Add(entry);
            }
            ohlcData.Data = entries;

            return ohlcData;
        }
    }
}