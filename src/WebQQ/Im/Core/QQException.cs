using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FclEx.Extensions;
using Newtonsoft.Json;

namespace WebQQ.Im.Core
{
    public class QQException : Exception
    {
        private static readonly ConcurrentDictionary<QQErrorCode, QQException> Exceptions
            = new ConcurrentDictionary<QQErrorCode, QQException>();

        private static QQErrorCode GetErrorCode(Exception e)
        {
            e = e.InnerException ?? e;

            if (e is TimeoutException) return QQErrorCode.Timeout;
            if (e is IOException) return QQErrorCode.IoError;
            if (e is ArgumentException) return QQErrorCode.ParameterError;
            if (e is JsonException) return QQErrorCode.JsonError;
            if(e is TaskCanceledException te && !te.CancellationToken.IsCancellationRequested) return QQErrorCode.Timeout;
            
            return QQErrorCode.UnknownError;
        }

        public QQErrorCode ErrorCode { get; set; }

        private readonly string _stackTrace;

        public static QQException CreateException(QQErrorCode errorCode)
        {
            return Exceptions.GetOrAdd(errorCode, key => new QQException(errorCode, ""));
        }

        public static QQException CreateException(QQErrorCode errorCode, string msg)
        {
            if (msg.IsNullOrEmpty()) return CreateException(errorCode);
            return new QQException(errorCode, msg);
        }

        public QQException(QQErrorCode errorCode, string msg) : base(msg)
        {
            ErrorCode = errorCode;
        }

        public QQException(QQErrorCode errorCode) : this(errorCode, errorCode.GetDescription())
        {
            ErrorCode = errorCode;
        }

        public QQException(Exception e) : base(e.Message)
        {
            ErrorCode = GetErrorCode(e);
            _stackTrace = e.StackTrace;
        }

        public QQException(QQErrorCode errorCode, Exception e) : base(e.Message)
        {
            ErrorCode = errorCode;
            _stackTrace = e.StackTrace;
        }

        public override string StackTrace => base.StackTrace ?? _stackTrace;

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
