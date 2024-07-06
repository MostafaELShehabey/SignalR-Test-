namespace SignalR_test.Model
{
    public class Message
    {
        public string Id { get; set; }
        public string Sender { get; set; }
        public string Reciever { get; set; }
        public string Mess { get; set; }
        public DateTime Time { get; set; }
        public  bool Read { get; set; }
    }
}
