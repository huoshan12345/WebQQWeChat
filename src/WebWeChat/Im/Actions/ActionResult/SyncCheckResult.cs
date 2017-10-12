namespace WebWeChat.Im.Actions.ActionResult
{
    public enum SyncCheckResult
    {
        /// <summary>
        /// 什么都没有
        /// </summary>
        Nothing = 0,

        /// <summary>
        /// 新消息
        /// </summary>
        NewMsg = 2,

        /// <summary>
        /// 正在使用手机微信
        /// </summary>
        UsingPhone = 7,

        /// <summary>
        /// 红包
        /// </summary>
        RedEnvelope = 6,

        /// <summary>
        /// 已离线
        /// </summary>
        Offline = 1100,

        /// <summary>
        /// 被踢
        /// </summary>
        Kicked = 1101,
    }
}
