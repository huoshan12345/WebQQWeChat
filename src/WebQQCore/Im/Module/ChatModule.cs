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

        private IQQActionFuture DoSendMsg(QQMsg msg, QQActionListener listener)
        {
            return PushHttpAction(new SendMsgAction(Context, listener, msg));
        }

        public IQQActionFuture GetRobotReply(QQMsg input, RobotType robotType, QQActionListener listener)
        {
            return PushHttpAction(new GetTuringRobotReplyAction(Context, listener, input));
        }

        public IQQActionFuture SendMsg(QQMsg msg, QQActionListener listener)
        {
            var future = new ProcActionFuture(listener, true);

            if (msg.Type == QQMsgType.SESSION_MSG)
            {
                var stranger = (QQStranger)msg.To;
                if (string.IsNullOrEmpty(stranger.GroupSig))
                {
                    GetSessionMsgSig(stranger, (sender, Event) =>
                    {
                        if (Event.Type == QQActionEventType.EvtOK)
                        {
                            DoSendMsg(msg, future.Listener);
                        }
                        else if (Event.Type == QQActionEventType.EvtError)
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
                        return future;
                    }
                }
            }
            return DoSendMsg(msg, listener);
        }

        public IQQActionFuture GetSessionMsgSig(QQStranger user, QQActionListener listener)
        {
            return PushHttpAction(new GetSessionMsgSigAction(Context, listener, user));
        }

        public IQQActionFuture UploadOffPic(QQUser user, string file, QQActionListener listener)
        {
            return PushHttpAction(new UploadOfflinePictureAction(Context, listener, user, file));
        }

        public IQQActionFuture UploadCFace(string file, QQActionListener listener)
        {
            return PushHttpAction(new UploadCustomFaceAction(Context, listener, file));
        }

        public IQQActionFuture GetCFaceSig(QQActionListener listener)
        {
            return PushHttpAction(new GetCustomFaceSigAction(Context, listener));
        }

        public IQQActionFuture SendShake(QQUser user, QQActionListener listener)
        {
            return PushHttpAction(new ShakeWindowAction(Context, listener, user));
        }

        public IQQActionFuture GetOffPic(OffPicItem offpic, QQMsg msg, Stream picout, QQActionListener listener)
        {
            return PushHttpAction(new GetOffPicAction(Context, listener, offpic, msg, picout));
        }

        public IQQActionFuture GetUserPic(CFaceItem cface, QQMsg msg, Stream picout, QQActionListener listener)
        {
            return PushHttpAction(new GetUserPicAction(Context, listener, cface, msg, picout));
        }

        public IQQActionFuture GetGroupPic(CFaceItem cface, QQMsg msg, Stream picout, QQActionListener listener)
        {
            return PushHttpAction(new GetGroupPicAction(Context, listener, cface, msg, picout));
        }

        public IQQActionFuture SendInputNotify(QQUser user, QQActionListener listener)
        {
            return PushHttpAction(new SendInputNotifyAction(Context, listener, user));
        }
    }
}
