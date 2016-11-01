namespace WebWeChat.Im.Core
{
    public enum WeChatErrorCode
    {
        /// <summary>
        /// 响应错误
        /// </summary>
        ResponseError,

        /// <summary>
        /// 网络错误
        /// </summary> 
        IoError,

        /// <summary>
        /// 参数错误
        /// </summary>
        ParameterError,

        /// <summary>
        /// Cookie错误
        /// </summary>
        CookieError,

        Timeout,           // 等待超时

        JsonError,             // JSON解析出错

        /// <summary>
        /// 未知错误
        /// </summary>
        UnknownError,
    }
}
