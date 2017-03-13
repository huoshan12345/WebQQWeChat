using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using FclEx.Extensions;
using Microsoft.Extensions.Logging;
using WebQQ.Im;
using WebQQ.Im.Bean;
using WebQQ.Im.Bean.Content;
using WebQQ.Im.Event;
using WebQQ.Im.Module.Impl;
using System.Reflection;
using System.Threading.Tasks;
using HttpAction.Event;
using Microsoft.Extensions.DependencyInjection;
using WebQQ.Im.Bean.Friend;
using WebQQ.Im.Bean.Group;
using WebQQ.Im.Service.Impl;

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

                case QQNotifyEventType.QRCodeReady:
                    {
                        var verify = (Image)notifyEvent.Target;
                        const string path = "verify.png";
                        verify.Save(path, ImageFormat.Png);
                        logger.LogInformation($"请扫描在项目根目录下{path}图片");
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

                case QQNotifyEventType.GroupMsg:
                    {
                        var msg = (GroupMessage)notifyEvent.Target;
                        logger.LogInformation($"[群消息][{msg.Group.ShowName}]{msg.GetText()}");
                        break;
                    }
                case QQNotifyEventType.ChatMsg:
                    {
                        var msg = (FriendMessage)notifyEvent.Target;
                        logger.LogInformation($"[好友消息][{msg.Friend.ShowName}]{msg.GetText()}");
                        break;
                    }

                default:
                    logger.LogInformation(notifyEvent.Type.GetFullDescription());
                    break;
            }

            return Task.CompletedTask;
        };

        public static void Main(string[] args)
        {
#if NETCORE
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif

            // 获取二维码
            var qq = new WebQQClient(new QQConsoleLogger(LogLevel.Debug), Listener);
            qq.Login().Wait();
            qq.BeginPoll();


            Console.Read();
        }
    }
}
