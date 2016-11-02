using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Utility.Extensions;

namespace WebWeChat.Im.Bean
{
    public enum MessageType
    {
        Unknown = 0,

        /// <summary>
        /// 文字
        /// </summary>
        [Description("文字")]
        Text = 1,

        /// <summary>
        /// 图片
        /// </summary>
        [Description("图片")]
        Photo = 3,

        /// <summary>
        /// 语音
        /// </summary>
        [Description("语音")]
        Voice = 34,

        /// <summary>
        /// 名片
        /// </summary>
        [Description("名片")]
        BusinessCard = 42,

        /// <summary>
        /// 表情
        /// </summary>       
        [Description("表情")]
        Face = 47,

        /// <summary>
        /// 链接
        /// </summary>
        [Description("链接")]
        Link = 49,

        /// <summary>
        /// 成功获取联系人信息
        /// </summary>
        [Description("成功获取联系人信息")]
        GetContact = 51,

        /// <summary>
        /// 小视频
        /// </summary>
        [Description("小视频")]
        Video = 62,

        /// <summary>
        /// 消息撤回
        /// </summary>
        [Description("消息撤回")]
        MsgWithdraw = 10002
    }

    /// <summary>
    /// 接收到的消息
    /// </summary>
    public class Message
    {
        public string MsgId { get; set; }
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public MessageType MsgType { get; set; }
        public string Content { get; set; }
        public int Status { get; set; }
        public int ImgStatus { get; set; }
        public int CreateTime { get; set; }
        public int VoiceLength { get; set; }
        public int PlayLength { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string MediaId { get; set; }
        public string Url { get; set; }
        public int AppMsgType { get; set; }
        public int StatusNotifyCode { get; set; }
        public string StatusNotifyUserName { get; set; }
        public RecommendInfo RecommendInfo { get; set; }
        public int ForwardFlag { get; set; }
        public AppInfo AppInfo { get; set; }
        public int HasProductId { get; set; }
        public string Ticket { get; set; }
        public int ImgHeight { get; set; }
        public int ImgWidth { get; set; }
        public int SubMsgType { get; set; }
        public long NewMsgId { get; set; }
    }
}
