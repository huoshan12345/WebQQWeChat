using WebQQ.Im.Core;

namespace WebQQ.Im.Event
{
    public delegate void QQNotifyEventListener(IQQClient sender, QQNotifyEvent e);

    public interface IQQNotifyEventHandler
    {
        event QQNotifyEventListener QQNotifyEvent;
    }
}
