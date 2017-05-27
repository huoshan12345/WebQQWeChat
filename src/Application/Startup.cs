using System;
using System.Net;
using System.Threading.Tasks;
using Application.Models;
using Application.Services;
using FclEx.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

            services.AddEntityFrameworkSqlite().AddDbContext<AppDbContext>();
            EfCore.Startup<AppDbContext>.ConfigureServices(services);

            services.AddTransient<IQQService, QQService>();

            _configureServicesExcuted = true;
        }

        public static void Configure(IApplicationBuilder app)
        {
            if (_configureExcuted) return;

            EfCore.Startup<AppDbContext>.Configure(app.ApplicationServices);
            Seed.AddData(app.ApplicationServices).Wait();

            _configureExcuted = true;
        }
    }
}
