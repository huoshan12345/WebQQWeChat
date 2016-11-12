using System;
using FxUtility.Logger;
using Microsoft.Extensions.Logging;
using WebWeChat.Im.Core;
using WebWeChat.Im.Module.Impl;
using WebWeChat.Im.Service.Interface;
using FxUtility.Extensions;

namespace WebWeChat.Im.Service.Impl
{
    public class WeChatLogger : SimpleConsoleLogger, IWeChatService
    {
        public IWeChatContext Context { get; set; }

        public WeChatLogger(IWeChatContext context, LogLevel minLevel = LogLevel.Information) : base("WeChat", minLevel)
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
            var userName = Context.GetModule<SessionModule>().User?.NickName;
            var prefix = userName.IsNullOrEmpty() ? string.Empty : $"[{userName}]";
            return $"{DateTime.Now:HH:mm:ss}> {prefix}{message}";
        }
    }
}
