using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Utility.HttpAction.Service;

namespace Utility.HttpAction
{
    public class Initializer
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpService, HttpService>();
        }

        public static void Configure(IServiceProvider provider)
        {
        }
    }
}
