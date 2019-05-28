using System.ComponentModel;

namespace com.rossbrigoli.Yana
{
    public enum MarketOrLimit
    {
        [Description("m")]
        Market = 0,
        [Description("l")]
        Limit = 1
    }
}