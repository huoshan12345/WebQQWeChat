using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.RequestContainer;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;

namespace WebQQServer
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
        }

        // This method gets called by a runtime.
        // Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            // Uncomment the following line to add Web API services which makes it easier to port Web API 2 controllers.
            // You will also need to add the Microsoft.AspNet.Mvc.WebApiCompatShim package to the 'dependencies' section of project.json.
            // services.AddWebApiConventions();
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerfactory)
        {
            // Add the console logger.
            loggerfactory.AddConsole();

            // Configure the HTTP request pipeline.
            app.UseStaticFiles();

            // Add the following to the request pipeline only in development environment.
            if (env.IsEnvironment("Development"))
            {
                // 在IHostingEnvironment.EnvironmentName为Development的情况下，才显示错误信息，并且错误信息的显示种类，
                // 可以通过额外的ErrorPageOptions参数来设定，可以设置全部显示，也可以设置只显示Cookies、Environment、
                // ExceptionDetails、Headers、Query、SourceCode SourceCodeLineCount中的一种或多种。
                app.UseBrowserLink();
                app.UseErrorPage(ErrorPageOptions.ShowAll);
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // send the request to the following path or controller action.
                app.UseErrorHandler("/Home/Error"); // 捕获所有的程序异常错误，并将请求跳转至指定的页面，以达到友好提示的目的。
            }

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                // Add the following route for porting Web API 2 controllers.
                routes.MapRoute("DefaultApi", "api/{controller}/{id?}");
            });

            app.Use(nextApp => new ContainerMiddleware(nextApp, app.ApplicationServices).Invoke);
        }
    }
}
