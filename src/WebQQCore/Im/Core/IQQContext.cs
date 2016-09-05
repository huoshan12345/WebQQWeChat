using iQQ.Net.WebQQCore.Im.Actor;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Log;

namespace iQQ.Net.WebQQCore.Im.Core
{
    /// <summary>
    /// QQ环境上下文，所有的模块都是用QQContext来获取对象
    /// </summary>
    public interface IQQContext
    {
        IQQLogger Logger { get; }

        void PushActor(IQQActor actor);

        void FireNotify(QQNotifyEvent qqNotifyEvent);

        T GetModule<T>(QQModuleType type) where T : IQQModule;

        T GetSerivce<T>(QQServiceType type) where T:IQQService;

        QQAccount Account { get; set; }
 
        QQSession Session { get; set; }

        QQStore Store { get; set; }
    }
}
