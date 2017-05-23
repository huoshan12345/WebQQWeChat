using System.ComponentModel;

namespace WebQQ.Im.Core
{
    public enum QQErrorCode
    {
        /// <summary>
        /// 登录凭证失效
        /// </summary>
        [Description("登录凭证失效")]
        InvalidLoginAuth,

        /// <summary>
        /// 响应错误
        /// </summary>
        [Description("响应错误")]
        ResponseError,

        /// <summary>
        /// 网络错误
        /// </summary> 
        [Description("网络错误")]
        IoError,

        /// <summary>
        /// 参数错误
        /// </summary>
        [Description("参数错误")]
        ParameterError,

        /// <summary>
        /// Cookie错误
        /// </summary>
        [Description("Cookie错误")]
        CookieError,

        Timeout,           // 等待超时

        JsonError,             // JSON解析出错

        /// <summary>
        /// 未知错误
        /// </summary>
        [Description("未知错误")]
        UnknownError,
    }

}
