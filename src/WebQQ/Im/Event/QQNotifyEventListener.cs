using System.Threading.Tasks;
using WebQQ.Im.Core;

namespace WebQQ.Im.Event
{
    public delegate Task QQNotifyEventListener(IQQClient sender, QQNotifyEvent e);

    public interface IQQNotifyEventHandler
    {
        event QQNotifyEventListener QQNotifyEvent;
    }
}
