using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using FclEx.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Bean.Content;
using WebQQ.Util;

namespace WebQQ.Im.Bean
{
    public enum MessageType
    {
        /// <summary>
        /// 好友消息
        /// </summary>
        [Description("好友消息")]
        Friend,
        /// <summary>
        /// 群消息
        /// </summary>
        [Description("群消息")]
        Group,
        /// <summary>
        /// 讨论组消息
        /// </summary>
        [Description("讨论组消息")]
        Discussion,
        /// <summary>
        /// 临时会话消息
        /// </summary>
        [Description("临时会话消息")]
        Session         
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
        public long Time { get; set; }
        [JsonProperty("to_uin")]
        public long ToUin { get; set; }
        [JsonProperty("group_code")]
        public long GroupCode { get; set; }
        [JsonProperty("send_uin")]
        public long SendUin { get; set; }

        public string GetText()
        {
            return string.Join("", Contents.Select(m => m.GetText()).Where(m => !m.IsNullOrWhiteSpace()));
        }

        public string PackContentList()
        {
            // ["font",{"size":10,"color":"808080","style":[0,0,0],"name":"\u65B0\u5B8B\u4F53"}]
            var json = new JArray();
            foreach (var contentItem in Contents)
            {
                json.Add(contentItem.ToJson());
            }
            return json.ToJson();
        }
    }
}
