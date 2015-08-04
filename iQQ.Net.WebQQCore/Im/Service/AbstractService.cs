using iQQ.Net.WebQQCore.Im.Core;

namespace iQQ.Net.WebQQCore.Im.Service
{
    /// <summary>
    /// 抽象的服务类，实现了部分接口，方便子类实现
    /// </summary>
    public abstract class AbstractService : IQQService
    {
        public QQContext Context { get; private set; }

        public virtual void Init(QQContext context)
        {
            Context = context;
        }

        public abstract void Destroy();
    }
}
