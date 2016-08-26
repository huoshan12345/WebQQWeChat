using System.IO;
using iQQ.Net.WebQQCore.Im.Action;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Bean.Content;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Event.Future;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore.Im.Module
{
    /// <summary>
    /// <para>消息处理</para>
    /// <para>@author ChenZhiHui</para>
    /// <para>@since 2013-2-25</para>
    /// </summary>
    public class ChatModule : AbstractModule
    {
        public override QQModuleType GetModuleType()
        {
            return QQModuleType.CHAT;
        }

        private QQActionFuture DoSendMsg(QQMsg msg, QQActionEventHandler listener)
        {
            return PushHttpAction(new SendMsgAction(Context, listener, msg));
        }

        public QQActionFuture GetRobotReply(QQMsg input, RobotType robotType, QQActionEventHandler listener)
        {
            return PushHttpAction(new GetRobotReplyAction(Context, listener, input, robotType));
        }

        public QQActionFuture SendMsg(QQMsg msg, QQActionEventHandler listener)
        {
            var future = new ProcActionFuture(listener, true);

            if (msg.Type == QQMsgType.SESSION_MSG)
            {
                var stranger = (QQStranger)msg.To;
                if (string.IsNullOrEmpty(stranger.GroupSig))
                {
                    GetSessionMsgSig(stranger, (sender, Event) =>
                    {
                        if (Event.Type == QQActionEventType.EVT_OK)
                        {
                            if (!future.IsCanceled)
                            {
                                DoSendMsg(msg, future.Listener);
                            }
                        }
                        else if (Event.Type == QQActionEventType.EVT_ERROR)
                        {
                            future.NotifyActionEvent(Event.Type, Event.Target);
                        }
                    });
                }
                return future;
            }
            else if (msg.Type == QQMsgType.GROUP_MSG || msg.Type == QQMsgType.DISCUZ_MSG)
            {
                if (msg.Type == QQMsgType.GROUP_MSG)
                {
                    if (msg.Group.Gin == 0)
                    {
                        msg.Group = Context.Store.GetGroupByCode(msg.Group.Code);
                        if (msg == null)
                        {
                            // update group list
                            // sendMsg(msg, future);
                        }
                        return future;
                    }
                }
            }
            return DoSendMsg(msg, listener);
        }

        public QQActionFuture GetSessionMsgSig(QQStranger user, QQActionEventHandler listener)
        {
            return PushHttpAction(new GetSessionMsgSigAction(this.Context, listener, user));
        }

        public QQActionFuture UploadOffPic(QQUser user, string file, QQActionEventHandler listener)
        {
            return PushHttpAction(new UploadOfflinePictureAction(this.Context, listener, user, file));
        }

        public QQActionFuture UploadCFace(string file, QQActionEventHandler listener)
        {
            return PushHttpAction(new UploadCustomFaceAction(this.Context, listener, file));
        }

        public QQActionFuture GetCFaceSig(QQActionEventHandler listener)
        {
            return PushHttpAction(new GetCustomFaceSigAction(this.Context, listener));
        }

        public QQActionFuture SendShake(QQUser user, QQActionEventHandler listener)
        {
            return PushHttpAction(new ShakeWindowAction(this.Context, listener, user));
        }

        public QQActionFuture GetOffPic(OffPicItem offpic, QQMsg msg, Stream picout, QQActionEventHandler listener)
        {
            return PushHttpAction(new GetOffPicAction(this.Context, listener, offpic, msg, picout));
        }

        public QQActionFuture GetUserPic(CFaceItem cface, QQMsg msg, Stream picout, QQActionEventHandler listener)
        {
            return PushHttpAction(new GetUserPicAction(this.Context, listener, cface, msg, picout));
        }

        public QQActionFuture GetGroupPic(CFaceItem cface, QQMsg msg, Stream picout, QQActionEventHandler listener)
        {
            return PushHttpAction(new GetGroupPicAction(this.Context, listener, cface, msg, picout));
        }

        public QQActionFuture SendInputNotify(QQUser user, QQActionEventHandler listener)
        {
            return PushHttpAction(new SendInputNotifyAction(this.Context, listener, user));
        }
    }
}
