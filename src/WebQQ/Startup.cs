using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HttpAction.Action;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebQQ.Im.Bean;
using WebQQ.Im.Bean.Friend;
using WebQQ.Im.Bean.Group;

namespace WebQQ
{
    public class Startup
    {
        public static IConfigurationRoot Configuration { get; private set; }

        /// <summary>
        /// 全局的执行器
        /// </summary>
        public static IActorDispatcher Dispatcher { get; private set; }

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
            services.AddSingleton<IActorDispatcher, ActorDispatcher>();
            services.AddSingleton(Configuration);
        }

        public static void Configure(IServiceProvider provider)
        {
            Dispatcher = provider.GetService<IActorDispatcher>();
            Dispatcher.BeginExcute();


            Mapper.Initialize(x =>
            {
                x.CreateMap<GroupInfo, QQGroup>();
                x.CreateMap<GroupMemberCard, GroupMember>();
                x.CreateMap<GroupMemberInfo, GroupMember>();
                x.CreateMap<GroupMemberStatus, GroupMember>();
                x.CreateMap<UserVipInfo, GroupMember>();

                x.CreateMap<FriendMarkName, QQFriend>();
                x.CreateMap<FriendInfo, QQFriend>();
                x.CreateMap<UserVipInfo, QQFriend>();
            });
        }

        public static void Dispose()
        {
            Dispatcher.Dispose();
        }
    }
}
