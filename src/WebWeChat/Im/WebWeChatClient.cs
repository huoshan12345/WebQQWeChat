using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpActionFrame.Action;
using HttpActionFrame.Actor;
using HttpActionFrame.Event;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebWeChat.Im.Core;
using WebWeChat.Im.Event;
using WebWeChat.Im.Module;
using WebWeChat.Im.Module.Impl;
using WebWeChat.Im.Module.Interface;

namespace WebWeChat.Im
{
    public class WebWeChatClient : IWebWeChatClient
    {
        private readonly Dictionary<Type, IWeChatModule> _modules;
        private readonly WeChatNotifyEventListener _notifyListener;
        private readonly IServiceProvider _services;
        private readonly ILoggerModule _logger;

        public WebWeChatClient(IServiceProvider services, WeChatNotifyEventListener notifyListener = null)
        {
            if(services == null) throw new ArgumentNullException(nameof(services));
            _services = services;
            _notifyListener = notifyListener;
            _modules = new Dictionary<Type, IWeChatModule>
            {
                [typeof(ILoginModule)] = GetSerivce<ILoginModule>(),
                [typeof(ILoggerModule)] = GetSerivce<ILoggerModule>(),
                [typeof(IHttpModule)] = GetSerivce<IHttpModule>(),

                [typeof(StoreModule)] = GetSerivce<StoreModule>(),
                [typeof(SessionModule)] = GetSerivce<SessionModule>(),
                [typeof(AccountModule)] = GetSerivce<AccountModule>(),
            };
            _logger = GetModule<ILoggerModule>();

            Init();
        }

        public IActionResult Login(ActionEventListener listener = null)
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
            return _services.GetService<T>();
        }
        /// <inheritdoc />
        public T GetModule<T>() where T : IWeChatModule
        {
            return (T)_modules[typeof(T)];
        }

        /// <summary>
        /// 初始化所有模块和服务
        /// </summary>
        private void Init()
        {
            try
            {
                foreach (var type in _modules.Keys)
                {
                    var module = _modules[type];
                    module.Init(this);
                }
            }
            catch (Exception e)
            {
                _logger?.LogError(0, e, $"初始化模块和服务失败{e}");
            }
        }

        /// <summary>
        /// 销毁所有模块和服务
        /// </summary>
        private void Destroy()
        {
            try
            {
                foreach (var module in _modules.Values)
                {
                    module.Destroy();
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"销毁所有模块和服务失败: {e}");
            }
        }

        public void Dispose()
        {
            Destroy();
        }
    }
}
