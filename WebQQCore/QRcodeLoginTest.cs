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
    /**
     * 使用二维码登录WebQQ
     * <p/>
     * <p/>
     * Created by Tony on 10/6/15.
     */
    public class QRcodeLoginTest
    {
        private static readonly IQQClient _mClient = new WebQQClient("", "", (client, notifyEvent) =>
        {
            switch (notifyEvent.Type)
            {
                case QQNotifyEventType.LOGIN_SUCCESS:
                DefaultLogger.Info("登录成功");
                break;

                case QQNotifyEventType.GROUP_MSG:
                {
                    var revMsg = (QQMsg)notifyEvent.Target;
                    DefaultLogger.Info($"{client.Account.QQ}-群{revMsg.Group.Name}好友{revMsg.From.QQ}消息：{revMsg.GetText()}");
                    break;
                }

                case QQNotifyEventType.CHAT_MSG:
                {
                    var revMsg = (QQMsg)notifyEvent.Target;
                    DefaultLogger.Info($"{client.Account.QQ}-好友{revMsg.From.QQ}消息：{revMsg.GetText()}");

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

                case QQNotifyEventType.QRCODE_READY:
                {
                    var verify = (Image)notifyEvent.Target;
                    const string path = "verify.png";
                    verify.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                    DefaultLogger.Info("请扫描在项目根目录下qrcode.png图片");
                    Process.Start(path);
                    break;
                }

                case QQNotifyEventType.QRCODE_INVALID:
                {
                    DefaultLogger.Info("二维码已失效");
                    break;
                }

                default:
                {
                    DefaultLogger.Info(notifyEvent.Type.GetFullDescription());
                    break;
                }
            }

        }, new ThreadActorDispatcher());

        public static void Main(string[] args)
        {
            // 获取二维码
            var loginResult = _mClient.LoginWithQRCode().WaitFinalEvent();
            if (loginResult.Type == QQActionEventType.EVT_OK)
            {
                _mClient.GetBuddyList((s, e) =>
                {
                    if (e.Type == QQActionEventType.EVT_OK) DefaultLogger.Info("加载好友列表成功");
                });
                _mClient.GetGroupList((s, e) =>
                {
                    if (e.Type == QQActionEventType.EVT_OK) DefaultLogger.Info("加载群列表成功");
                });
                _mClient.GetSelfInfo((s, e) =>
                {
                    if (e.Type == QQActionEventType.EVT_OK) DefaultLogger.Info("获取个人信息成功");
                });
                _mClient.BeginPollMsg();
            }
            Console.ReadKey();
        }
    }
}
