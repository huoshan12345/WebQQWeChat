using System;
using FclEx.Extensions;
using FclEx.Logger;
using WebQQ.Im.Core;
using WebQQ.Im.Module.Impl;
using WebQQ.Im.Service.Interface;
using Microsoft.Extensions.Logging;

namespace WebQQ.Im.Service.Impl
{
    public class QQConsoleLogger : SimpleConsoleLogger, IQQService
    {
        public IQQContext Context { get; set; }

        public QQConsoleLogger(LogLevel minLevel = LogLevel.Information) : base("WebQQ", minLevel)
        {
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
