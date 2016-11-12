using System;
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

namespace WebWeChat.Im
{
    public class WebWeChatClient : IWebWeChatClient
    {
        private readonly WeChatNotifyEventListener _notifyListener;
        private readonly ServiceCollection _services;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public WebWeChatClient(WeChatNotifyEventListener notifyListener = null)
        {
            _services = new ServiceCollection();
            Startup.ConfigureServices(_services);

            _services.AddSingleton<IWeChatContext>(this);

            // 模块
            _services.AddSingleton<ILoginModule, LoginModule>();
            _services.AddSingleton<IContactModule, ContactModule>();
            _services.AddSingleton<IChatModule, ChatModule>();
            _services.AddSingleton<StoreModule>();
            _services.AddSingleton<SessionModule>();

            // 服务
            _services.AddSingleton<IHttpService, WeChatHttp>();
            _services.AddSingleton<ILogger>(provider => new WeChatLogger(this, LogLevel.Information));
            _services.AddSingleton<IWeChatActionFactory, WeChatActionFactory>();

            _serviceProvider = _services.BuildServiceProvider();
            Startup.Configure(_serviceProvider);

            _notifyListener = notifyListener;
            _logger = GetSerivce<ILogger>();
        }

        public Task<ActionEvent> Login(ActionEventListener listener = null)
        {
            return GetModule<ILoginModule>().Login(listener);
        }

        public void BeginSyncCheck()
        {
            GetModule<ILoginModule>().BeginSyncCheck();
        }

        /// <inheritdoc />
        public void FireNotify(WeChatNotifyEvent notifyEvent)
        {
            try
            {
                _notifyListener?.Invoke(this, notifyEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError($"FireNotify Error!! {ex}", ex);
            }
        }

        public Task FireNotifyAsync(WeChatNotifyEvent notifyEvent)
        {
            return Task.Run(() => FireNotify(notifyEvent));
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
            try
            {
                foreach (var service in _services)
                {
                    var serviceType = service.ServiceType;
                    if (typeof(IDisposable).IsAssignableFrom(serviceType) &&
                        (typeof(IWeChatModule).IsAssignableFrom(serviceType)
                        || typeof(IWeChatService).IsAssignableFrom(serviceType)))
                    {
                        var obj = (IDisposable)service.ImplementationInstance;
                        obj.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"销毁所有模块和服务失败: {e}");
            }
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
    }
}
