using iQQ.Net.WebQQCore.Im.Event;

namespace iQQ.Net.WebQQCore.Im
{

    public delegate void QQNotifyHandler(object sender, QQNotifyEvent e);

    /// <summary>
    /// QQ通知事件监听器
    /// </summary>
    public interface IQQNotifyListener
    {
        event QQNotifyHandler OnNotifyEvent;
    }
}
