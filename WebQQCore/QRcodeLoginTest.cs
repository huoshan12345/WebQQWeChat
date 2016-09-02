using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using iQQ.Net.WebQQCore.Im;
using iQQ.Net.WebQQCore.Im.Actor;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Bean.Content;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Util;
using iQQ.Net.WebQQCore.Util.Extensions;

namespace iQQ.Net.WebQQCore
{
    /// <summary>
    /// 使用二维码登录WebQQ
    /// </summary>
    public class QRcodeLoginTest
    {
        private static Process _qrCodeProcess = null;

        private static readonly IQQClient _mClient = new WebQQClient("", "", (client, notifyEvent) =>
        {
            switch (notifyEvent.Type)
            {
                case QQNotifyEventType.LoginSuccess:
                client.Logger.Info("登录成功");
                break;

                case QQNotifyEventType.GroupMsg:
                {
                    var revMsg = (QQMsg)notifyEvent.Target;
                    client.Logger.Info($"群[{revMsg.Group.Name}]-好友[{revMsg.From.Nickname}]：{revMsg.GetText()}");
                    break;
                }

                case QQNotifyEventType.ChatMsg:
                {
                    var revMsg = (QQMsg)notifyEvent.Target;
                    client.Logger.Info($"好友[{revMsg.From.Nickname}]：{revMsg.GetText()}");

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
                    verify.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                    client.Logger.Info("请扫描在项目根目录下qrcode.png图片");
                    _qrCodeProcess = Process.Start(path);
                    break;
                }

                case QQNotifyEventType.QrcodeSuccess:
                {
                    _qrCodeProcess?.CloseMainWindow();
                    _qrCodeProcess?.Kill();
                    break;
                }

                case QQNotifyEventType.QrcodeInvalid:
                {
                    client.Logger.Warn("二维码已失效");
                    _qrCodeProcess?.CloseMainWindow();
                    _qrCodeProcess?.Kill();
                    break;
                }

                default:
                {
                    client.Logger.Info(notifyEvent.Type.GetFullDescription());
                    break;
                }
            }

        }, new ThreadActorDispatcher());

        public static void Main(string[] args)
        {
            // 获取二维码
            _mClient.LoginWithQRCode(); // 登录之后自动开始轮训
            Console.ReadKey();
        }
    }
}
