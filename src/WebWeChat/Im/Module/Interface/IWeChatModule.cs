using System;
using WebWeChat.Im.Core;

namespace WebWeChat.Im.Module.Interface
{
    /// <summary>
    /// Module表示一个对象的一个模块（一个功能集合），与对象上下文有关
    /// </summary>
    public interface IWeChatModule : IDisposable
    {
        /// <summary>
        /// 对象上下文
        /// </summary>
        IWeChatContext Context { get; }
    }
}
