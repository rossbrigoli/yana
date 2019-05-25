using System.Collections.Generic;
using Newtonsoft.Json;

namespace com.rossbrigoli.Yana
{
    public class AssetInfo
    {
        [JsonProperty("aclass")]
        public string AssetClass { get; set; }
        public string AltName{ get; set; }
        public int Decimals { get; set; }
        [JsonProperty("display_decimals")]
        public int DisplayDecimals { get; set; }
    }
}