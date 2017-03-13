using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpAction.Action;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebWeChat.Im.Module;

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
                builder.AddUserSecrets<Startup>();
            }
            return builder.Build();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(p => BuildConfig());
        }

        public static void Configure(IServiceProvider provider)
        {
        }

        public static void Dispose()
        {
        }
    }
}
