using System;
using System.Threading.Tasks;
using HttpAction.Event;
using HttpAction.Service;
using WebQQ.Im.Core;
using WebQQ.Im.Event;
using WebQQ.Im.Module.Impl;
using WebQQ.Im.Module.Interface;
using WebQQ.Im.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebQQ.Im.Service.Impl;
using System.Reflection;

namespace WebQQ.Im
{
    public class WebQQClient : IQQClient
    {
        private readonly QQNotifyEventListener _notifyListener;
        private readonly IServiceCollection _services;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        /// <summary>
        /// 构造方法，初始化模块和服务
        /// </summary>
        public WebQQClient(QQNotifyEventListener notifyListener = null)
        {
            _services = new ServiceCollection();
            Startup.ConfigureServices(_services);

            _services.AddSingleton<IQQContext>(this);

            // 模块
            _services.AddSingleton<ILoginModule, LoginModule>();
            _services.AddSingleton<StoreModule>();
            _services.AddSingleton<SessionModule>();

            // 服务
            _services.AddSingleton<IHttpService, QQHttp>();
            _services.AddSingleton<ILogger>(new QQLogger(this, LogLevel.Debug));
            _services.AddSingleton<IQQActionFactory, QQActionFactory>();

            _serviceProvider = _services.BuildServiceProvider();
            Startup.Configure(_serviceProvider);

            _notifyListener = notifyListener;
            _logger = GetSerivce<ILogger>();
        }
        
        public T GetSerivce<T>()
        {
            return _serviceProvider.GetRequiredService<T>();
        }
        
        public T GetModule<T>() where T : IQQModule
        {
            return _serviceProvider.GetRequiredService<T>();
        }

        /// <summary>
        /// 销毁所有模块和服务
        /// </summary>
        /// 
        public void Dispose()
        {
            try
            {
                foreach (var service in _services)
                {
                    var serviceType = service.ServiceType;
                    if (typeof(IDisposable).IsAssignableFrom(serviceType) &&
                        (typeof(IQQModule).IsAssignableFrom(serviceType)
                        || typeof(IQQService).IsAssignableFrom(serviceType)))
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

        public void FireNotify(QQNotifyEvent notifyEvent)
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

        public Task FireNotifyAsync(QQNotifyEvent notifyEvent)
        {
            return Task.Run(() => FireNotify(notifyEvent));
        }

        public Task<ActionEvent> Login(ActionEventListener listener = null)
        {
           return GetModule<ILoginModule>().Login(listener);
        }

        public void BeginPoll()
        {
            GetModule<ILoginModule>().BeginPoll();
        }
    }
}
