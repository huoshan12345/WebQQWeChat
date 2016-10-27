using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpActionFrame.Actor;
using HttpActionFrame.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace HttpActionFrame
{
    public class Initializer
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpService, HttpService>();
            services.AddSingleton<IActorDispatcher, SimpleActorDispatcher>();
        }

        public static void Configure(IServiceProvider provider)
        {
        }
    }
}
