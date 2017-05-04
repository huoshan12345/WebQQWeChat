using System;
using Application.Models;
using Microsoft.AspNetCore.Builder;
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

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 10;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            //// Adds IdentityServer
            services.AddIdentityServer(options =>
            {
                // options.UserInteraction.LogoutUrl = "/api/Account/Logout/";
            })
            .AddTemporarySigningCredential()
            .AddInMemoryApiResources(Config.GetApiResources())
            .AddInMemoryClients(Config.GetClients())
            .AddTestUsers(Config.GetTestUsers())
            .AddAspNetIdentity<AppUser>();


            _configureServicesExcuted = true;
        }

        public static void Configure(IApplicationBuilder app)
        {
            if (_configureExcuted) return;

            EfCore.Startup<AppDbContext>.Configure(app.ApplicationServices);
            Seed.AddData(app.ApplicationServices).Wait();

            app.UseIdentity();
            // 授权端
            app.UseIdentityServer();

            _configureExcuted = true;
        }
    }
}
