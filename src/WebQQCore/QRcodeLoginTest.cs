using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using iQQ.Net.WebQQCore.Im;
using iQQ.Net.WebQQCore.Im.Actor;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Bean.Content;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Log;
using iQQ.Net.WebQQCore.Util;
using iQQ.Net.WebQQCore.Util.Extensions;
using Microsoft.Extensions.Logging;

namespace iQQ.Net.WebQQCore
{
    /// <summary>
    /// 使用二维码登录WebQQ
    /// </summary>
    public class QRcodeLoginTest
    {
        private static readonly QQNotifyListener Listener = (client, notifyEvent) =>
        {
            switch (notifyEvent.Type)
            {
                case QQNotifyEventType.LoginSuccess:
                client.Logger.LogInformation("登录成功");
                break;

                case QQNotifyEventType.GroupMsg:
                {
                    var revMsg = (QQMsg)notifyEvent.Target;
                    client.Logger.LogInformation($"群[{revMsg.Group.Name}]-好友[{revMsg.From.Nickname}]：{revMsg.GetText()}");
                    break;
                }

                case QQNotifyEventType.ChatMsg:
                {
                    var revMsg = (QQMsg)notifyEvent.Target;
                    client.Logger.LogInformation($"好友[{revMsg.From.Nickname}]：{revMsg.GetText()}");

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

                    client.SendMsg(msgReply);
                    break;
                }

                case QQNotifyEventType.QrcodeReady:
                {
                    var verify = (Image)notifyEvent.Target;
                    const string path = "verify.png";
                    verify.Save(path);
                    client.Logger.LogInformation("请扫描在项目根目录下qrcode.png图片");
                    break;
                }

                case QQNotifyEventType.QrcodeSuccess:
                {
                    break;
                }

                case QQNotifyEventType.QrcodeInvalid:
                {
                    client.Logger.LogWarning("二维码已失效");
                    break;
                }

                default:
                {
                    client.Logger.LogInformation(notifyEvent.Type.GetFullDescription());
                    break;
                }
            }
        };

        public static void Main(string[] args)
        {
#if NETCORE
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif

            // 测试每一种控制台颜色
            //foreach (var @enum in EnumExtension.GetValues<ConsoleColor>())
            //{
            //    Console.ForegroundColor = @enum;
            //    Console.WriteLine("Exception thrown: 'iQQ.Net.WebQQCore.Im.QQException' in WebQQCore.exe");
            //}
            //Console.ReadKey();

            // 获取二维码
            var qq = new WebQQClient("", "", Listener, new SimpleActorDispatcher(), new QQConsoleLogger());
            qq.LoginWithQRCode(); // 登录之后自动开始轮训
            Console.ReadKey();
        }
    }
}
