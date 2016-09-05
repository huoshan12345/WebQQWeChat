using System;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Util;
using iQQ.Net.WebQQCore.Util.Extensions;
using Microsoft.Extensions.Logging;

namespace iQQ.Net.WebQQCore.Im.Log
{
    public class QQConsoleLogger : SimpleConsoleLogger, IQQLogger
    {
        public QQConsoleLogger(string name) : base(name)
        {
        }

        public QQConsoleLogger() : this("iQQ.Net") { }

        public override string GetMessage(string message, Exception exception)
        {
            if (!message.IsNullOrEmpty() && Context?.Account != null)
            {
                var qqStr = Context.Account.QQ.IsDefault() ? string.Empty : $"[{Context.Account.QQ}] ";
                return $"{qqStr}{message}";
            }
            else return message;
        }

        public IQQContext Context { get; set; }
    }
}
