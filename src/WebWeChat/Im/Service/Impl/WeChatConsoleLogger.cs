using System;
using Microsoft.Extensions.Logging;
using WebWeChat.Im.Core;
using WebWeChat.Im.Module.Impl;
using WebWeChat.Im.Service.Interface;
using FclEx.Extensions;
using FclEx.Logger;

namespace WebWeChat.Im.Service.Impl
{
    public class WeChatConsoleLogger : SimpleConsoleLogger, IWeChatService
    {
        public IWeChatContext Context { get; }

        public WeChatConsoleLogger(IWeChatContext context, LogLevel minLevel = LogLevel.Information) : base("WeChat", minLevel)
        {
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
            var userName = Context?.GetModule<SessionModule>().User?.NickName;
            var prefix = userName.IsNullOrEmpty() ? string.Empty : $"[{userName}]";
            return $"{DateTime.Now:HH:mm:ss}> {prefix}{message}";
        }

        public void Dispose()
        {
        }
    }
}
