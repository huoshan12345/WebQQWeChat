using System;
using WebQQ.Im.Core;

namespace WebQQ.Im.Service.Interface
{
    /// <summary>
    /// 就是用来表示该服务是属于一个QQ实例的
    /// 从而在QQ实例销毁的时候服务也能一起销毁
    /// 如果是全局的服务（即多个实例共享的）请不要继承该接口
    /// </summary>
    public interface IQQService : IDisposable
    {
        IQQContext Context { get; }
    }

}
