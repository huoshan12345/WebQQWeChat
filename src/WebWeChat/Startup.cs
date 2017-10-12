using Microsoft.Extensions.Configuration;

namespace WebWeChat
{
    public class Startup
    {
        public static IConfigurationRoot BuildConfig()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            if (builder.GetFileProvider().GetFileInfo("project.json")?.Exists == true)
            {
                builder.AddUserSecrets<Startup>();
            }
            return builder.Build();
        }
    }
}
