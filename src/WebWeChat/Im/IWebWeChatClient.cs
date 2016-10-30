using System.Threading.Tasks;
using Utility.HttpAction.Event;
using WebWeChat.Im.Core;

namespace WebWeChat.Im
{
    public interface IWebWeChatClient : IWeChatContext
    {
        Task<ActionEventType> Login(ActionEventListener listener = null);
    }
}
