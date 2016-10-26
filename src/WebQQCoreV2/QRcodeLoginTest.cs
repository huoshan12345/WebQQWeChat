using System;
using System.Diagnostics;
using System.Drawing;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Bean.Content;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using Microsoft.Extensions.Logging;
using iQQ.Net.WebQQCore.Im.Service.Log;
using System.Text;
using HttpActionFrame.Event;
using iQQ.Net.WebQQCore.Im;
using Utility.Extensions;

namespace iQQ.Net.WebQQCore
{
    /// <summary>
    /// 使用二维码登录WebQQ
    /// </summary>
    public class QRcodeLoginTest
    {
        private static readonly QQNotifyEventListener Listener = (client, notifyEvent) =>
        {
            var logger = client.GetSerivce<IQQLogger>(QQServiceType.Log);
            switch (notifyEvent.Type)
            {
                case QQNotifyEventType.LoginSuccess:
                {
                    logger.LogInformation("登录成功");
                    break;
                }

                case QQNotifyEventType.GroupMsg:
                {
                    var revMsg = (QQMsg)notifyEvent.Target;
                    logger.LogInformation($"群[{revMsg.Group.Name}]-好友[{revMsg.From.Nickname}]：{revMsg.GetText()}");
                    break;
                }

                case QQNotifyEventType.ChatMsg:
                {
                    var revMsg = (QQMsg)notifyEvent.Target;
                    logger.LogInformation($"好友[{revMsg.From.Nickname}]：{revMsg.GetText()}");

                    var msgReply = new QQMsg()
                    {
                        Type = QQMsgType.BUDDY_MSG,
                        To = revMsg.From,
                        From = client.Account,
                        Date = DateTime.Now,
                    };
                    msgReply.AddContentItem(new TextItem("hello from iqq")); // 添加文本内容
                    msgReply.AddContentItem(new FaceItem(0));            // QQ id为0的表情
                    msgReply.AddContentItem(new FontItem());             // 使用默认字体

                    // client.SendMsg(msgReply);
                    break;
                }

                case QQNotifyEventType.QrcodeReady:
                {
                    var verify = (Image)notifyEvent.Target;
                    const string path = "verify.png";
                    verify.Save(path);
                    logger.LogInformation("请扫描在项目根目录下qrcode.png图片");
                    break;
                }

                case QQNotifyEventType.QrcodeSuccess:
                {
                    break;
                }

                case QQNotifyEventType.QrcodeInvalid:
                {
                    logger.LogWarning("二维码已失效");
                    break;
                }

                default:
                {
                    logger.LogInformation(notifyEvent.Type.GetFullDescription());
                    break;
                }
            }
        };

        private static TimeSpan TestAction(Action action, int times)
        {
            var watch = new Stopwatch();
            watch.Start();
            for (var i = 0; i < times; i++)
            {
                action();
            }
            watch.Stop();
            return watch.Elapsed;
        }

        public static void Main(string[] args)
        {
#if NETCORE
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif

            // 测试每一种控制台颜色
            //foreach (var @enum in EnumExtension.GetValues<ConsoleColor>())
            //{
            //    Console.ForegroundColor = @enum;
            //    Console.WriteLine("颜色和编码测试");
            //}

            // 获取二维码
            // var qq = new WebQQClient("", "", Listener, null, new QQConsoleLogger());
            var qq = new WebQQClient();

            var timeSpan = TestAction(() =>
            {
                var @event = qq.LoginWithQRCode().WaitFinalEvent();
                if (@event.Type != ActionEventType.EvtOK)
                {
                    Console.WriteLine("xxxxxxxxxxxxxx");
                }
            }, 100);

            Console.WriteLine(timeSpan.TotalMilliseconds);
            
            Console.Read();
        }
    }
}
