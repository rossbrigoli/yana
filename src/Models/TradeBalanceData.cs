using System;
using Newtonsoft.Json;

namespace com.rossbrigoli.Yana
{
    public class TradeBalanceData
    {
        // eb = equivalent balance (combined balance of all currencies)
        // tb = trade balance (combined balance of all equity currencies)
        // m = margin amount of open positions
        // n = unrealized net profit/loss of open positions
        // c = cost basis of open positions
        // v = current floating valuation of open positions
        // e = equity = trade balance + unrealized net profit/loss
        // mf = free margin = equity - initial margin (maximum margin available to open new positions)
        // ml = margin level = (equity / initial margin) * 100
        [JsonProperty("eb")]
        public decimal EquivalentBalance { get; set; }
        [JsonProperty("tb")]
        public decimal TradeBalance { get; set; }
        [JsonProperty("m")]
        public decimal Margin { get; set; }
        [JsonProperty("n")]
        public decimal UnrealizedNetProfit { get; set; }
        [JsonProperty("c")]
        public decimal CostBasisOfOpenPositions { get; set; }
        [JsonProperty("v")]
        public decimal CurrentFloatingValuation { get; set; }
        [JsonProperty("e")]
        public decimal Equity { get; set; }
        [JsonProperty("mf")]
        public decimal FreeMargin { get; set; }
        [JsonProperty("ml")]
        public decimal MarginLevel { get; set; }
    }
}