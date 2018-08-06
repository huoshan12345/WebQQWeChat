using System.Threading.Tasks;
using WebIm.Im.Core;
using WebQQ.Im.Bean;
using WebQQ.Im.Event;
using WebQQ.Im.Module;
using WebQQ.Im.Modules.Interface;
using WebQQ.Im.Service.Interface;

namespace WebQQ.Im.Core
{
    public interface IQQContext : IImContext
    {
        Task FireNotifyAsync(QQNotifyEvent notifyEvent);
    }
}
