using Newtonsoft.Json;

namespace WebQQ.Im.Bean.Group
{
    public class GroupMember : GroupMemberInfo
    {
        [JsonProperty("muin")]
        public override long Uin { get; set; }

        [JsonProperty("mflag")]
        public int Flag { get; set; }

        public string Card { get; set; }
        public int ClientType { get; set; }
        public QQStatusType Status { get; set; }
        public int VipLevel { get; set; }
        public int IsVip { get; set; }
    }
}
