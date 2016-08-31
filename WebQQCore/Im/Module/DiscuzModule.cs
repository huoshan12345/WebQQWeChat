using iQQ.Net.WebQQCore.Im.Action;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;

namespace iQQ.Net.WebQQCore.Im.Module
{
    /// <summary>
    /// <para>讨论组模块</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class DiscuzModule : AbstractModule
    {
        public override QQModuleType GetModuleType()
        {
            return QQModuleType.DISCUZ;
        }

        public IQQActionFuture GetDiscuzList(QQActionEventHandler listener)
        {
            return PushHttpAction(new GetDiscuzListAction(this.Context, listener));
        }

        public IQQActionFuture GetDiscuzInfo(QQDiscuz discuz, QQActionEventHandler listener)
        {
            return PushHttpAction(new GetDiscuzInfoAction(this.Context, listener, discuz));
        }
    }

}
