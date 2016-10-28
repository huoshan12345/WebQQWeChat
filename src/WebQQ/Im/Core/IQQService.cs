namespace WebQQ.Im.Core
{
    /**
     *
     * QQ服务
     *
     * 提供和模块与协议无关的公共服务，供模块调用，如定时服务，网络连接服务，异步任务服务等
     *
     * @author solosky
     */

    public enum QQServiceType
    {
        Timer, //定时服务
        Http, //HTTP
        Task, //异步任务，可以执行比较耗时的操作
        Udp,
        Log,
        Actor
    };

    public interface IQQService : IQQLifeCycle
    {

    }

}
