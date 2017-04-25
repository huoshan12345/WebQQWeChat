using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using WebWeChat.Im;
using WebWeChat.Im.Event;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using WebWeChat.Im.Bean;
using WebWeChat.Im.Module.Impl;
using FclEx.Extensions;
using HttpAction.Event;
using WebWeChat.Im.Service.Impl;
using HttpAction;

namespace WebWeChat
{
    public class Program
    {
        private static Process _process = null;

        private static readonly WeChatNotifyEventListener _listener = async (client, notifyEvent) =>
        {
            var logger = client.GetSerivce<ILogger>();
            switch (notifyEvent.Type)
            {
                case WeChatNotifyEventType.LoginSuccess:
                    logger.LogInformation("登录成功");
                    break;

                case WeChatNotifyEventType.QRCodeReady:
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

                case WeChatNotifyEventType.QRCodeSuccess:
                    if (_process != null && !_process.HasExited) _process.Kill();
                    logger.LogInformation("请在手机上点击确认以登录");
                    break;

                case WeChatNotifyEventType.QRCodeInvalid:
                    if(_process != null && !_process.HasExited) _process.Kill();
                    logger.LogWarning("二维码已失效");
                    break;

                case WeChatNotifyEventType.Message:
                    {
                        var msg = (Message)notifyEvent.Target;
                        logger.LogInformation($"[{msg.MsgType.GetDescription()} 来自 {msg.FromUser?.ShowName}]: {msg.Content}");
                        var userName = client.GetModule<SessionModule>().User.UserName;
                        if (msg.FromUserName == userName && msg.MsgType == MessageType.Text && !msg.Content.IsNullOrEmpty())
                        {
                            var reply = await client.GetRobotReply(RobotType.Tuling, msg.Content);
                            if (reply.IsOk())
                            {
                                var text = (string) reply.Target;
                                text = $"{text}  --来自机器人回复";
                                await client.SendMsg(MessageSent.CreateTextMsg(text, userName, msg.ToUserName));
                            }
                        }
                        break;
                    }

                case WeChatNotifyEventType.Offline:
                    logger.LogCritical("微信已经掉线");
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
            var client = new WebWeChatClient(m => new WeChatConsoleLogger(m, LogLevel.Debug), _listener);
            client.Login((s, e) =>
            {
                if (e.IsOk())
                {
                    // await client.GetContact(); // 登录过程中会获取一次联系人
                    client.BeginSyncCheck();
                }
                else
                {
                    Console.WriteLine("登录失败");
                }
                return Task.CompletedTask;
            });

            Console.Read();
        }
    }
}
