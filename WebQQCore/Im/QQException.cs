using System;

namespace iQQ.Net.WebQQCore.Im
{
    public enum QQErrorCode
    {
        /** 登录凭证实效 */
        INVALID_LOGIN_AUTH,
        /** 参数无效 */
        INVALID_PARAMETER,
        /** 获取好友头像失败 */
        UNEXPECTED_RESPONSE,
        /** 无效的用户 */
        INVALID_USER,
        /** 密码错误 */
        WRONG_PASSWORD,
        /** 验证码错误 */
        WRONG_CAPTCHA,
        /** 需要验证 */
        NEED_CAPTCHA,
        /** 网络错误 */
        IO_ERROR,
        /** 网络超时 */
        IO_TIMEOUT,
        /** 用户没有找到 */
        USER_NOT_FOUND,
        /** 回答验证问题错误 */
        WRONG_ANSWER,
        /** 用户拒绝添加好友 */
        USER_REFUSE_ADD,
        /** 无法解析的结果 */
        INVALID_RESPONSE,
        /** 错误的状态码 */
        ERROR_HTTP_STATUS,
        /** 初始化错误 */
        INIT_ERROR,
        /** 用户取消操作 */
        CANCELED,
        /** 无法取消 */
        UNABLE_CANCEL,
        /** JSON解析出错 */
        JSON_ERROR,
        /** 未知的错误 */
        UNKNOWN_ERROR,
        /** 等待事件被中断 */
        WAIT_INTERUPPTED,
        /** 等待超时 */
        WAIT_TIMEOUT,
        /** COOKIE超时 */
        COOKIE_ERROR,
    }


    public class QQException : Exception
    {
        public QQErrorCode ErrorCode { get; set; }

        public QQException(QQErrorCode errorCode)
            : base(errorCode.ToString())
        {
            ErrorCode = errorCode;
        }

        public QQException(QQErrorCode errorCode, string msg)
            : base(msg)
        {
            ErrorCode = errorCode;
        }

        public QQException(QQErrorCode errorCode, Exception e)
            : base(e.Message, e)
        {
            ErrorCode = errorCode;
        }
    }
}
