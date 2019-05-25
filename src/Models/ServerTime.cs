using System;

namespace com.rossbrigoli.Yana
{
    public class ServerTime
    {
        public long UnixTime { get; set; }
        public string Rfc1123 { get; set; }

        public DateTime DateTime
        {
            get 
            {
                return FromUnixTime.Create(this.UnixTime);
            }
        }
    }
}