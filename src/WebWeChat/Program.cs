using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using HttpActionFrame;
using HttpActionFrame.Event;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebWeChat.Im;
using WebWeChat.Im.Event;
using WebWeChat.Im.Module.Interface;
using Microsoft.Extensions.Logging;
using Utility.Extensions;

namespace WebWeChat
{
    public class Program
    {
        private static readonly WeChatNotifyEventListener Listener = (client, notifyEvent) =>
        {
            var logger = client.GetModule<ILoggerModule>();
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
                        break;
                    }

                case WeChatNotifyEventType.QRCodeSuccess:
                    logger.LogInformation("请在手机上点击确认以登录");
                    break;

                case WeChatNotifyEventType.QRCodeInvalid:
                    logger.LogWarning("二维码已失效");
                    break;

                default:
                    logger.LogInformation(notifyEvent.Type.GetFullDescription());
                    break;

            }
        };

        public static IServiceProvider ServiceProvider { get; private set; }

        private static void Init()
        {
            var services = new ServiceCollection();
            Startup.ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
            Startup.Configure(ServiceProvider);
        }

        public static void Main(string[] args)
        {
            Init();

            var client = new WebWeChatClient(ServiceProvider, Listener);
            var @event = client.Login().WaitFinalEvent();
            Console.WriteLine($"登录结束{@event.Type}");
            Console.Read();
        }
    }
}
