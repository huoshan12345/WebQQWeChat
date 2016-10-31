using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Utility.HttpAction.Event;
using WebWeChat.Im.Core;
using WebWeChat.Im.Event;
using WebWeChat.Im.Module;
using WebWeChat.Im.Module.Impl;
using WebWeChat.Im.Module.Interface;

namespace WebWeChat.Im
{
    public class WebWeChatClient : IWebWeChatClient
    {
        private readonly WeChatNotifyEventListener _notifyListener;
        private readonly ServiceCollection _services;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILoggerModule _logger;

        public WebWeChatClient(WeChatNotifyEventListener notifyListener = null)
        {
            _services = new ServiceCollection();
            Startup.ConfigureServices(_services);

            _services.AddSingleton<IWeChatContext>(this);
            _services.AddSingleton<IHttpModule, HttpModule>();
            _services.AddSingleton<ILoggerModule>(provider => new LoggerModule(this, LogLevel.Debug));
            _services.AddSingleton<ILoginModule, LoginModule>();

            // 以下三个就不以接口形式添加了
            _services.AddSingleton<StoreModule>();
            _services.AddSingleton<SessionModule>();
            _services.AddSingleton<AccountModule>();

            _serviceProvider = _services.BuildServiceProvider();
            Startup.Configure(_serviceProvider);

            _notifyListener = notifyListener;
            _logger = GetModule<ILoggerModule>();
        }

        public Task<ActionEvent> Login(ActionEventListener listener = null)
        {
            var login = GetModule<ILoginModule>();
            return login.Login(listener);
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

        /// <inheritdoc />
        public T GetSerivce<T>()
        {
            return _serviceProvider.GetService<T>();
        }
        /// <inheritdoc />
        public T GetModule<T>() where T : IWeChatModule
        {
            return _serviceProvider.GetService<T>();
        }

        ///// <summary>
        ///// 初始化所有模块和服务
        ///// </summary>
        //private void Init()
        //{
        //    try
        //    {
        //        foreach (var type in _modules.Keys)
        //        {
        //            var module = _modules[type];
        //            module.Init(this);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        _logger?.LogError(0, e, $"初始化模块和服务失败{e}");
        //    }
        //}

        /// <summary>
        /// 销毁所有模块和服务
        /// </summary>
        public void Dispose()
        {
            try
            {
                foreach (var service in _services.Where(s=>s.ServiceType.GetTypeInfo().IsAssignableFrom(typeof(IWeChatModule))))
                {
                    var obj = (IWeChatModule) service.ImplementationInstance;
                    obj.Dispose();
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"销毁所有模块和服务失败: {e}");
            }
        }
    }
}
