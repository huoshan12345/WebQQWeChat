using System;
using System.Linq;
using System.Threading.Tasks;
using HttpAction.Event;
using HttpAction.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebWeChat.Im.Bean;
using WebWeChat.Im.Core;
using WebWeChat.Im.Event;
using WebWeChat.Im.Module.Impl;
using WebWeChat.Im.Module.Interface;
using WebWeChat.Im.Service.Impl;
using WebWeChat.Im.Service.Interface;
using System.Reflection;
using FclEx.Logger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace WebWeChat.Im
{
    public class WebWeChatClient : IWebWeChatClient
    {
        private readonly WeChatNotifyEventListener _notifyListener;
        private readonly ServiceCollection _services;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public static IServiceCollection CommonServices { get; } = new ServiceCollection();

        static WebWeChatClient()
        {
            CommonServices.AddSingleton<ILogger, EmptyLogger>();
            CommonServices.AddSingleton<IConfigurationRoot>(p => Startup.BuildConfig());
        }

        public WebWeChatClient(Action<IServiceCollection> configureServices = null)
        {
            _services = new ServiceCollection();
            foreach (var service in CommonServices)
            {
                _services.Add(service);
            }
            _services.AddSingleton<IWeChatContext>(this);

            // 模块
            _services.AddSingleton<ILoginModule, LoginModule>();
            _services.AddSingleton<IContactModule, ContactModule>();
            _services.AddSingleton<IChatModule, ChatModule>();
            _services.AddSingleton<StoreModule>();
            _services.AddSingleton<SessionModule>();

            // 服务
            _services.AddSingleton<IHttpService, HttpService>();
            _services.AddSingleton<IWeChatActionFactory, WeChatActionFactory>();

            configureServices?.Invoke(_services);
            _serviceProvider = _services.BuildServiceProvider();

            _notifyListener = GetSerivce<WeChatNotifyEventListener>();
            _logger = GetSerivce<ILogger>();
        }

        /// <summary>
        /// 构造方法，初始化模块和服务
        /// </summary>
        public WebWeChatClient(ILogger logger, WeChatNotifyEventListener notifyListener) : this(m =>
        {
            if (logger != null) m.AddSingleton(logger);
            if (notifyListener != null) m.AddSingleton(notifyListener);
        })
        {
        }

        /// <summary>
        /// 构造方法，初始化模块和服务
        /// </summary>
        public WebWeChatClient(Func<IWeChatContext, ILogger> loggerFunc, WeChatNotifyEventListener notifyListener) : this(m =>
         {
             if (loggerFunc != null)
             {
                 m.AddSingleton<ILogger>(p => loggerFunc(p.GetRequiredService<IWeChatContext>()));
             }
             if (notifyListener != null) m.AddSingleton(notifyListener);
         })
        {
        }

        public Task<ActionEvent> Login(ActionEventListener listener = null)
        {
            return GetModule<ILoginModule>().Login(listener);
        }

        public void BeginSyncCheck()
        {
            GetModule<ILoginModule>().BeginSyncCheck();
        }

        public async Task FireNotifyAsync(WeChatNotifyEvent notifyEvent)
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

        /// <inheritdoc />
        public T GetSerivce<T>()
        {
            return _serviceProvider.GetRequiredService<T>();
        }

        /// <inheritdoc />
        public T GetModule<T>() where T : IWeChatModule
        {
            return _serviceProvider.GetRequiredService<T>();
        }

        /// <summary>
        /// 销毁所有模块和服务
        /// </summary>
        public void Dispose()
        {
            foreach (var service in _services.Where(m => m != null && typeof(IWeChatService).GetTypeInfo().IsAssignableFrom(m.ServiceType)))
            {
                try
                {
                    var obj = (IWeChatService)service.ImplementationInstance;
                    if (obj != _logger) obj.Dispose();
                }
                catch (Exception e)
                {
                    _logger.LogError($"销毁所有模块和服务失败: {e}");
                }
            }
            (_logger as IWeChatService)?.Dispose(); // 最后释放logger
        }

        public Task<ActionEvent> GetContact(ActionEventListener listener = null)
        {
            return GetModule<IContactModule>().GetContact(listener);
        }

        public Task<ActionEvent> GetGroupMember(ActionEventListener listener = null)
        {
            return GetModule<IContactModule>().GetGroupMember(listener);
        }

        public Task<ActionEvent> SendMsg(MessageSent msg, ActionEventListener listener = null)
        {
            return GetModule<IChatModule>().SendMsg(msg, listener);
        }

        public Task<ActionEvent> GetRobotReply(RobotType robotType, string input, ActionEventListener listener = null)
        {
            return GetModule<IChatModule>().GetRobotReply(robotType, input, listener);
        }

        public IWeChatContext Context => this;
    }
}
