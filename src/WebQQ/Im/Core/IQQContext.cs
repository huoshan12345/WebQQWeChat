using System.Threading.Tasks;
using WebQQ.Im.Bean;
using WebQQ.Im.Event;
using WebQQ.Im.Module;
using WebQQ.Im.Module.Interface;
using WebQQ.Im.Service.Interface;

namespace WebQQ.Im.Core
{
    public interface IQQContext
    {
        T GetModule<T>() where T : IQQModule;

        // 获取服务，该服务既可以是全局共享的，也可以是qq实例特有的，所以不要求T继承IQQService
        T GetSerivce<T>();

        void FireNotify(QQNotifyEvent notifyEvent);

        Task FireNotifyAsync(QQNotifyEvent notifyEvent);
    }
}
