using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebQQ.Im.Bean.Discussion
{
    public class DiscussionMemberStatus : UserStatus
    {
        [JsonProperty("status")]
        public override QQStatusType Status { get; set; }
    }
}
