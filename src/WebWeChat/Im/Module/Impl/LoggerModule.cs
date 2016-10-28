using System;
using Microsoft.Extensions.Logging;
using Utility.Extensions;
using Utility.Logger;
using WebWeChat.Im.Core;
using WebWeChat.Im.Module.Interface;

namespace WebWeChat.Im.Module.Impl
{
    public class LoggerModule : SimpleConsoleLogger, ILoggerModule
    {
        public void Init(IWeChatContext context)
        {
            if(context == null) throw new ArgumentNullException(nameof(context));
            Context = context;
        }

        public void Destroy()
        {
        }

        public IWeChatContext Context { get; set; }

        public LoggerModule(LogLevel minLevel = LogLevel.Information) : base("WeChat", minLevel)
        {
        }

        public override string GetMessage(string message, Exception exception)
        {
            var userName = Context.GetModule<AccountModule>().User?.NickName;
            var prefix = userName.IsNullOrEmpty() ? string.Empty : $"[{userName}] ";
            return $"{DateTime.Now:HH:mm:ss}> {prefix}{message}";
        }
    }
}
