using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Event;

namespace iQQ.Net.WebQQCore.Im.Core
{
    public interface IQQContext
    {
        void FireNotify(QQNotifyEvent qqNotifyEvent);

        T GetModule<T>(QQModuleType type) where T : IQQModule;

        T GetSerivce<T>(QQServiceType type) where T : IQQService;

        QQAccount Account { get; set; }

        QQSession Session { get; set; }

        QQStore Store { get; set; }
    }
}
