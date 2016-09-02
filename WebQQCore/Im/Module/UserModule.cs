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

        public IQQActionFuture GetUserInfo(QQUser user, QQActionListener listener)
        {
            return PushHttpAction(new GetFriendInfoAction(Context, listener, user));
        }

        public IQQActionFuture GetUserFace(QQUser user, QQActionListener listener)
        {
            return PushHttpAction(new GetFriendFaceAction(Context, listener, user));
        }
        
        public IQQActionFuture GetUserAccount(QQUser user, QQActionListener listener)
        {
            return PushHttpAction(new GetFriendAccoutAction(Context, listener, user));
        }

        public IQQActionFuture GetUserSign(QQUser user, QQActionListener listener)
        {
            return PushHttpAction(new GetFriendSignAction(Context, listener, user));
        }

        public IQQActionFuture GetUserLevel(QQUser user, QQActionListener listener)
        {
            return PushHttpAction(new GetUserLevelAction(Context, listener, user));
        }

        public IQQActionFuture ChangeStatus(QQStatus status, QQActionListener listener)
        {
            return PushHttpAction(new ChangeStatusAction(Context, listener, status));
        }

        public IQQActionFuture GetStrangerInfo(QQUser user, QQActionListener listener)
        {
            return PushHttpAction(new GetStrangerInfoAction(Context, listener, user));
        }

    }
}
