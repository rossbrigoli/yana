using System.ComponentModel;

namespace com.rossbrigoli.Yana
{
    public enum OrderStatus
    {
        [Description("pending")]
        Pending = 0,
        [Description("open")]
        Open = 1,
        [Description("closed")]
        Closed = 2,
        [Description("cancelled")]
        Cancelled = 3,
        [Description("expired")]
        Expired = 4
    }
}