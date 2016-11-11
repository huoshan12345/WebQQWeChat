using Newtonsoft.Json;

namespace WebQQ.Im.Bean.Group
{
    public class GroupMemberCard
    {
        [JsonProperty("muin")]
        public long Uin { get; set; }
        public string Card { get; set; }
    }
}
