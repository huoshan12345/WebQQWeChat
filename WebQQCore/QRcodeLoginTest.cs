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
        private static Process qrCodeProcess = null;
        private static readonly IQQClient _mClient = new WebQQClient("", "", (client, notifyEvent) =>
        {
            switch (notifyEvent.Type)
            {
                case QQNotifyEventType.LoginSuccess:
                client.Logger.Info($"[{client.Account.QQ}]: 登录成功");
                break;

                case QQNotifyEventType.GroupMsg:
                {
                    var revMsg = (QQMsg)notifyEvent.Target;
                    client.Logger.Info($"{client.Account.QQ}-群{revMsg.Group.Name}好友{revMsg.From.QQ}消息：{revMsg.GetText()}");
                    break;
                }

                case QQNotifyEventType.ChatMsg:
                {
                    var revMsg = (QQMsg)notifyEvent.Target;
                    client.Logger.Info($"{client.Account.QQ}-好友{revMsg.From.QQ}消息：{revMsg.GetText()}");

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
                    DefaultLogger.Info("请扫描在项目根目录下qrcode.png图片");
                    qrCodeProcess = Process.Start(path);
                    break;
                }

                case QQNotifyEventType.QrcodeSuccess:
                {
                    qrCodeProcess?.CloseMainWindow();
                    qrCodeProcess?.Kill();
                    break;
                }

                case QQNotifyEventType.QrcodeInvalid:
                {
                    DefaultLogger.Warn("二维码已失效");
                    qrCodeProcess?.CloseMainWindow();
                    qrCodeProcess?.Kill();
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
            var loginResult = _mClient.LoginWithQRCode().WaitFinalEvent(); // 登录之后自动开始轮训
            if (loginResult.Type == QQActionEventType.EvtOK)
            {
                _mClient.GetBuddyList((s, e) =>
                {
                    if (e.Type == QQActionEventType.EvtOK) _mClient.Logger.Info("加载好友列表成功");
                });
                //_mClient.GetGroupList((s, e) =>
                //{
                //    if (e.Type == QQActionEventType.EvtOK) client.Logger.Info("加载群列表成功");
                //});
                _mClient.GetSelfInfo((s, e) =>
                {
                    if (e.Type == QQActionEventType.EvtOK) _mClient.Logger.Info("获取个人信息成功");
                });
            }
            Console.ReadKey();
        }
    }
}
