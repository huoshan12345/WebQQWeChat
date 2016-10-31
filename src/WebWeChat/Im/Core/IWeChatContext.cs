using System;
using WebWeChat.Im.Event;
using WebWeChat.Im.Module;
using WebWeChat.Im.Module.Interface;

namespace WebWeChat.Im.Core
{
    public interface IWeChatContext
    {
        void FireNotify(WeChatNotifyEvent notifyEvent);

        T GetSerivce<T>();

        T GetModule<T>() where T : IWeChatModule;
    }
}
