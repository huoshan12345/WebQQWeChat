using System;
using HttpActionTools.Extensions;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore.Im.Service.Log
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

        public void Init(IQQContext context){}

        public void Destroy(){}
    }
}
