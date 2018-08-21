using System;
using System.Threading.Tasks;
using WebQQ.Im.Core;
using WebQQ.Im.Event;
using WebQQ.Im.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebQQ.Im.Service.Impl;
using AutoMapper;
using FclEx;
using FclEx.Http.Event;
using FclEx.Http.Services;
using FclEx.Log;
using WebIm.Im.Core;
using WebIm.Utils;
using WebQQ.Im.Bean;
using WebQQ.Im.Bean.Discussion;
using WebQQ.Im.Bean.Friend;
using WebQQ.Im.Bean.Group;
using WebQQ.Im.Modules.Impl;
using WebQQ.Im.Modules.Interface;
using WebQQ.Util;
using QQListener = FclEx.Utils.AsyncEventHandler<WebQQ.Im.Core.IQQClient, WebQQ.Im.Event.QQNotifyEvent>;

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
        }

        public static IServiceCollection CommonServices { get; } = new ServiceCollection();

        private readonly IServiceCollection _services;
        private readonly IServiceProvider _serviceProvider;
        private volatile bool _isInited;

        /// <summary>
        /// 构造方法，初始化模块和服务
        /// </summary>
        public WebQQClient(ILogger logger = null, QQListener notifyListener = null) : this(m =>
        {
            m.AddSingletonIfNotNull(logger)
                .AddSingletonIfNotNull(notifyListener);
        })
        {
        }

        /// <summary>
        /// 构造方法，初始化模块和服务
        /// </summary>
        public WebQQClient(Func<IQQContext, ILogger> func, QQListener notifyListener) : this(m =>
         {
             m.AddSingletonIf(p => func(p.GetRequiredService<IQQContext>()), func.IsNotNull())
                 .AddSingletonIfNotNull(notifyListener);
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
            _services.AddSingleton<IHttpService, LightHttpService>();
            _services.AddSingleton<IQQActionFactory, QQActionFactory>();
            configureServices?.Invoke(_services);

            _serviceProvider = _services.BuildServiceProvider();
            OnQQNotifyEvent += _serviceProvider.GetService<QQListener>();
            Logger = _serviceProvider.GetRequiredService<ILogger>();
            Http = _serviceProvider.GetRequiredService<IHttpService>();

            Init();
        }

        public T GetModule<T>() where T : IImModule
        {
            return _serviceProvider.GetRequiredService<T>();
        }

        public ILogger Logger { get; }
        public IHttpService Http { get; }

        /// <summary>
        /// 销毁所有模块和服务
        /// </summary>
        public void Dispose()
        {
            foreach (var service in _services)
            {
                try
                {
                    if (service.ImplementationInstance is IDisposable disposable)
                        if (!Equals(disposable, Logger)) disposable.Dispose();
                }
                catch (Exception e)
                {
                    Logger.LogError($"销毁模块和服务失败: {e}");
                }
            }
            (Logger as IDisposable)?.Dispose(); // 最后释放logger
        }

        public void Init()
        {
            if (_isInited) return;
            foreach (var service in _services)
            {
                try
                {
                    if (service.ImplementationInstance is IImModule module && module != this)
                        module.Init();
                }
                catch (Exception e)
                {
                    Logger.LogError($"初始化模块和服务失败: {e}");
                }
            }
            _isInited = true;
        }

        public async Task FireNotifyAsync(QQNotifyEvent notifyEvent)
        {
            try
            {
                await OnQQNotifyEvent.InvokeAsync(this, notifyEvent);
            }
            catch (Exception ex)
            {
                Logger.LogError($"FireNotify Error!! {ex}", ex);
            }
        }

        public ValueTask<ActionEvent> Login(ActionEventListener listener = null)
        {
            return GetModule<ILoginModule>().Login(listener);
        }

        public void BeginPoll()
        {
            GetModule<ILoginModule>().BeginPoll();
        }

        public IQQContext Context => this;

        public ValueTask<ActionEvent> SendMsg(Message msg, ActionEventListener listener = null)
        {
            return GetModule<IChatModule>().SendMsg(msg, listener);
        }

        public ValueTask<ActionEvent> GetRobotReply(RobotType robotType, string input, ActionEventListener listener = null)
        {
            throw new NotImplementedException();
        }

        public event QQListener OnQQNotifyEvent = (sender, @event) => Task.CompletedTask;
    }
}
