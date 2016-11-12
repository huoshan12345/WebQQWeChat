using Newtonsoft.Json;

namespace WebQQ.Im.Bean.Friend
{
    public class FriendBaseInfo
    {
        public long Uin { get; set; }

        public int Face { get; set; }

        [JsonProperty("flag")]
        public int InfoFlag { get; set; }

        public string Nick { get; set; }
    }
}
