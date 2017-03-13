using WebQQ.Im.Core;
using WebQQ.Im.Service.Interface;

namespace WebQQ.Im.Module.Interface
{
    /// <summary>
    /// 模块功能接口
    /// </summary>
    public interface IQQModule
    {
        IQQContext Context { get; }
    }
}
