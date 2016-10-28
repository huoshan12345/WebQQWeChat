namespace WebQQ.Im.Bean
{
    /// <summary>
    /// 对方设置的加好友策略
    /// </summary>
    public enum QQAllow
    {
        /// <summary>允许所有人添加</summary>
        ALLOW_ALL, // 0

        /// <summary>需要验证信息</summary>
        NEED_CONFIRM, // 1

        /// <summary>拒绝任何人加好友</summary>
        REFUSE_ALL, // 2

        /// <summary>需要回答问题</summary>
        NEED_ANSWER, // 3

        /// <summary>需要验证和回答问题</summary>
        NEED_ANSWER_AND_CONFIRM, // 4
    }
}
