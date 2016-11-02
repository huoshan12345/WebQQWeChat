using System;
using Utility.Extensions;

namespace WebWeChat.Im.Bean
{
    /// <summary>
    /// 要发送的消息
    /// </summary>
    public class MessageSent
    {
        public MessageType Type { get; set; }
        public string Content { get; set; }
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public string LocalID { get; }
        public string ClientMsgId { get; }

        public MessageSent()
        {
            var time = DateTime.Now.ToTimestampMilli();
            var random = new Random().Next(0, 9999).ToString("d4");
            LocalID = $"{time}{random}";
            ClientMsgId = LocalID;
        }
    }
}
