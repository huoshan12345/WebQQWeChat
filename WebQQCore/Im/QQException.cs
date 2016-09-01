using iQQ.Net.WebQQCore.Util;
using System;
using System.IO;
using System.Net;
using iQQ.Net.WebQQCore.Util.Extensions;
using Newtonsoft.Json;

namespace iQQ.Net.WebQQCore.Im
{
    public enum QQErrorCode
    {
        INVALID_LOGIN_AUTH,     // 登录凭证失效
        INVALID_PARAMETER,      // 参数无效
        UNEXPECTED_RESPONSE,    // 获取好友头像失败
        INVALID_USER,           // 无效的用户
        WRONG_PASSWORD,         // 密码错误
        WRONG_CAPTCHA,          // 验证码错误
        NEED_CAPTCHA,           // 需要验证
        IO_ERROR,               // 网络错误

        /// <summary>
        /// 网络超时
        /// </summary>
        IO_TIMEOUT,            
        USER_NOT_FOUND,         // 用户没有找到
        WRONG_ANSWER,           // 回答验证问题错误
        USER_REFUSE_ADD,        // 用户拒绝添加好友
        INVALID_RESPONSE,       // 无法解析的结果
        ERROR_HTTP_STATUS,      // 错误的状态码
        INIT_ERROR,             // 初始化错误
        CANCELED,               // 用户取消操作
        UNABLE_CANCEL,          // 无法取消
        JSON_ERROR,             // JSON解析出错
        UNKNOWN_ERROR,          // 未知的错误
        WAIT_INTERUPPTED,       // 等待事件被中断
        WAIT_TIMEOUT,           // 等待超时
        COOKIE_ERROR,           // COOKIE超时
        SNED_MSG_ERROR,         // 发送消息失败
    }


    public class QQException : Exception
    {
        private static QQErrorCode GetErrorCode(Exception e)
        {
            if (e is TimeoutException) return QQErrorCode.IO_TIMEOUT;
            if (e is IOException) return QQErrorCode.IO_ERROR;
            if (e is ArgumentException) return QQErrorCode.INVALID_PARAMETER;
            if (e is JsonException) return QQErrorCode.JSON_ERROR;

            var webEx = e as WebException;
            if (webEx != null)
            {
                switch (webEx.Status)
                {
                    case WebExceptionStatus.Success: break;

                    case WebExceptionStatus.NameResolutionFailure: return QQErrorCode.INVALID_PARAMETER;

                    case WebExceptionStatus.ConnectFailure:
                    case WebExceptionStatus.ReceiveFailure:
                    case WebExceptionStatus.SendFailure:
                    case WebExceptionStatus.PipelineFailure:
                        return QQErrorCode.IO_ERROR;

                    case WebExceptionStatus.RequestCanceled:
                        break;
                    case WebExceptionStatus.ProtocolError:
                        break;
                    case WebExceptionStatus.ConnectionClosed:
                        break;
                    case WebExceptionStatus.TrustFailure:
                        break;
                    case WebExceptionStatus.SecureChannelFailure:
                        break;
                    case WebExceptionStatus.ServerProtocolViolation:
                        break;
                    case WebExceptionStatus.KeepAliveFailure:
                        break;
                    case WebExceptionStatus.Pending:
                        break;
                    case WebExceptionStatus.Timeout: return QQErrorCode.IO_TIMEOUT;

                    case WebExceptionStatus.ProxyNameResolutionFailure:
                        break;
                    case WebExceptionStatus.UnknownError: return QQErrorCode.UNKNOWN_ERROR;
                    case WebExceptionStatus.MessageLengthLimitExceeded:
                        break;
                    case WebExceptionStatus.CacheEntryNotFound:
                        break;
                    case WebExceptionStatus.RequestProhibitedByCachePolicy:
                        break;
                    case WebExceptionStatus.RequestProhibitedByProxy:
                        break;
                    default:
                        break;
                }
            }

            return QQErrorCode.UNKNOWN_ERROR;
        }

        public QQErrorCode ErrorCode { get; set; }

        public QQException(QQErrorCode errorCode) : base(errorCode.ToString())
        {
            ErrorCode = errorCode;
        }

        public QQException(QQErrorCode errorCode, string msg) : base(msg)
        {
            ErrorCode = errorCode;
        }

        public QQException(Exception e) : base(e.Message, e)
        {
            ErrorCode = GetErrorCode(e);
        }

        public QQException(QQErrorCode errorCode, Exception e) : base(e.Message, e)
        {
            ErrorCode = errorCode;
        }

        public override string StackTrace => base.StackTrace ?? InnerException?.StackTrace ?? string.Empty;

        public override string Message => base.Message.RegexReplace(@"[\r\n]+", string.Empty);
    }
}
