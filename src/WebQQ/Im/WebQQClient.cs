using System;
using System.Linq;
using System.Threading.Tasks;
using HttpAction.Event;
using HttpAction.Service;
using WebQQ.Im.Core;
using WebQQ.Im.Event;
using WebQQ.Im.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebQQ.Im.Service.Impl;
using System.Reflection;
using AutoMapper;
using FclEx.Logger;
using Microsoft.Extensions.Configuration;
using WebQQ.Im.Bean;
using WebQQ.Im.Bean.Discussion;
using WebQQ.Im.Bean.Friend;
using WebQQ.Im.Bean.Group;
using WebQQ.Im.Modules.Impl;
using WebQQ.Im.Modules.Interface;
using WebQQ.Util;

namespace WebQQ.Im
{
    public class WebQQClient : IQQClient
    {
        static WebQQClient()
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

            CommonServices.AddSingleton<ILogger, EmptyLogger>();
            CommonServices.AddSingleton<IConfigurationRoot>(p => Startup.BuildConfig());
        }

        public static IServiceCollection CommonServices { get; } = new ServiceCollection();

        private readonly QQNotifyEventListener _notifyListener;
        private readonly IServiceCollection _services;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        /// <summary>
        /// 构造方法，初始化模块和服务
        /// </summary>
        public WebQQClient(ILogger logger = null, QQNotifyEventListener notifyListener = null) : this(m =>
        {
            if (logger != null) m.AddSingleton(logger);
            if (notifyListener != null) m.AddSingleton(notifyListener);
        })
        {
        }

        /// <summary>
        /// 构造方法，初始化模块和服务
        /// </summary>
        public WebQQClient(Func<IQQContext, ILogger> loggerFunc, QQNotifyEventListener notifyListener) : this(m =>
         {
             if (loggerFunc != null)
             {
                 m.AddSingleton<ILogger>(p => loggerFunc(p.GetRequiredService<IQQContext>()));
             }
             if (notifyListener != null) m.AddSingleton(notifyListener);
         })
        {
        }

        /// <summary>
        /// 构造方法，初始化模块和服务
        /// </summary>
        public WebQQClient(Action<IServiceCollection> configureServices = null)
        {
            _services = new ServiceCollection();
            foreach (var service in CommonServices)
            {
                _services.Add(service);
            }

            _services.AddSingleton<IQQContext>(this);

            // 模块
            _services.AddSingleton<ILoginModule, LoginModule>();
            _services.AddSingleton<IChatModule, ChatModule>();
            _services.AddSingleton<StoreModule>();
            _services.AddSingleton<SessionModule>();

            // 服务
            _services.AddSingleton<IHttpService, HttpService>();
            _services.AddSingleton<IQQActionFactory, QQActionFactory>();

            configureServices?.Invoke(_services);
            _serviceProvider = _services.BuildServiceProvider();

            _notifyListener = GetSerivce<QQNotifyEventListener>();
            _logger = GetSerivce<ILogger>();
        }

        public T GetSerivce<T>()
        {
            return _serviceProvider.GetService<T>();
        }

        public T GetModule<T>() where T : IQQModule
        {
            return _serviceProvider.GetRequiredService<T>();
        }

        /// <summary>
        /// 销毁所有模块和服务
        /// </summary>
        public void Dispose()
        {
            foreach (var service in _services.Where(m => m != null && typeof(IQQService).GetTypeInfo().IsAssignableFrom(m.ServiceType)))
            {
                try
                {
                    var obj = (IQQService)service.ImplementationInstance;
                    if (obj != _logger) obj.Dispose();
                }
                catch (Exception e)
                {
                    _logger.LogError($"销毁所有模块和服务失败: {e}");
                }
            }
            (_logger as IQQService)?.Dispose(); // 最后释放logger
        }

        public async Task FireNotifyAsync(QQNotifyEvent notifyEvent)
        {
            try
            {
                if (_notifyListener != null)
                {
                    await _notifyListener(this, notifyEvent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"FireNotify Error!! {ex}", ex);
            }
        }

        public Task<ActionEvent> Login(ActionEventListener listener = null)
        {
            return GetModule<ILoginModule>().Login(listener);
        }

        public void BeginPoll()
        {
            GetModule<ILoginModule>().BeginPoll();
        }

        public IQQContext Context => this;

        public Task<ActionEvent> SendMsg(Message msg, ActionEventListener listener = null)
        {
            return GetModule<IChatModule>().SendMsg(msg, listener);
        }

        public Task<ActionEvent> GetRobotReply(RobotType robotType, string input, ActionEventListener listener = null)
        {
            throw new NotImplementedException();
        }
    }
}
