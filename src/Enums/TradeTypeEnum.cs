using System.ComponentModel;

namespace com.rossbrigoli.Yana
{
    public enum TradeType
    {
        [Description("b")]
        Buy = 0,
        [Description("s")]
        Sell = 1
    }

    public enum TradeTypeOrder
    {
        [Description("buy")]
        Buy = 0,
        [Description("sell")]
        Sell = 1
    }
}