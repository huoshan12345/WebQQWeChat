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
using FxUtility.Extensions;
using HttpAction.Event;

namespace WebWeChat
{
    public class Program
    {
        private static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false); // 用这个来保持控制台不退出
        private static Process _process = null;

        private static readonly WeChatNotifyEventListener Listener = async (client, notifyEvent) =>
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
                    QuitEvent.Set();
                    break;

                case WeChatNotifyEventType.Message:
                    {
                        var msg = (Message)notifyEvent.Target;
                        logger.LogInformation($"[{msg.MsgType.GetDescription()} 来自 {msg.FromUser?.ShowName}]: {msg.Content}");
                        var userName = client.GetModule<SessionModule>().User.UserName;
                        if (msg.FromUserName == userName && msg.MsgType == MessageType.Text && !msg.Content.IsNullOrEmpty())
                        {
                            var reply = await client.GetRobotReply(RobotType.Tuling, msg.Content);
                            if (reply.Type == ActionEventType.EvtOK)
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
                    QuitEvent.Set();
                    break;

                default:
                    logger.LogInformation(notifyEvent.Type.GetFullDescription());
                    break;

            }
        };

        private static async Task TestWeChat()
        {
            using (var client = new WebWeChatClient(Listener))
            {
                var @event = await client.Login();
                if (@event.Type == ActionEventType.EvtOK)
                {
                    // await client.GetContact(); // 登录过程中会获取一次联系人
                    client.BeginSyncCheck();
                }
                else
                {
                    Console.WriteLine("登录失败");
                }
                QuitEvent.WaitOne();
            }
        }

        public static void Main(string[] args)
        {
#if NETCORE
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
            TestWeChat().Wait();
            Startup.Dispose(); // 释放全局资源
            Console.Read();
        }
    }
}
