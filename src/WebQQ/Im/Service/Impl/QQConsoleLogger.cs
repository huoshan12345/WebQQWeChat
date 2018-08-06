using System;
using FclEx.Extensions;
using FclEx.Logger;
using WebQQ.Im.Core;
using WebQQ.Im.Service.Interface;
using Microsoft.Extensions.Logging;
using WebQQ.Im.Modules.Impl;

namespace WebQQ.Im.Service.Impl
{
    public class QQConsoleLogger : SimpleConsoleLogger, IQQService
    {
        public IQQContext Context { get; }

        public QQConsoleLogger(IQQContext context, LogLevel minLevel = LogLevel.Information) : base("WebQQ", minLevel)
        {
            Context = context;
        }
        
        protected override string GetMessage(string message, Exception exception)
        {
            var userName = Context?.GetModule<SessionModule>().User?.Uin;
            var prefix = userName.IsNullOrDefault() ? string.Empty : $"[{userName}]";
            return $"{DateTime.Now:HH:mm:ss}> {prefix}{message}";
        }

        public void Dispose()
        {
        }
    }
}
