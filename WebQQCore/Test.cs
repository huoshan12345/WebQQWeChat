using System;
using System.Collections.Generic;
using iQQ.Net.WebQQCore.Im;
using iQQ.Net.WebQQCore.Im.Actor;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore
{
    class Test
    {
        private static readonly QQNotifyHandler handler = (sender, Event) =>
        {
            var client = sender as IQQClient;
            if (client == null) return;

            switch (Event.Type)
            {
                case QQNotifyEventType.GROUP_MSG:
                case QQNotifyEventType.CHAT_MSG:
                {
                    var msg = (QQMsg)Event.Target;
                        DefaultLogger.Info("{0}-好友{1}消息：{2}", client.Account.QQ, msg.From.QQ, msg.GetText());
                        break;
                }
                case QQNotifyEventType.KICK_OFFLINE:
                {
                    DefaultLogger.Info(client.Account.QQ + "-被踢下线: " + (string)Event.Target);
                    break;
                }
                case QQNotifyEventType.CAPACHA_VERIFY:
                {
                    try
                    {
                        var verify = (ImageVerify)Event.Target;
                        verify.Image.Save("verify.png", System.Drawing.Imaging.ImageFormat.Png);
                        DefaultLogger.Info(verify.Reason);
                        Console.Write(client.Account.Username + "-请输入verify.png里面的验证码：");
                        var code = Console.ReadLine();
                        client.SubmitVerify(code, Event);
                    }
                    catch (Exception e)
                    {
                        // TODO Auto-generated catch block
                        DefaultLogger.Info(client.Account.QQ + e.StackTrace);
                    }
                    break;
                }

                case QQNotifyEventType.UNKNOWN_ERROR:
                case QQNotifyEventType.NET_ERROR:
                DefaultLogger.Info(client.Account.QQ + "-出错：" + Event.Target.ToString());
                break;

                default:
                DefaultLogger.Info(client.Account.QQ + "-" + Event.Type + ", " + Event.Target);
                break;
            }
        };



        public static void Main(string[] args)
        {
            var threadActorDispatcher = new ThreadActorDispatcher();
            IQQClient client = null;

            start:
            Console.Write("请输入QQ号：");
            var username = Console.ReadLine();
            Console.Write("请输入QQ密码：");
            var password = Console.ReadLine();
            client = new WebQQClient(username, password, handler, threadActorDispatcher);


            //测试同步模式登录
            var future = client.Login(QQStatus.ONLINE, null);
            DefaultLogger.Info(client.Account.Username + "-登录中......");

            var Event = future.WaitFinalEvent();
            if (Event.Type == QQActionEventType.EVT_OK)
            {
                DefaultLogger.Info(client.Account.Username + "-登录成功！！！！");

                var getUserInfoEvent = client.GetUserInfo(client.Account, null).WaitFinalEvent();

                if (getUserInfoEvent.Type == QQActionEventType.EVT_OK)
                {
                    DefaultLogger.Info(client.Account.QQ + "-用户信息:" + getUserInfoEvent.Target);


                    client.GetBuddyList(null).WaitFinalEvent();
                    DefaultLogger.Info(client.Account.QQ + "-Buddy count: " + client.GetBuddyList().Count);

                    foreach (var buddy in client.GetBuddyList())
                    {
                        var f = client.GetUserQQ(buddy, null);
                        var e = f.WaitFinalEvent();
                        var name = string.IsNullOrEmpty(buddy.MarkName) ? buddy.Nickname : buddy.MarkName;
                        DefaultLogger.Info("{0}, {1}", buddy.QQ, name);
                    }
                }
                //所有的逻辑完了后，启动消息轮询
                client.BeginPollMsg();
            }
            else if (Event.Type == QQActionEventType.EVT_ERROR)
            {
                var ex = (QQException)Event.Target;
                DefaultLogger.Info(ex.Message);
                client.Destroy();
                goto start;
            }
            else
            {
                DefaultLogger.Info(client.Account.Username + "-登录失败");
                client.Destroy();
                goto start;
            }

            DefaultLogger.Info("按任意键退出");
            Console.ReadLine();
        }
    }
}
