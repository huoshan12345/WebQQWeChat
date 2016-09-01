using System;
using iQQ.Net.WebQQCore.Im.Event;

namespace iQQ.Net.WebQQCore.Im
{
    // 强类型的委托
    public delegate void QQNotifyHandler(IQQClient sender, QQNotifyEvent e);

    /// <summary>
    /// QQ通知事件监听器
    /// </summary>
    public interface IQQNotifyListener
    {
        // event QQNotifyHandler OnNotifyEvent;
        event EventHandler<QQNotifyEvent> OnNotifyEvent;
    }
}
