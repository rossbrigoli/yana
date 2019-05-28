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
            var data  = jo.ToObject<Dictionary<string, JToken>>();
            var orderData = new OrderData();
            var orders = new List<OrderInfo>();

            if(data.ContainsKey("open"))
            {
                var openOrders = data["open"].ToObject<Dictionary<string, Dictionary<string, object>>>();            
                orders.AddRange(GenerateOrderInfo(openOrders));
            }

            if(data.ContainsKey("closed"))
            {
                var closedOrders = data["closed"].ToObject<Dictionary<string, Dictionary<string, object>>>();
                orders.AddRange(GenerateOrderInfo(closedOrders));
                
            }

            if(data.ContainsKey("count"))
            {
                orderData.Count = long.Parse(data["count"].ToString());                
            }

            var otherKeys = data.Where(c => c.Key != "open" && c.Key != "closed" && c.Key != "count")
                .ToDictionary(c => c.Key, d => d.Value.ToObject<Dictionary<string, object>>());

            orders.AddRange(GenerateOrderInfo(otherKeys));
            orderData.Orders = orders;
            return orderData;
        }

        private IEnumerable<OrderInfo> GenerateOrderInfo(Dictionary<string, Dictionary<string, object>> data)
        {
            foreach(var entry in data)
            {
                var info = new OrderInfo();

                var refId = entry.Value["refid"];
                var userref = entry.Value["userref"];
                info.TransactionId = entry.Key;
                info.ReferenceId = refId != null ? refId.ToString() : null;
                info.UserReferenceId = userref != null ? userref.ToString() : null;

                info.Status = EnumHelper.GetValueFromDesc<OrderStatus>(entry.Value["status"].ToString());
                info.OpenTime = FromUnixTime.Create(double.Parse(entry.Value["opentm"].ToString()));
                info.StartTime = FromUnixTime.Create(double.Parse(entry.Value["starttm"].ToString()));
                info.ExpireTime = FromUnixTime.Create(double.Parse(entry.Value["expiretm"].ToString()));
                info.Volume = decimal.Parse(entry.Value["vol"].ToString());
                info.VolumeExecuted = decimal.Parse(entry.Value["vol_exec"].ToString());
                info.Cost = decimal.Parse(entry.Value["cost"].ToString());
                info.Fee = decimal.Parse(entry.Value["fee"].ToString());
                info.Price = decimal.Parse(entry.Value["price"].ToString());
                info.StopPrice = entry.Value.ContainsKey("stopprice") 
                    ? decimal.Parse(entry.Value["stopprice"].ToString()) : 0;
                info.LimitPrice = entry.Value.ContainsKey("limitprice") 
                    ? decimal.Parse(entry.Value["limitprice"].ToString()) : 0;
                info.Misc = entry.Value["misc"].ToString();

                info.Trades = new List<string>();
                if(entry.Value.ContainsKey("trades"))
                {
                    var tObj = entry.Value["trades"] as JToken;
                    info.Trades = tObj.ToObject<string[]>();
                }

                info.CloseTime = entry.Value.ContainsKey("closetm") ? (Nullable<DateTime>) FromUnixTime.Create(double.Parse(entry.Value["closetm"].ToString())) : null;
                info.Reason = entry.Value.ContainsKey("reason") && entry.Value["reason"] != null 
                    ? entry.Value["reason"].ToString() : null;

                var jObj = entry.Value["descr"] as JObject;
                info.DescriptionInfo = jObj.ToObject<OrderDesc>();

                yield return info;
            }
        }
    }
}