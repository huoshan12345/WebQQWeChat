using iQQ.Net.WebQQCore.Util;
using System;
using System.IO;
using System.Net;
using System.Text;
using iQQ.Net.WebQQCore.Util.Extensions;
using Newtonsoft.Json;

namespace iQQ.Net.WebQQCore.Im
{
    

    public class QQException : Exception
    {
        private static QQErrorCode GetErrorCode(Exception e)
        {
            if (e is TimeoutException) return QQErrorCode.IOTimeout;
            if (e is IOException) return QQErrorCode.IOError;
            if (e is ArgumentException) return QQErrorCode.InvalidParameter;
            if (e is JsonException) return QQErrorCode.JsonError;

            var webEx = e as WebException;
            if (webEx != null)
            {
                switch (webEx.Status)
                {
                    case WebExceptionStatus.Success: break;

                    case WebExceptionStatus.NameResolutionFailure: return QQErrorCode.InvalidParameter;

                    case WebExceptionStatus.ConnectFailure:
                    case WebExceptionStatus.ReceiveFailure:
                    case WebExceptionStatus.SendFailure:
                    case WebExceptionStatus.PipelineFailure:
                        return QQErrorCode.IOError;

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
                    case WebExceptionStatus.Timeout: return QQErrorCode.IOTimeout;

                    case WebExceptionStatus.ProxyNameResolutionFailure:
                        break;
                    case WebExceptionStatus.UnknownError: return QQErrorCode.UnknownError;
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

            return QQErrorCode.UnknownError;
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
