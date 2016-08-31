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

        /// <summary>
        /// 等待事件的到来 Note:可能有最终会产生多个事件如EVT_READ, EVT_WRITE等，此时应该反复调用WaitEvent来获得需要的事件
        /// </summary>
        /// <returns></returns>
        QQActionEvent WaitEvent();

        bool IsCanceled { get; set; }

        /// <summary>
        /// 给定一个超时时间，等待事件到来
        /// </summary>
        /// <param name="timeoutMs">超时时间，毫秒为单位</param>
        /// <returns>超时抛出 WAIT_TIMEOUT， 等待被中断抛出WAIT_INTERUPPTED</returns>
        QQActionEvent WaitEvent(long timeoutMs);

        /// <summary>
        /// 等待最终的事件，通常是EVT_CANCELED,EVT_ERROR,EVT_OK
        /// </summary>
        /// <returns></returns>
        QQActionEvent WaitFinalEvent();

        /// <summary>
        /// 给定一个超时时间，等待最终的事件
        /// </summary>
        /// <param name="timeoutMs"></param>
        /// <returns></returns>
        QQActionEvent WaitFinalEvent(long timeoutMs);

        Task<QQActionEvent> WhenEvent();

        Task<QQActionEvent> WhenEvent(CancellationToken token);

        Task<QQActionEvent> WhenFinalEvent();

        Task<QQActionEvent> WhenFinalEvent(CancellationToken token);

        Task<QQActionEvent> WhenFinalEvent(int timeoutMs);
    }
}
