using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebWeChat.Im.Module;
using WebWeChat.Im.Module.Impl;
using WebWeChat.Im.Module.Interface;

namespace WebWeChat
{
    public class Initializer
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IHttpModule, HttpModule>();
            services.AddTransient<ILoggerModule, LoggerModule>();
            services.AddTransient<ILoginModule, LoginModule>();
        }

        public static void Configure(IServiceProvider provider)
        {
        }
    }
}
