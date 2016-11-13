using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HttpAction.Action;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebQQ.Im.Bean;
using WebQQ.Im.Bean.Discussion;
using WebQQ.Im.Bean.Friend;
using WebQQ.Im.Bean.Group;

namespace WebQQ
{
    public static class Startup
    {
        private static IConfigurationRoot BuildConfig()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            if (builder.GetFileProvider().GetFileInfo("project.json")?.Exists == true)
            {
                builder.AddUserSecrets();
            }
            return builder.Build();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(p => BuildConfig());
        }

        public static void Configure(IServiceProvider provider)
        {
            Mapper.Initialize(x =>
            {
                x.CreateMap<SelfInfo, QQUser>();

                x.CreateMap<GroupInfo, QQGroup>();
                x.CreateMap<GroupMemberCard, GroupMember>();
                x.CreateMap<GroupMemberInfo, GroupMember>();
                x.CreateMap<UserStatus, GroupMember>();
                x.CreateMap<UserVipInfo, GroupMember>();

                x.CreateMap<FriendMarkName, QQFriend>();
                x.CreateMap<FriendBaseInfo, QQFriend>();
                x.CreateMap<UserVipInfo, QQFriend>();
                x.CreateMap<FriendOnlineInfo, QQFriend>();
                x.CreateMap<FriendInfo, QQFriend>();
                x.CreateMap<DiscussionMemberStatus, DiscussionMember>();
            });
        }

        public static void Dispose()
        {
        }
    }
}
