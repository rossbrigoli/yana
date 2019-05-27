using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace com.rossbrigoli.Yana
{
    public class OrderDataMapper : IDataMapper<OrderData>
    {
        public OrderData MapFields(JObject jo)
        {
            //var jp = jo.Properties().First();
            var data  = jo.ToObject<Dictionary<string, JToken>>();//Dictionary<string, Dictionary<string, object>>>>();

            var orderData = new OrderData();

            if(data.ContainsKey("open"))
            {
                var openOrders = data["open"].ToObject<Dictionary<string, Dictionary<string, object>>>();            
                orderData.OpenOrders = GenerateOrderInfo(openOrders);
            }

            if(data.ContainsKey("closed"))
            {
                var closedOrders = data["closed"].ToObject<Dictionary<string, Dictionary<string, object>>>();
                orderData.ClosedOrders = GenerateOrderInfo(closedOrders);

                orderData.Count = long.Parse(data["count"].ToString());
            }

            return orderData;
        }

        private IEnumerable<OrderInfo> GenerateOrderInfo(Dictionary<string, Dictionary<string, object>> data)
        {
            foreach(var entry in data)
            {
                var info = new OrderInfo();

                info.ReferenceId = entry.Key;
                info.UserReferenceId = long.Parse(entry.Value["userref"].ToString());
                info.Status = EnumHelper.GetValueFromDesc<OrderStatus>(entry.Value["status"].ToString());
                info.OpenTime = FromUnixTime.Create(double.Parse(entry.Value["opentm"].ToString()));
                info.StartTime = FromUnixTime.Create(double.Parse(entry.Value["starttm"].ToString()));
                info.ExpireTime = FromUnixTime.Create(double.Parse(entry.Value["expiretm"].ToString()));
                info.Volume = decimal.Parse(entry.Value["vol"].ToString());
                info.VolumeExecuted = decimal.Parse(entry.Value["vol_exec"].ToString());
                info.Cost = decimal.Parse(entry.Value["cost"].ToString());
                info.Fee = decimal.Parse(entry.Value["fee"].ToString());
                info.Price = decimal.Parse(entry.Value["price"].ToString());
                info.StopPrice = decimal.Parse(entry.Value["stopprice"].ToString());
                info.LimitPrice = decimal.Parse(entry.Value["limitprice"].ToString());
                info.Misc = entry.Value["misc"].ToString();
                info.Trades = entry.Value.ContainsKey("trades") ? (IEnumerable<string>) entry.Value["trades"] : null;
                info.CloseTime = entry.Value.ContainsKey("closetm") ? (Nullable<DateTime>) FromUnixTime.Create(long.Parse(entry.Value["closetm"].ToString())) : null;
                info.Reason = entry.Value.ContainsKey("reason") ? entry.Value["reason"].ToString() : null;

                var jObj = entry.Value["descr"] as JObject;
                info.DescriptionInfo = jObj.ToObject<OrderDesc>();

                yield return info;
            }
        }
    }
}