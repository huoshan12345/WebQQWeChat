using iQQ.Net.WebQQCore.Im.Event;

namespace iQQ.Net.WebQQCore.Im
{
    public delegate void QQActionEventHandler(object sender, QQActionEvent e);

    public interface IQQActionListener
    {
        // void OnActionEvent(QQActionEvent qqActionEvent);
        event QQActionEventHandler OnActionEvent;
    }
}
