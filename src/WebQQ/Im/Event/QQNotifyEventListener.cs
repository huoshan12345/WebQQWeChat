using System.Threading.Tasks;
using FclEx.Utils;
using WebQQ.Im.Core;

namespace WebQQ.Im.Event
{
    public interface IQQNotifyEventHandler
    {
        event AsyncEventHandler<IQQClient, QQNotifyEvent> OnQQNotifyEvent;
    }
}
