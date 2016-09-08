namespace iQQ.Net.WebQQCore.Im.Core
{

    /// <summary>
    /// 生命周期管理
    /// 实现了这个接口就可以支持对象的重复使用
    /// </summary>
    public interface IQQLifeCycle
    {


        /// <summary>
        /// 初始化，在使用之前调用
        /// </summary>
        /// <param name="context"></param>
        void Init(IQQContext context);

        /// <summary>
        /// 销毁，在使用完毕之后调用
        /// </summary>
        void Destroy();
    }
}