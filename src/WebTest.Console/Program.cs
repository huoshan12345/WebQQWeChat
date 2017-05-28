using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FclEx.Extensions;
using FclEx.Logger;
using HttpAction;
using HttpAction.Action;
using HttpAction.Service;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Bean.Friend;
using WebQQ.Im.Bean.Group;
using WebQQ.Im.Core;
using WebQQ.Im.Event;
using WebQQ.Util;
using WebTest.Actions;

namespace WebTest
{
    public class Program
    {
        private static readonly ILogger _logger = new SimpleConsoleLogger(nameof(Program), LogLevel.Trace);

        public static async Task TestWebQQ()
        {
            var service = new HttpService();
            var result = await new GetTokenAction(service).ExecuteAsyncAuto();
            if (result.IsOk())
            {
                var token = result.Get<string>();
                var loginResult = await new LoginQQAction(token, service).ExecuteAsyncAuto();
                if (loginResult.IsOk())
                {
                    var id = loginResult.Get<string>();
                    var action = new GetQQEventsAction(token, id, service);
                    while (true)
                    {
                        var events = await action.ExecuteAsyncAuto();
                        if (events.TryGet<IReadOnlyList<QQNotifyEvent>>(out var list))
                        {
                            Console.WriteLine($"消息数量为：{list.Count}");
                            foreach (var item in list)
                            {
                                try
                                {
                                    await NotifyEvent(_logger, item);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine(events.Target.CastTo<Exception>().Message);
                        }
                    }
                }
                else
                {
                    Console.WriteLine(loginResult.Target);
                }
            }
        }

        public static Task NotifyEvent(ILogger logger, QQNotifyEvent notifyEvent)
        {
            switch (notifyEvent.Type)
            {
                case QQNotifyEventType.LoginSuccess:
                    logger.LogInformation("登录成功");
                    break;

                case QQNotifyEventType.QRCodeReady:
                    {
                        var verify = notifyEvent.Target.CastTo<string>().Base64StringToImage();
                        const string path = "verify.png";
                        verify.Save(path);
                        logger.LogInformation($"请扫描在项目根目录下{path}图片");
                        break;
                    }

                case QQNotifyEventType.QRCodeInvalid:
                    logger.LogWarning("二维码已失效");
                    break;

                case QQNotifyEventType.ChatMsg:
                case QQNotifyEventType.GroupMsg:
                    logger.LogInformation(notifyEvent.Target.CastTo<string>());
                    break;

                default:
                    logger.LogInformation(notifyEvent.Type.GetFullDescription());
                    break;
            }

            return Task.CompletedTask;
        }

        public static void TestBase64()
        {
            var img = ImageSharp.Image.Load("verify.png");
            var base64 = img.ToRawBase64String();

            var json = new List<QQNotifyEvent>()
            {
                QQNotifyEvent.CreateEvent(QQNotifyEventType.QRCodeReady, base64)
            }.ToJson();

            var base642 = json.ToJToken().ToObject<List<QQNotifyEvent>>()[0].Target.CastTo<string>();
            var img2 = base642.Base64StringToImage();
            img2.Save("verify2.png");

        }


        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            // TestBase64();

            while (true)
            {
                Console.WriteLine("press any key to test qq");
                if (Console.ReadLine() == "q") break;
                TestWebQQ().Forget();
            }

            Console.WriteLine("press any key to exit");
            Console.ReadLine();
        }
    }
}