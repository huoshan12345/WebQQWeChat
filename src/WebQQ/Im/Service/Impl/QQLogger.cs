using System;
using FxUtility.Extensions;
using FxUtility.Logger;
using WebQQ.Im.Core;
using WebQQ.Im.Module.Impl;
using WebQQ.Im.Service.Interface;
using Microsoft.Extensions.Logging;

namespace WebQQ.Im.Service.Impl
{
    public class QQLogger : SimpleConsoleLogger, IQQService
    {
        public QQLogger(IQQContext context, LogLevel minLevel = LogLevel.Information) : base("WebQQ")
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            Context = context;
        }

        /// <summary>
        /// :warning:
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        protected override string GetMessage(string message, Exception exception)
        {
            var userName = Context.GetModule<AccountModule>().User?.QQ;
            var prefix = userName.IsDefault() ? string.Empty : $"[{userName}]";
            return $"{DateTime.Now:HH:mm:ss}> {prefix}{message}";
        }

        public IQQContext Context { get; set; }
    }
}
