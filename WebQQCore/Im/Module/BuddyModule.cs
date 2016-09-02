using iQQ.Net.WebQQCore.Im.Action;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;

namespace iQQ.Net.WebQQCore.Im.Module
{
    /// <summary>
    /// <para>好友信息处理模块</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class BuddyModule : AbstractModule
    {
        public IQQActionFuture GetOnlineBuddy(QQActionListener listener)
        {
            return PushHttpAction(new GetOnlineFriendAction(this.Context, listener));
        }

        public IQQActionFuture GetRecentList(QQActionListener listener)
        {
            return PushHttpAction(new GetRecentListAction(this.Context, listener));
        }

        public IQQActionFuture AddBuddy(QQActionListener listener, string account)
        {
            return PushHttpAction(new AcceptBuddyAddAction(Context, listener, account));
        }

        public override QQModuleType GetModuleType()
        {
            return QQModuleType.BUDDY;
        }
    }

}
