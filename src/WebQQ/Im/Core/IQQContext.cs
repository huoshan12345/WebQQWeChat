using System.Threading.Tasks;
using WebIm.Im.Core;
using WebQQ.Im.Bean;
using WebQQ.Im.Event;

namespace WebQQ.Im.Core
{
    public interface IQQContext : IImContext, IQQNotifyEventHandler
    {
        Task FireNotifyAsync(QQNotifyEvent notifyEvent);
    }
}
