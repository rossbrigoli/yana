
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace com.rossbrigoli.Yana
{
    /// <summary>
    /// Converts a Kraken Json into C# objects
    /// </summary>
    /// <typeparam name="T">IDataMapper&gtTargetModel&lt</typeparam>
    public class KrakenJsonConverter<T>  : JsonConverter
    {
        public override bool CanWrite => false;
        public override bool CanConvert(Type objectType)
        {
            return objectType.GetTypeInfo().IsClass;
        }

        /// <summary>
        /// This reads the value of the "result" property
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object instance = Activator.CreateInstance(objectType);
            var jo = JObject.Load(reader);

            var method = typeof(T).GetMethod("MapFields"); //MapFields(jp);
            var mapper = Activator.CreateInstance(typeof(T));
            var result = method.Invoke(mapper, new object[] { jo });

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}