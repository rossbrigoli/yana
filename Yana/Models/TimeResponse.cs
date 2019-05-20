namespace com.rossbrigoli.Yana
{
    public class TimeResponse : IKrakenResponse
    {
        public string[] Error { get; set; }

        public TimeResult Result { get; set; }
    }

    public class TimeResult
    {
        public long UnixTime { get; set; }
        public string Rfc1123 { get; set; }
    }
}