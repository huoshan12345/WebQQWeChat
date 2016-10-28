namespace WebQQ.Im.Bean
{

    /// <summary>
    /// QQ好友，出现在好友列表的用户
    /// </summary>
    
    public class QQBuddy : QQUser
    {
        /// <summary>
        /// 备注
        /// </summary>
        public string MarkName { get; set; }
        /// <summary>
        /// 分组
        /// </summary>
        public QQCategory Category { get; set; }

        public string ShowName => string.IsNullOrEmpty(MarkName) ? Nickname : MarkName;

    }
}
