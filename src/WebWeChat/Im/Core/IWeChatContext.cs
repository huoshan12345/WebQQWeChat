namespace WebWeChat.Im.Core
{
    public interface IWeChatContext
    {        

void FireNotify(QQNotifyEvent qqNotifyEvent);
        T GetSerivce<T>();
    }
}
