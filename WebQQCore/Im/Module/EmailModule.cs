using System.Collections.Generic;
using iQQ.Net.WebQQCore.Im.Action;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore.Im.Module
{
    /// <summary>
    /// <para>邮箱模块</para>
    /// <para>@author 承∮诺</para>
    /// <para>@since 2014年1月23日</para>
    /// </summary>
    public class EmailModule : AbstractModule
    {
        public override QQModuleType GetModuleType()
        {
            return QQModuleType.EMAIL;
        }

        public int ErrorCount { get; set; }

        /// <summary>
        /// 轮询邮件信息 先通过pt4获取check url 再通过check检查登录验证 再通过login获取key2 
        /// 再通过wpkey获取key3 然后得到wpkey，进行邮件轮询
        /// </summary>
        internal void DoPoll()
        {
            // 步骤四
            QQActionEventHandler wpkeyListener = (sender, Event) =>
            {
                if (Event.Type == QQActionEventType.EVT_OK)
                {
                    ErrorCount = 0;
                    // 跳到轮询
                    LoopPoll(Event.Target.ToString(), 0);
                }
                else if (Event.Type == QQActionEventType.EVT_ERROR)
                {
                    while (ErrorCount < QQConstants.MAX_POLL_ERR_CNT)
                    {
                        if (GetPT4Auth(null).WaitFinalEvent(3000).Type != QQActionEventType.EVT_OK)
                        {
                            ErrorCount++;
                        }
                        else
                        {
                            ErrorCount = 0;
                            // 跳到轮询
                            LoopPoll(Event.Target.ToString(), 0);
                        }
                    }
                }
            };

            // 步骤三
            QQActionEventHandler loginListener = (sender, Event) =>
            {
                if (Event.Type == QQActionEventType.EVT_OK)
                {
                    ErrorCount = 0;
                    // 跳到步骤四
                    var key = Event.Target.ToString();
                    GetWPKey(key, wpkeyListener);
                    Context.Session.EmailAuthKey = key;
                }
                else if (Event.Type == QQActionEventType.EVT_ERROR)
                {
                    while (ErrorCount < QQConstants.MAX_POLL_ERR_CNT)
                    {
                        if (GetPT4Auth(null).WaitFinalEvent(3000).Type != QQActionEventType.EVT_OK)
                        {
                            ErrorCount++;
                        }
                        else
                        {
                            ErrorCount = 0;
                            // 跳到步骤四
                            var key = Event.Target.ToString();
                            GetWPKey(key, wpkeyListener);
                            Context.Session.EmailAuthKey = key;
                        }
                    }
                }
            };

            // 步骤二
            QQActionEventHandler checkListener = (sender, Event) =>
            {
                if (Event.Type == QQActionEventType.EVT_OK)
                {
                    ErrorCount = 0;
                    // 跳到步骤三
                    Login(loginListener);
                }
                else if (Event.Type == QQActionEventType.EVT_ERROR)
                {
                    while (ErrorCount < QQConstants.MAX_POLL_ERR_CNT)
                    {
                        if (GetPT4Auth(null).WaitFinalEvent(3000).Type != QQActionEventType.EVT_OK)
                        {
                            ErrorCount++;
                        }
                        else
                        {
                            ErrorCount = 0;
                            // 跳到步骤三
                            Login(loginListener);
                        }
                    }
                }
            };

            // 步骤一
            QQActionEventHandler pt4Listener = (sender, Event) =>
            {
                if (Event.Type == QQActionEventType.EVT_OK)
                {
                    ErrorCount = 0;
                    // 跳到步骤二
                    Check(Event.Target.ToString(), checkListener);
                }
                else if (Event.Type == QQActionEventType.EVT_ERROR)
                {
                    while (ErrorCount < QQConstants.MAX_POLL_ERR_CNT)
                    {
                        if (GetPT4Auth(null).WaitFinalEvent(3000).Type != QQActionEventType.EVT_OK)
                        {
                            var ex = (QQException)Event.Target;
                            if (ex.ErrorCode == QQErrorCode.INVALID_LOGIN_AUTH)
                            {
                                // 登录失败，QQ消息的POLL同时也失效，这时那边会重新登录
                                // 如果已经在登录中，或者已经登录了，就不用再次执行
                                DefaultLogger.Warn("GetPT4Auth error!!! wait Relogin...", ex);
                                var session = Context.Session;
                                if (session.State == QQSessionState.LOGINING
                                    || session.State == QQSessionState.KICKED) return;

                                var procModule = Context.GetModule<ProcModule>(QQModuleType.PROC);
                                procModule.Relogin();// 重新登录成功会重新唤醒beginPoll
                            }
                            else if (ErrorCount < QQConstants.MAX_POLL_ERR_CNT)
                            {
                                ErrorCount++;
                            }
                        }
                        else
                        {
                            ErrorCount = 0;
                            // 跳到步骤二
                            Check(Event.Target.ToString(), checkListener);
                        }
                    }
                }
            };

            GetPT4Auth(pt4Listener);
        }

        /// <summary>
        /// 反复轮询
        /// </summary>
        /// <param name="sid">凭证ID，就算没有Cookie都可以轮询</param>
        /// <param name="t">邮件，好像是时间，用来消除轮询返回的列表中邮件，不然一直会返回那个邮件回来</param>
        private void LoopPoll(string sid, long t)
        {
            Poll(sid, t, (sender, Event) =>
            {
                if (Event.Type == QQActionEventType.EVT_OK)
                {
                    ErrorCount = 0;
                    if (Event.Target == null)
                    {
                        // 没有新邮件，t直接传0
                        LoopPoll(sid, 0);
                    }
                    else
                    {
                        // 有新邮件
                        var evt = (QQNotifyEvent)Event.Target;
                        // 通知事件
                        Context.FireNotify(evt);
                        // 消除所有，传上最后t的标记上去
                        var mailList = (List<QQEmail>)evt.Target;
                        LoopPoll(sid, mailList[mailList.Count - 1].Flag);

                        // 把邮件标记为已读，需要邮件列表ID
                        // mark(false, mailList, null);
                    }
                }
                else if (Event.Type == QQActionEventType.EVT_ERROR)
                {
                    var ex = (QQException)Event.Target;
                    if (ex.ErrorCode == QQErrorCode.INVALID_LOGIN_AUTH)
                    {
                        // 凭证失效，重新认证
                        DoPoll();
                    }
                    else if (ErrorCount < QQConstants.MAX_POLL_ERR_CNT)
                    {
                        LoopPoll(sid, 0);
                        ErrorCount++;
                    }
                }
            });
        }

        /**
         * 轮询，需要到sid
         *
         * @param sid a {@link java.lang.String} object.
         * @param t a long.
         * @param listener a {@link iqq.im.IQQActionListener} object.
         * @return a {@link iqq.im.Event.QQActionFuture} object.
         */
        private IQQActionFuture Poll(string sid, long t, QQActionEventHandler listener)
        {
            return PushHttpAction(new PollEmailAction(sid, t, Context, listener));
        }

        /**
         * 通过登录得到的sid，获取到wpkey
         *
         * @param sid a {@link java.lang.String} object.
         * @param listener a {@link iqq.im.IQQActionListener} object.
         * @return a {@link iqq.im.Event.QQActionFuture} object.
         */
        private IQQActionFuture GetWPKey(string sid, QQActionEventHandler listener)
        {
            return PushHttpAction(new GetWPKeyAction(sid, Context, listener));
        }

        /**
         * 登录邮箱
         *
         * @param listener a {@link iqq.im.IQQActionListener} object.
         * @return a {@link iqq.im.Event.QQActionFuture} object.
         */
        private IQQActionFuture Login(QQActionEventHandler listener)
        {
            return PushHttpAction(new LoginEmailAction(Context, listener));
        }

        /**
         * 通过pt4获取到的URL进行封装 检测邮箱是否合法登录了
         *
         * @param url a {@link java.lang.String} object.
         * @param listener a {@link iqq.im.IQQActionListener} object.
         * @return a {@link iqq.im.Event.QQActionFuture} object.
         */
        private IQQActionFuture Check(string url, QQActionEventHandler listener)
        {
            return PushHttpAction(new CheckEmailSig(url, Context, listener));
        }

        /**
         * pt4登录验证 获取到一个链接
         *
         * @param listener a {@link iqq.im.IQQActionListener} object.
         * @return a {@link iqq.im.Event.QQActionFuture} object.
         */
        private IQQActionFuture GetPT4Auth(QQActionEventHandler listener)
        {
            return PushHttpAction(new GetPT4Auth(Context, listener));
        }

        /**
         * 把邮件标记为已经读，或者未读
         *
         * @param unread a bool.
         * @param mails a {@link java.util.List} object.
         * @param listener a {@link iqq.im.IQQActionListener} object.
         * @return a {@link iqq.im.Event.QQActionFuture} object.
         */
        private IQQActionFuture Mark(bool unread, List<QQEmail> mails, QQActionEventHandler listener)
        {
            return PushHttpAction(new MarkEmailAction(unread, mails, Context, listener));
        }

        /**
         * 删除邮件
         *
         * @param mails a {@link java.util.List} object.
         * @param listener a {@link iqq.im.IQQActionListener} object.
         * @return a {@link iqq.im.Event.QQActionFuture} object.
         */
        private IQQActionFuture Delete(List<QQEmail> mails, QQActionEventHandler listener)
        {
            return PushHttpAction(new DeleteEmailAction(mails, Context, listener));
        }
    }

}
