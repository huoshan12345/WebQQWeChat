using iQQ.Net.WebQQCore.Im.Action;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Event;

namespace iQQ.Net.WebQQCore.Im.Module
{
    /// <summary>
    /// <para>群模块</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class GroupModule : AbstractModule
    {
        public QQActionFuture GetGroupList(QQActionEventHandler listener)
        {
            return PushHttpAction(new GetGroupListAction(Context, listener));
        }

        public QQActionFuture UpdateGroupMessageFilter(QQActionEventHandler listener)
        {
            return PushHttpAction(new UpdateGroupMessageFilterAction(Context, listener));
        }

        public QQActionFuture GetGroupFace(QQGroup group, QQActionEventHandler listener)
        {
            return PushHttpAction(new GetGroupFaceAction(Context, listener, group));
        }

        public QQActionFuture GetGroupInfo(QQGroup group, QQActionEventHandler listener)
        {
            return PushHttpAction(new GetGroupInfoAction(Context, listener, group));
        }

        public QQActionFuture GetGroupGid(QQGroup group, QQActionEventHandler listener)
        {
            return PushHttpAction(new GetGroupAccoutAction(Context, listener, group));
        }

        public QQActionFuture GetMemberStatus(QQGroup group, QQActionEventHandler listener)
        {
            return PushHttpAction(new GetGroupMemberStatusAction(Context, listener, group));
        }

        public QQActionFuture SearchGroup(QQGroupSearchList resultList, QQActionEventHandler listener)
        {
            return PushHttpAction(new SearchGroupInfoAction(Context, listener, resultList));
        }
    }

}
