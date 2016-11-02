using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Microsoft.Extensions.DependencyInjection;
using WebWeChat.Im;
using WebWeChat.Im.Event;
using Microsoft.Extensions.Logging;
using Utility.Extensions;
using System.Runtime.InteropServices;
using Utility.HttpAction.Event;
using System.Text;
using System.Threading;
using WebWeChat.Im.Bean;
using WebWeChat.Im.Service.Interface;

namespace WebWeChat
{
    public class Program
    {
        private static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false); // 用这个来保持控制台不退出

        private static Process _process = null;
        private static readonly WeChatNotifyEventListener Listener = (client, notifyEvent) =>
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
                        verify.Save(path);
                        logger.LogInformation($"请扫描在项目根目录下{path}图片");
#if NET
                        _process = Process.Start(path);
#endif
                        break;
                    }

                case WeChatNotifyEventType.QRCodeSuccess:
                    _process?.Kill();
                    logger.LogInformation("请在手机上点击确认以登录");
                    break;

                case WeChatNotifyEventType.QRCodeInvalid:
                    _process?.Kill();
                    logger.LogWarning("二维码已失效");
                    break;

                case WeChatNotifyEventType.Message:
                    {
                        var msg = (Message)notifyEvent.Target;
                        logger.LogInformation($"[{msg.MsgType.GetDescription()}]: {msg.Content}");
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
        public static void Main(string[] args)
        {
#if NETCORE
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
            var client = new WebWeChatClient(Listener);
            var @event = client.Login().Result;
            if (@event.Type != ActionEventType.EvtOK)
            {
                Console.WriteLine("登录失败");
            }

            QuitEvent.WaitOne();
            // Console.Read(); // 不要用这句来保持不退出，会导致死锁
            client.Dispose();
            Startup.Dispose(); // 释放全局资源
        }



        public static void Test()
        {

        }

    }
}
