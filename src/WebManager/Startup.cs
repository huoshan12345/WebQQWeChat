using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Application;
using Application.Models;
using FclEx.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WebManager.Filters;

namespace WebManager
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            if (env.IsDevelopment())
            {
                //注意：如果你的appsetting.json文件中有和secrets.json文件中相同节点（冲突）的配置项，那么就会被secrets.json中的设置项给覆盖掉，
                //因为 builder.AddUserSecrets()晚于 AddJsonFile("appsettings.json")注册, 
                //那么我们可以利用这个特性来在每个开发人员的机器上重新设置数据库连接字符串了。
                builder.AddUserSecrets<Startup>();
            }
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Application.Startup.ConfigureServices(services);

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 10;
                options.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = context =>
                    {
                        if (context.Request.IsApiUrl())
                        {
                            context.Response.Clear();
                            context.Response.StatusCode = HttpStatusCode.Unauthorized.ToInt();
                            return Task.CompletedTask;
                        }
                        context.Response.Redirect(context.RedirectUri);
                        return Task.CompletedTask;
                    }
                };
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            //// Adds IdentityServer
            services.AddIdentityServer(options =>
            {
                // options.UserInteraction.LogoutUrl = "/api/Account/Logout/";
                options.IssuerUri = Program.HomeUrl;
            })
            .AddTemporarySigningCredential()
            .AddInMemoryApiResources(Config.GetApiResources())
            .AddInMemoryClients(Config.GetClients())
            .AddTestUsers(Config.GetTestUsers())
            .AddAspNetIdentity<AppUser>()
            //  Try moving the call to AddProfileService to the end of the AddIdentityServer
            // The AddAspNetIdentity method registers its own IProfileService, which will overwrite any previous registration.
            .AddProfileService<IdentityWithAdditionalClaimsProfileService>();


            services.AddAuthorization(options =>
            {
                options.AddPolicy("admin", policyAdmin =>
                {
                    policyAdmin.RequireClaim("role", "admin");
                });
            });


            // Add framework services.
            services.AddMvc(options => options.Filters.Add(typeof(ValidateFilter)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Application.Startup.Configure(app);

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();


            app.UseIdentity();
            // 授权端
            app.UseIdentityServer();

            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions()
            {
                Authority = Program.HomeUrl,
                ApiName = Config.Resource.Name,
                RequireHttpsMetadata = false,
            });

            app.UseMvcWithDefaultRoute();
        }
    }
}
