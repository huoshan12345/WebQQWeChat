using Newtonsoft.Json;

namespace WebQQ.Im.Bean.Friend
{
    public class FriendMarkName
    {
        public long Uin { get; set; }

        public string MarkName { get; set; }

        [JsonProperty("type")]
        public int MarknameType { get; set; }
    }
}
