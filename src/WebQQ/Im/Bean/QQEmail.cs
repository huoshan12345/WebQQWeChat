namespace WebQQ.Im.Bean
{
    public class QQEmail
    {
        public string Id { get; set; }
        public string Sender { get; set; }
        public string SenderNick { get; set; }
        public string SenderEmail { get; set; }
        public string Subject { get; set; }
        public string Summary { get; set; }
        public bool Unread { get; set; }
        public long Flag { get; set; }
    }
}
