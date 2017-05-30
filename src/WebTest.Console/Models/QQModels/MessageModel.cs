using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebTest.Models.QQModels
{
    public enum QQMessageType
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

    public class QQMessageModel
    {
        [Required]
        public QQMessageType Type { get; set; }

        [Required]
        public string Text { get; set; }

        public string UserName { get; set; }
        public long UserUin { get; set; }
        
        public string QQId { get; set; }
    }
}
