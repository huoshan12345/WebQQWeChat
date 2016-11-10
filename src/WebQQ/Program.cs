using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using FxUtility.Extensions;
using Microsoft.Extensions.Logging;
using WebQQ.Im;
using WebQQ.Im.Bean;
using WebQQ.Im.Bean.Content;
using WebQQ.Im.Event;
using WebQQ.Im.Module.Impl;
using System.Reflection;

namespace WebQQ
{
    /// <summary>
    /// 使用二维码登录WebQQ
    /// </summary>
    public class Program
    {
        private static Process _process = null;
        private static readonly QQNotifyEventListener Listener = (client, notifyEvent) =>
        {
            var logger = client.GetSerivce<ILogger>();
            switch (notifyEvent.Type)
            {
                case QQNotifyEventType.LoginSuccess:
                    logger.LogInformation("登录成功");
                    break;

                case QQNotifyEventType.GroupMsg:
                    {
                        var revMsg = (QQMsg)notifyEvent.Target;
                        logger.LogInformation($"群[{revMsg.Group.Name}]-好友[{revMsg.From.Nick}]：{revMsg.GetText()}");
                        break;
                    }

                case QQNotifyEventType.ChatMsg:
                    {
                        var revMsg = (QQMsg)notifyEvent.Target;
                        logger.LogInformation($"好友[{revMsg.From.Nick}]：{revMsg.GetText()}");

                        var msgReply = new QQMsg()
                        {
                            Type = QQMsgType.BUDDY_MSG,
                            To = revMsg.From,
                            From = client.GetModule<SessionModule>().User,
                            Date = DateTime.Now,
                        };
                        msgReply.AddContentItem(new TextItem("hello from iqq")); // 添加文本内容
                        msgReply.AddContentItem(new FaceItem(0));            // QQ id为0的表情
                        msgReply.AddContentItem(new FontItem());             // 使用默认字体

                        // client.SendMsg(msgReply);
                        break;
                    }

                case QQNotifyEventType.QRCodeReady:
                    {
                        var verify = (Image)notifyEvent.Target;
                        const string path = "verify.png";
                        verify.Save(path);
                        logger.LogInformation("请扫描在项目根目录下qrcode.png图片");
#if NET
                        _process = Process.Start(path);
#endif
                        break;
                    }


                case QQNotifyEventType.QRCodeSuccess:
                    _process?.Kill();
                    break;

                case QQNotifyEventType.QRCodeInvalid:
                    _process?.Kill();
                    logger.LogWarning("二维码已失效");
                    break;

                default:
                    logger.LogInformation(notifyEvent.Type.GetFullDescription());
                    break;
            }
        };

        public static void Main(string[] args)
        {
#if NETCORE
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
            // 获取二维码
            var qq = new WebQQClient(Listener);
            qq.Login().Wait();

            Console.Read();
        }
    }
}
