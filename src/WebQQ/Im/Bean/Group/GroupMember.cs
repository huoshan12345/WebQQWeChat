namespace WebQQ.Im.Bean.Group
{
    public class GroupMember
    {
        public long Uin { get; set; }
        public string Card { get; set; }
        public string Nick { get; set; }
        public string Province { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int ClientType { get; set; }
        public QQStatusType Status { get; set; }
        public int VipLevel { get; set; }
        public int IsVip { get; set; }
        public int Flag { get; set; }
    }
}
