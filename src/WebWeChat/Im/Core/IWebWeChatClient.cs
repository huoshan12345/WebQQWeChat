using System;
using System.Threading.Tasks;
using Utility.HttpAction.Event;
using WebWeChat.Im.Module.Interface;

namespace WebWeChat.Im.Core
{
    public interface IWebWeChatClient : IDisposable, IWeChatContext, ILoginModule, IContactModule
    {

    }
}
