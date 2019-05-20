
using System.Collections.Generic;

namespace com.rossbrigoli.Yana
{
    public class TickerResponse : IKrakenResponse
    {
        public string[] Error { get; set; }
        public Dictionary<string, TickerPair> Result { get; set; }

    }

    public class TickerPair
    {
        public string[] a { get; set; }
        public string[] b { get; set; }
        public string[] c { get; set; }
        public string[] v { get; set; }
        public string[] p { get; set; }
        public long[] t { get; set; }
        public string[] l { get; set; }
        public string[] h { get; set; }
        public string o { get; set; }
    }
}