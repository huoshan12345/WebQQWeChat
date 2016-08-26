using iQQ.Net.WebQQCore.Im.Action;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;

namespace iQQ.Net.WebQQCore.Im.Module
{
    /// <summary>
    /// 个人信息模块
    /// </summary>
    public class UserModule : AbstractModule
    {
        public override QQModuleType GetModuleType()
        {
            return QQModuleType.USER;
        }

        public QQActionFuture GetUserInfo(QQUser user, QQActionEventHandler listener)
        {
            return PushHttpAction(new GetFriendInfoAction(Context, listener, user));
        }

        public QQActionFuture GetUserFace(QQUser user, QQActionEventHandler listener)
        {
            return PushHttpAction(new GetFriendFaceAction(Context, listener, user));
        }
        
        public QQActionFuture GetUserAccount(QQUser user, QQActionEventHandler listener)
        {
            return PushHttpAction(new GetFriendAccoutAction(Context, listener, user));
        }

        public QQActionFuture GetUserSign(QQUser user, QQActionEventHandler listener)
        {
            return PushHttpAction(new GetFriendSignAction(Context, listener, user));
        }

        public QQActionFuture GetUserLevel(QQUser user, QQActionEventHandler listener)
        {
            return PushHttpAction(new GetUserLevelAction(Context, listener, user));
        }

        public QQActionFuture ChangeStatus(QQStatus status, QQActionEventHandler listener)
        {
            return PushHttpAction(new ChangeStatusAction(Context, listener, status));
        }

        public QQActionFuture GetStrangerInfo(QQUser user, QQActionEventHandler listener)
        {
            return PushHttpAction(new GetStrangerInfoAction(Context, listener, user));
        }

    }
}
