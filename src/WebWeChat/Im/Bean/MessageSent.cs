using FxUtility.Extensions;
using System;

namespace WebWeChat.Im.Bean
{
    /// <summary>
    /// 要发送的消息
    /// </summary>
    public class MessageSent
    {
        public MessageType Type { get; }
        public string Content { get; }
        public string FromUserName { get; }
        public string ToUserName { get; }
        public string LocalID { get; }
        public string ClientMsgId { get; }

        private MessageSent()
        {
            var time = DateTime.Now.ToTimestampMilli();
            var random = new Random().Next(0, 9999).ToString("d4");
            LocalID = $"{time}{random}";
            ClientMsgId = LocalID;
        }

        public MessageSent(MessageType type, string content, string fromUserName, string toUserName)
            : this()
        {
            Type = type;
            Content = content;
            FromUserName = fromUserName;
            ToUserName = toUserName;
        }

        public static MessageSent CreateTextMsg(string content, string fromUserName, string toUserName)
        {
            return new MessageSent(MessageType.Text, content, fromUserName, toUserName);
        }
    }
}
