using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
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
                        break;
                    }

                case QQNotifyEventType.ChatMsg:
                    {
                        break;
                    }

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
