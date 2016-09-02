using System.Threading;
using System.Threading.Tasks;

namespace iQQ.Net.WebQQCore.Im.Event
{
    /// <summary>
    /// qq动作的执行结果
    /// </summary>
    public interface IQQActionFuture
    {
        /// <summary>
        /// 判断这个操作是否可以取消
        /// </summary>
        /// <returns></returns>
        bool IsCancelable();

        /// <summary>
        /// 尝试取消操作
        /// </summary>
        void Cancel();

        bool IsCanceled { get; }

        /// <summary>
        /// 等待最终的事件，通常是EVT_CANCELED,EVT_ERROR,EVT_OK
        /// 不抛出异常
        /// </summary>
        /// <returns></returns>
        QQActionEvent WaitFinalEvent();

        /// <summary>
        /// 给定一个超时时间，等待最终的事件
        /// </summary>
        /// <param name="timeoutMs"></param>
        /// <returns></returns>
        QQActionEvent WaitFinalEvent(long timeoutMs);

        Task<QQActionEvent> WhenFinalEvent();

        Task<QQActionEvent> WhenFinalEvent(long timeoutMs);
    }
}
