using Microsoft.Extensions.Logging;
using WebWeChat.Im.Core;
using WebWeChat.Im.Module.Interface;

namespace WebWeChat.Im.Service.Interface
{
    public interface IWeChatLogger : ILogger, IWeChatService
    {
        /// <summary>
        /// 对象上下文
        /// </summary>
        IWeChatContext Context { get; }
    }
}
