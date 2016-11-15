using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebQQ.Im.Bean.Content;

namespace WebQQ.Im.Bean
{
    public enum MessageType
    {
        Friend, 		    //好友消息
        Group,		    // 群消息
        Discussion,		//讨论组消息
        Session         //临时会话消息
    }

    public class Message
    {
        [JsonIgnore]
        public MessageType Type { get; set; }

        [JsonIgnore]
        public List<IContentItem> Contents { get; set; } = new List<IContentItem>();

        [JsonProperty("from_uin")]
        public long FromUin { get; set; }
        [JsonProperty("msg_id")]
        public int MsgID { get; set; }
        [JsonProperty("msg_type")]
        public int MsgType { get; set; }
        [JsonProperty("time")]
        public int Time { get; set; }
        [JsonProperty("to_uin")]
        public int ToUin { get; set; }
        [JsonProperty("group_code")]
        public int GroupCode { get; set; }
        [JsonProperty("to_uin")]
        public int SendUin { get; set; }
    }
}
