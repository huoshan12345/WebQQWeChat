using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpActionFrame;
using HttpActionFrame.Event;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebWeChat.Im;

namespace WebWeChat
{
    public class Program
    {
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

            var client = new WebWeChatClient(ServiceProvider);
            var @event = client.Login().WaitFinalEvent();
            Console.WriteLine(@event.Type != ActionEventType.EvtOK ? "登录失败" : "登录成功");
            Console.Read();
        }
    }
}
