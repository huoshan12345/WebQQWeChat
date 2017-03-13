using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebWeChat.Im.Core;

namespace WebWeChat.Im.Service.Interface
{
    /// <summary>
    /// 就是用来表示该服务是属于一个微信实例的
    /// 从而在微信实例销毁的时候服务也能一起销毁
    /// 如果是全局的服务（即多个实例共享的）请不要继承该接口
    /// </summary>
    public interface IWeChatService : IDisposable
    {
        IWeChatContext Context { get; }
    }
}
