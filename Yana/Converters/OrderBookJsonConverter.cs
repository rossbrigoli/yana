
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace com.rossbrigoli.Yana
{
    public class OrderBookJsonConverter : JsonConverter
    {
        public override bool CanWrite => false;
        public override bool CanConvert(Type objectType)
        {
            return objectType.GetTypeInfo().IsClass;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object instance = Activator.CreateInstance(objectType);
            var props = objectType.GetTypeInfo().DeclaredProperties.ToList();
            var jo = JObject.Load(reader);
            foreach(var jp in jo.Properties())
            {
                var prop = props.FirstOrDefault(pi => pi.CanWrite && pi.Name.ToLower() == jp.Name.ToLower());
                if (prop != null)
                {
                    prop.SetValue(instance, jp.Value.ToObject(prop.PropertyType, serializer));
                }

                if (jp.Name.ToLower() == "result")
                {
                    var assetPair = jp.Value.ToObject<Dictionary<string,Dictionary<string, object[][]>>>();
                    props.First(c => c.Name.ToLower() == "assetpair").SetValue(instance, assetPair.Keys.First());
                    var asks = assetPair.First().Value.First(c => c.Key == "asks").Value;
                    var bids = assetPair.First().Value.First(c => c.Key == "bids").Value;

                    var askEntries = new List<OrderBookEntry>();
                    foreach(var askEntry in asks)
                    {
                        var item = new OrderBookEntry();
                        item.Price = decimal.Parse(askEntry[0].ToString());
                        item.Volume = decimal.Parse(askEntry[1].ToString());
                        item.TimeStamp = long.Parse(askEntry[2].ToString());
                        askEntries.Add(item);
                    }
                    props.First(pi => pi.CanWrite && pi.Name.ToLower() == "asks").SetValue(instance, askEntries);

                    var bidEntries = new List<OrderBookEntry>();
                    foreach(var bidEntry in bids)
                    {
                        var item = new OrderBookEntry();
                        item.Price = decimal.Parse(bidEntry[0].ToString());
                        item.Volume = decimal.Parse(bidEntry[1].ToString());
                        item.TimeStamp = long.Parse(bidEntry[2].ToString());
                        bidEntries.Add(item);
                    }
                    props.First(pi => pi.CanWrite && pi.Name.ToLower() == "bids").SetValue(instance, bidEntries);
                }
            }

            return instance;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}