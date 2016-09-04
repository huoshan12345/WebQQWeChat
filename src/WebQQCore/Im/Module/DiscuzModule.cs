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
        public IQQActionFuture GetDiscuzList(QQActionListener listener)
        {
            return PushHttpAction(new GetDiscuzListAction(this.Context, listener));
        }

        public IQQActionFuture GetDiscuzInfo(QQDiscuz discuz, QQActionListener listener)
        {
            return PushHttpAction(new GetDiscuzInfoAction(this.Context, listener, discuz));
        }
    }

}
