using System.ComponentModel;

namespace com.rossbrigoli.Yana
{
    public enum OrderType
    {
        [Description("market")]
        Market = 0,
        [Description("limit")]
        Limit = 1,
        [Description("stop-loss")]
        StopLoss = 2,
        [Description("take-profit")]
        TakeProfit = 3,
        [Description("stop-loss-profit")]
        StopLossProfit = 4,
        [Description("stop-loss-profit-limit")]
        StopLossProfitLimit = 5,
        [Description("stop-loss-limit")]
        StopLossLimit = 6,
        [Description("take-profit-limit")]
        TakeProfitLimit = 7,
        [Description("trailing-stop")]
        TrailingStop = 8,
        [Description("trailing-stop-limit")]
        TrailingStopLimit = 9,
        [Description("stop-loss-and-limit")]
        StopLossAndLimit = 10,
        [Description("settle-position")]
        SettlePosition = 11
    }
}