using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FclEx.Extensions;
using HttpAction;
using HttpAction.Action;
using HttpAction.Service;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Event;
using WebTest.Actions;

namespace WebTest
{
    public class Program
    {
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
                            foreach (var item in list)
                            {
                                Console.WriteLine(item);
                            }
                        }
                        else
                        {
                            Console.WriteLine(events.Target);
                        }
                    }
                }
            }
        }


        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            Console.WriteLine("press any key to test qq");
            Console.ReadLine();

            TestWebQQ().Forget();

            Console.ReadLine();
        }
    }
}