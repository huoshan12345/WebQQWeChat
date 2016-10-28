using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpActionFrame;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebWeChat.Im.Module;
using WebWeChat.Im.Module.Impl;
using WebWeChat.Im.Module.Interface;

namespace WebWeChat
{
    public class Startup
    {
        private static IConfigurationRoot BuildConfig()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            if (builder.GetFileProvider().GetFileInfo("project.json")?.Exists == true)
            {
                builder.AddUserSecrets();
            }
            return builder.Build();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            var config = BuildConfig();
            services.AddSingleton(config);
            HttpActionFrame.Initializer.ConfigureServices(services, config);

            services.AddTransient<IHttpModule, HttpModule>();
            services.AddTransient<ILoggerModule>(provider => new LoggerModule(LogLevel.Information));
            services.AddTransient<ILoginModule, LoginModule>();

            // 以下三个就不以接口形式添加了
            services.AddTransient<StoreModule>();
            services.AddTransient<SessionModule>();
            services.AddTransient<AccountModule>();
        }

        public static void Configure(IServiceProvider provider)
        {
            HttpActionFrame.Initializer.Configure(provider);
        }
    }
}
