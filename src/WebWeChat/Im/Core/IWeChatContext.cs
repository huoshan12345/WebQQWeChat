using System;
using System.Threading.Tasks;
using WebWeChat.Im.Event;
using WebWeChat.Im.Module;
using WebWeChat.Im.Module.Interface;
using WebWeChat.Im.Service.Interface;

namespace WebWeChat.Im.Core
{
    public interface IWeChatContext
    {
        Task FireNotifyAsync(WeChatNotifyEvent notifyEvent);

        T GetSerivce<T>();

        T GetModule<T>() where T : IWeChatModule;
    }
}
