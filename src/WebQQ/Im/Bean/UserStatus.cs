using Newtonsoft.Json;

namespace WebQQ.Im.Bean
{
    public class UserStatus
    {
        public long Uin { get; set; }

        [JsonProperty("client_type")]
        public int ClientType { get; set; }

        [JsonProperty("stat")]
        public virtual QQStatusType Status { get; set; }
    }
}
