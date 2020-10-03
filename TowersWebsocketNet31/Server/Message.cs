namespace TowersWebsocketNet31.Server
{
    public class Message
    {
        public string _TARGET { get; set; }
        public string _ROOMID { get; set; }
        public string _SENDER { get; set; }
        public string _GRID { get; set; }
        public string _CLASS { get; set; }
        public string _METHOD { get; set; }
        public ArgumentMessage[] _ARGS { get; set; }
    }

    public class ArgumentMessage
    {
        public string tokenPlayer { get; set; }
        public string tokenTarget { get; set; }
        public string room { get; set; }
    }
}