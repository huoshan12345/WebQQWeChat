using Microsoft.Extensions.Logging;
using WebWeChat.Im.Module.Interface;

namespace WebWeChat.Im.Service.Interface
{
    public interface IWeChatLogger : ILogger, IWeChatService
    {
    }
}
