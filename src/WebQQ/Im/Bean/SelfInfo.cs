using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebQQ.Im.Bean.Friend;

namespace WebQQ.Im.Bean
{
    public class SelfInfo
    {
        public virtual Birthday Birthday{ get; set; }
        public int Face { get; set; }
        public string Phone { get; set; }
        public string Occupation { get; set; }
        public AllowType Allow { get; set; }
        public string College { get; set; }
        public long Uin { get; set; }
        public int Blood { get; set; }
        public int Constel { get; set; }
        [JsonProperty("lnick")]
        public string LongNick { get; set; }
        public string Vfwebqq { get; set; }
        public string Homepage { get; set; }
        [JsonProperty("vip_info")]
        public int VipInfo { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Personal { get; set; }
        public int Shengxiao { get; set; }
        public string Nick { get; set; }
        public string Email { get; set; }
        public string Province { get; set; }
        public long Account { get; set; }
        public string Gender { get; set; }
        public string Mobile { get; set; }
    }
}
