using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Utility.HttpAction.Action;
using WebWeChat.Im.Module;
using WebWeChat.Im.Module.Impl;
using WebWeChat.Im.Module.Interface;

namespace WebWeChat
{
    public class Startup
    {
        public static IConfigurationRoot Configuration { get; private set; }

        /// <summary>
        /// 全局的执行器
        /// </summary>
        public static IActorDispatcher Dispatcher { get; } = new ActorDispatcher();

        static Startup()
        {
            BuildConfig();
            Dispatcher.BeginExcute();
        }

        private static void BuildConfig()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            if (builder.GetFileProvider().GetFileInfo("project.json")?.Exists == true)
            {
                builder.AddUserSecrets();
            }
            Configuration = builder.Build();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Dispatcher);
            services.AddSingleton(Configuration);
        }

        public static void Configure(IServiceProvider provider)
        {
        }

        public static void Dispose()
        {
            Dispatcher.Dispose();
        }
    }
}
