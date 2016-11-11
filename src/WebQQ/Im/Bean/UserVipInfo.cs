using Newtonsoft.Json;

namespace WebQQ.Im.Bean
{
    public class UserVipInfo
    {
        [JsonProperty("u")]
        public long Uin { get; set; }

        [JsonProperty("vip_level")]
        public int VipLevel { get; set; }

        [JsonProperty("is_vip")]
        public int IsVip { get; set; }
    }
}
