using System;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public class Startup
    {
        private static bool _configureServicesExcuted = false;
        private static bool _configureExcuted = false;

        public static void ConfigureServices(IServiceCollection services)
        {
            if (_configureServicesExcuted) return;

            _configureServicesExcuted = true;
        }

        public static void Configure(IServiceProvider provider)
        {
        }
    }
}
