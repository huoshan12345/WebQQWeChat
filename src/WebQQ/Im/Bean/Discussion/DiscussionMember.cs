using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebQQ.Im.Bean.Discussion
{
    public class DiscussionMember
    {
        [JsonProperty("mem_uin")]
        public long Uin { get; set; }

        [JsonProperty("ruin")]
        public long QQNumber { get; set; }

        public string Nick { get; set; }
        
        public int ClientType { get; set; }
        
        public virtual QQStatusType Status { get; set; }
    }
}
