using System;

namespace com.rossbrigoli.Yana
{
    public static class FromUnixTime
    {
        private static readonly DateTime Epoch = new DateTime(1970,1,1,0,0,0, DateTimeKind.Utc);
        public static DateTime Create(double unixTime)
        {
            return Epoch.AddSeconds(unixTime);
        }
    }
}