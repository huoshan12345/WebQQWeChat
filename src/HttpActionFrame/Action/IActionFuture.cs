using System.Threading;
using HttpActionFrame.Actor;
using HttpActionFrame.Event;

namespace HttpActionFrame.Action
{
    public interface IActionFuture: IActionResult, IActor
    {
        CancellationToken Token { get; }

        /// <summary>
        /// 放入一个action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IActionFuture PushAction(IAction action);

        /// <summary>
        /// 放入最后一个action，并执行Future
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IActionFuture PushLastAction(IAction action);

        void ExcuteAction(IAction action);

        void Terminate(IAction sender, ActionEvent actionEvent);
    }
}
