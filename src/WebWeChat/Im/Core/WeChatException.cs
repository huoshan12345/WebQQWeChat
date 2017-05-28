using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using FclEx.Extensions;

namespace WebWeChat.Im.Core
{
    public class WeChatException : Exception
    {
        private static readonly ConcurrentDictionary<WeChatErrorCode, WeChatException> Exceptions 
            = new ConcurrentDictionary<WeChatErrorCode, WeChatException>();

        private static WeChatErrorCode GetErrorCode(Exception e)
        {
            e = e.InnerException ?? e;

            if (e is TimeoutException) return WeChatErrorCode.Timeout;
            if (e is IOException) return WeChatErrorCode.IoError;
            if (e is ArgumentException) return WeChatErrorCode.ParameterError;
            if (e is JsonException) return WeChatErrorCode.JsonError;

            return WeChatErrorCode.UnknownError;
        }

        public WeChatErrorCode ErrorCode { get; set; }

        public static WeChatException CreateException(WeChatErrorCode errorCode)
        {
            return Exceptions.GetOrAdd(errorCode, key => new WeChatException(errorCode, ""));
        }

        public static WeChatException CreateException(WeChatErrorCode errorCode, string msg)
        {
            if (msg.IsNullOrEmpty()) return CreateException(errorCode);
            return new WeChatException(errorCode, msg);
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
            var msg = new StringBuilder($"ErrorCode={ErrorCode}, ErrorMsg={this.GetAllMessages()}, StackTrace=");
            msg.AppendLineIf($"{Environment.NewLine}{StackTrace}", StackTrace != null);
            return msg.ToString();
        }

        public string ToSimpleString()
        {
            return $"ErrorCode={ErrorCode}, ErrorMsg={Message}";
        }
    }
}
