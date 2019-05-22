using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace com.rossbrigoli.Yana
{
    public class KrakenResponse<T> : IKrakenResponse<T>
    {
        public string[] Error { get; set; }
        public T Result { get; set; }
    }
}