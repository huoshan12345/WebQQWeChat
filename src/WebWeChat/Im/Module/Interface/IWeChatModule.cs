using WebWeChat.Im.Core;

namespace WebWeChat.Im.Module.Interface
{
    /// <summary>
    /// Module表示一个对象的一个模块（一个功能集合），与对象上下文有关
    /// </summary>
    public interface IWeChatModule
    {
        /// <summary>
        /// 初始化，在使用之前调用
        /// </summary>
        /// <param name="context"></param>
        void Init(IWeChatContext context);

        /// <summary>
        /// 销毁，在使用完毕之后调用
        /// </summary>
        void Destroy();

        /// <summary>
        /// 对象上下文
        /// </summary>
        IWeChatContext Context { get; }
    }
}
