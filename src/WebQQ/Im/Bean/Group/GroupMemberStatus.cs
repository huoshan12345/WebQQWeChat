using Newtonsoft.Json;

namespace WebQQ.Im.Bean.Group
{
    public class GroupMemberStatus
    {
        public long Uin { get; set; }

        [JsonProperty("client_type")]
        public int ClientType { get; set; }

        [JsonProperty("stat")]
        public QQStatusType Status { get; set; }
    }
}
