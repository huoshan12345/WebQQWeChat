using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public static IConfigurationRoot Configuration { get; private set; }

        static Startup()
        {
            BuildConfig();
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
            services.AddSingleton(Configuration);
        }

        public static void Configure(IServiceProvider provider)
        {
        }
    }
}
