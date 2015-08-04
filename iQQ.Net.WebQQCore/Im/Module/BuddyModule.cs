using iQQ.Net.WebQQCore.Im.Action;
using iQQ.Net.WebQQCore.Im.Event;

namespace iQQ.Net.WebQQCore.Im.Module
{
    /// <summary>
    /// <para>好友信息处理模块</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class BuddyModule : AbstractModule
    {
        public QQActionFuture GetOnlineBuddy(QQActionEventHandler listener)
        {
            return PushHttpAction(new GetOnlineFriendAction(this.Context, listener));
        }

        public QQActionFuture GetRecentList(QQActionEventHandler listener)
        {
            return PushHttpAction(new GetRecentListAction(this.Context, listener));
        }

        public QQActionFuture AddBuddy(QQActionEventHandler listener, string account)
        {
            return PushHttpAction(new AcceptBuddyAddAction(Context, listener, account));
        }
    }

}
