using WebQQ.Im.Bean;
using WebQQ.Im.Core;

namespace WebQQ.Im.Module.Impl
{
    public class AccountModule : QQModule
    {
        public QQUser User { get; set; }

        public AccountModule(IQQContext context) : base(context)
        {
        }
    }
}
