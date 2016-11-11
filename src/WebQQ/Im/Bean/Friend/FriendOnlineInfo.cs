using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebQQ.Im.Bean.Friend
{
    public class FriendOnlineInfo
    {
        public long Uin { get; set; }

        [JsonProperty("client_type")]
        public int ClientType { get; set; }

        public QQStatusType Status { get; set; }
    }
}
