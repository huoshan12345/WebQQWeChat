using System;
using System.Collections.Generic;
using iQQ.Net.WebQQCore.Im;
using iQQ.Net.WebQQCore.Im.Actor;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Util;
using iQQ.Net.WebQQCore.Util.Extensions;
using Microsoft.Extensions.Logging;

namespace iQQ.Net.WebQQCore
{
    [Obsolete("验证码登录方式已经不可用，请使用二维码登录")]
    class Test
    {
        private static readonly QQNotifyListener Listener = (client, Event) =>
        {
            //var client = sender as IQQClient;
            //if (client == null) return;
 
            switch (Event.Type)
            {
                case QQNotifyEventType.GroupMsg:
                case QQNotifyEventType.ChatMsg:
                {
                    var msg = (QQMsg)Event.Target;
                        client.Logger.LogInformation($"{client.Account.QQ}-好友{msg.From.QQ}消息：{msg.GetText()}");
                        break;
                }
                case QQNotifyEventType.KickOffline:
                {
                    client.Logger.LogInformation(client.Account.QQ + "-被踢下线: " + (string)Event.Target);
                    break;
                }
                case QQNotifyEventType.CapachaVerify:
                {
                    try
                    {
                        var verify = (ImageVerify)Event.Target;
                        verify.Image.Save("verify.png");
                        client.Logger.LogInformation(verify.Reason);
                        Console.Write(client.Account.Username + "-请输入verify.png里面的验证码：");
                        var code = Console.ReadLine();
                        client.SubmitVerify(code, Event);
                    }
                    catch (Exception e)
                    {
                        // TODO Auto-generated catch block
                        client.Logger.LogInformation(client.Account.QQ + e.StackTrace);
                    }
                    break;
                }

                case QQNotifyEventType.UnknownError:
                case QQNotifyEventType.NetError:
                client.Logger.LogInformation(client.Account.QQ + "-出错：" + Event.Target.ToString());
                break;

                default:
                client.Logger.LogInformation(client.Account.QQ + "-" + Event.Type + ", " + Event.Target);
                break;
            }
        };



        public static void Main2(string[] args)
        {
            var threadActorDispatcher = new SimpleActorDispatcher();
            IQQClient client = null;

            start:
            Console.Write("请输入QQ号：");
            var username = Console.ReadLine();
            Console.Write("请输入QQ密码：");
            var password = Console.ReadLine();
            client = new WebQQClient(username, password, Listener, threadActorDispatcher);


            //测试同步模式登录
            var future = client.Login(QQStatus.ONLINE, null);
            client.Logger.LogInformation(client.Account.Username + "-登录中......");

            var Event = future.WaitFinalEvent();
            if (Event.Type == QQActionEventType.EvtOK)
            {
                client.Logger.LogInformation(client.Account.Username + "-登录成功！！！！");

                var getUserInfoEvent = client.GetUserInfo(client.Account, null).WaitFinalEvent();

                if (getUserInfoEvent.Type == QQActionEventType.EvtOK)
                {
                    client.Logger.LogInformation(client.Account.QQ + "-用户信息:" + getUserInfoEvent.Target);


                    client.GetBuddyList(null).WaitFinalEvent();
                    client.Logger.LogInformation(client.Account.QQ + "-Buddy count: " + client.GetBuddyList().Count);

                    foreach (var buddy in client.GetBuddyList())
                    {
                        var f = client.GetUserQQ(buddy, null);
                        var e = f.WaitFinalEvent();
                        var name = string.IsNullOrEmpty(buddy.MarkName) ? buddy.Nickname : buddy.MarkName;
                        client.Logger.LogInformation($"{buddy.QQ}, {name}");
                    }
                }
                //所有的逻辑完了后，启动消息轮询
                client.BeginPollMsg();
            }
            else if (Event.Type == QQActionEventType.EvtError)
            {
                var ex = (QQException)Event.Target;
                client.Logger.LogInformation(ex.Message);
                client.Destroy();
                goto start;
            }
            else
            {
                client.Logger.LogInformation(client.Account.Username + "-登录失败");
                client.Destroy();
                goto start;
            }

            client.Logger.LogInformation("按任意键退出");
            Console.ReadLine();
        }
    }
}
