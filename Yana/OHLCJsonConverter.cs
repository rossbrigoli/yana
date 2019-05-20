

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace com.rossbrigoli.Yana
{
    public class OHLCJsonConverter : JsonConverter
{   
    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType.GetTypeInfo().IsClass;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        object instance = Activator.CreateInstance(objectType);
        var props = objectType.GetTypeInfo().DeclaredProperties.ToList();

        JObject jo = JObject.Load(reader);
         
        var matchedJProps = new List<string>();
        foreach (JProperty jp in jo.Properties())
        {
            PropertyInfo prop = props.FirstOrDefault(pi =>
                pi.CanWrite && pi.Name.ToLower() == jp.Name.ToLower());
            if (prop != null)
            {
                prop.SetValue(instance, jp.Value.ToObject(prop.PropertyType, serializer));
                matchedJProps.Add(jp.Name);
            }
        }

        var unmatched = jo.Properties().FirstOrDefault(p => !matchedJProps.Contains(p.Name));

        if (unmatched != null)
        {
            var propName = props.FirstOrDefault(pi => pi.Name == "PairName");
            propName.SetValue(instance, unmatched.Name);

            var prop = props.FirstOrDefault(pi => pi.Name == "Data");
            
            if (prop != null)
            {
                var values = (object[][]) unmatched.Value.ToObject(typeof(object[][]), serializer);
                
                var children = new List<OHLCDataEntry>();

                for (int i=0;i<values.Count(); i++)
                {
                    var childInstance = (OHLCDataEntry) Activator.CreateInstance(typeof(OHLCDataEntry));
                    var cProps = typeof(OHLCDataEntry).GetTypeInfo().DeclaredProperties.ToList();

                    cProps.First(pi => pi.Name == "Time").SetValue(childInstance, values[i][0]);
                    cProps.First(pi => pi.Name == "Open").SetValue(childInstance, decimal.Parse(values[i][1].ToString()));
                    cProps.First(pi => pi.Name == "High").SetValue(childInstance, decimal.Parse(values[i][2].ToString()));
                    cProps.First(pi => pi.Name == "Low").SetValue(childInstance, decimal.Parse(values[i][3].ToString()));
                    cProps.First(pi => pi.Name == "Close").SetValue(childInstance, decimal.Parse(values[i][4].ToString()));
                    cProps.First(pi => pi.Name == "Vwap").SetValue(childInstance, decimal.Parse(values[i][5].ToString()));
                    cProps.First(pi => pi.Name == "Volume").SetValue(childInstance, decimal.Parse(values[i][6].ToString()));
                    cProps.First(pi => pi.Name == "Count").SetValue(childInstance, values[i][7]);
                    children.Add(childInstance);
                }

                prop.SetValue(instance, children);
            }
        }

        return instance;
    }
}
}