using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Utility.Extensions;

namespace WebWeChat.Im
{
    public class WeChatException : Exception
    {
        private static WeChatErrorCode GetErrorCode(Exception e)
        {
            if (e is TimeoutException) return WeChatErrorCode.Timeout;
            if (e is IOException) return WeChatErrorCode.IoError;
            if (e is ArgumentException) return WeChatErrorCode.ParameterError;
            if (e is JsonException) return WeChatErrorCode.JsonError;

            var webEx = e as WebException;
            if (webEx != null)
            {
                switch (webEx.Status)
                {
                    case WebExceptionStatus.Success: break;

                    case WebExceptionStatus.NameResolutionFailure: return WeChatErrorCode.ParameterError;

                    case WebExceptionStatus.ConnectFailure:
                    case WebExceptionStatus.ReceiveFailure:
                    case WebExceptionStatus.SendFailure:
                    case WebExceptionStatus.PipelineFailure:
                        return WeChatErrorCode.IoError;

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
                    case WebExceptionStatus.Timeout: return WeChatErrorCode.Timeout;

                    case WebExceptionStatus.ProxyNameResolutionFailure:
                        break;
                    case WebExceptionStatus.UnknownError: return WeChatErrorCode.UnknownError;
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

            return WeChatErrorCode.UnknownError;
        }

        public WeChatErrorCode ErrorCode { get; set; }

        public WeChatException(WeChatErrorCode errorCode) : base(errorCode.ToString())
        {
            ErrorCode = errorCode;
        }

        public WeChatException(WeChatErrorCode errorCode, string msg) : base(msg)
        {
            ErrorCode = errorCode;
        }

        public WeChatException(Exception e) : base(e.Message, e)
        {
            ErrorCode = GetErrorCode(e);
        }

        public WeChatException(WeChatErrorCode errorCode, Exception e) : base(e.Message, e)
        {
            ErrorCode = errorCode;
        }

        public override string StackTrace => base.StackTrace ?? InnerException?.StackTrace;

        public override string Message => base.Message.RegexReplace(@"[\r\n]+", string.Empty);

        public override string ToString()
        {
            var msg = new StringBuilder($"ErrorCode={ErrorCode}, ErrorMsg={Message}, StackTrace=");
            msg.AppendLineIf($"{Environment.NewLine}{StackTrace}", StackTrace != null);
            return msg.ToString();
        }

        public string ToSimpleString()
        {
            return $"ErrorCode={ErrorCode}, ErrorMsg={Message}";
        }
    }
}
