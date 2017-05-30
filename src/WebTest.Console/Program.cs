using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FclEx.Extensions;
using FclEx.Logger;
using HttpAction;
using HttpAction.Service;
using Microsoft.Extensions.Logging;
using WebTest.Actions;
using WebTest.Models;
using WebTest.Models.QQModels;

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

                    var sendMsgAction = new SendMsgAction(token, new QQMessageModel{QQId = id, Text = "Test", Type = QQMessageType.Group, UserName = "存储文件", }, service);
                    await sendMsgAction.ExecuteAsyncAuto();

                    var action = new PollAction(token, id, service);
                    while (true)
                    {
                        var events = await action.ExecuteAsyncAuto();
                        if (events.TryGet<QQNotifyEventModel[]>(out var list))
                        {
                            Console.WriteLine($"消息数量为：{list.Length}");
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

        public static Task NotifyEvent(ILogger logger, QQNotifyEventModel notifyEvent)
        {
            switch (notifyEvent.Type)
            {
                case QQNotifyEventType.LoginSuccess:
                    logger.LogInformation("登录成功");
                    break;

                case QQNotifyEventType.QRCodeReady:
                    {
                        var verify = notifyEvent.Target.Base64StringToImage();
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
                    logger.LogInformation(notifyEvent.Target);
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

            var json = new List<QQNotifyEventModel>()
            {
                new QQNotifyEventModel(QQNotifyEventType.QRCodeReady, base64)
            }.ToJson();

            var base642 = json.ToJToken().ToObject<QQNotifyEventModel[]>()[0].Target;
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