using System.Collections.Generic;
using Newtonsoft.Json;

namespace com.rossbrigoli.Yana
{

    public class AssetPair
    {
        public string AltName { get; set; }
        public string WsName { get; set; }
        [JsonProperty("aclass_base")]
        public string AssetClassBase { get; set; }
        public string Base { get; set; }
        [JsonProperty("aclass_quote")]
        public string AssetClassQuote { get; set; }
        public string Quote { get; set; }
        public string Lot { get; set; }
        [JsonProperty("pair_decimals")]
        public int PairDecimals { get; set; }
        [JsonProperty("lot_decimals")]
        public int LotDecimals { get; set; }
        [JsonProperty("lot_multiplier")]
        public string LotMultiplier { get; set; }
        [JsonProperty("leverage_buy")]
        public int[] LeverageBuy { get; set; }
        [JsonProperty("leverage_sell")]
        public int[] LeverageSell { get; set; }
        public IEnumerable<double[]> Fees { get; set; }
        [JsonProperty("fees_maker")]
        public IEnumerable<decimal[]> FeesMaker { get; set; }
        [JsonProperty("fee_volume_currency")]
        public string FeeVolumeCurrency { get; set; }
        [JsonProperty("magin_call")]
        public decimal MarginCall { get; set; }
        public decimal MarginStop { get; set; }
    }
}