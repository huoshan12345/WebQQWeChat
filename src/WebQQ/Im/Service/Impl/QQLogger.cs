using System;
using FclEx.Extensions;
using FclEx.Logger;
using WebQQ.Im.Core;
using WebQQ.Im.Module.Impl;
using WebQQ.Im.Service.Interface;
using Microsoft.Extensions.Logging;

namespace WebQQ.Im.Service.Impl
{
    public class QQLogger : SimpleConsoleLogger, IQQService
    {
        private readonly IQQContext _context;
        public QQLogger(IQQContext context, LogLevel minLevel = LogLevel.Information) : base("WebQQ", minLevel)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            _context = context;
        }

        /// <summary>
        /// :warning:
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        protected override string GetMessage(string message, Exception exception)
        {
            var userName = _context.GetModule<SessionModule>().User?.Uin;
            var prefix = userName.IsNullOrDefault() ? string.Empty : $"[{userName}]";
            return $"{DateTime.Now:HH:mm:ss}> {prefix}{message}";
        }
    }
}
