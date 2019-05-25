using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace com.rossbrigoli.Yana
{
    public class SpreadDataMapper : IDataMapper<SpreadData>
    {
        public SpreadData MapFields(JObject jo)
        {
            var jp = jo.Properties().First();
            var data  = jp.Value.ToObject<object[][]>();

            var td = new SpreadData();
            td.PairName = jp.Name;

            var entries = new List<SpreadEntry>();
            foreach( var trade in data)
            {
                var entry = new SpreadEntry();
                entry.Time = UnixTime.Create(double.Parse(trade[0].ToString()));
                entry.Bid = decimal.Parse(trade[0].ToString());
                entry.Ask = decimal.Parse(trade[1].ToString());

                entries.Add(entry);
            }
            td.Data = entries;
            return td;
        }
    }
}