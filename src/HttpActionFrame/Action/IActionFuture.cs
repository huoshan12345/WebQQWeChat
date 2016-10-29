using System;
using System.Threading;
using HttpActionFrame.Actor;
using HttpActionFrame.Event;

namespace HttpActionFrame.Action
{
    public interface IActionFuture : IActionResult, IActor
    {
        CancellationToken Token { get; }

        /// <summary>
        /// 放入一个action到执行队列末尾
        /// </summary>
        /// <param name="action"></param>
        /// <param name="excuteFuture">是否开始执行Future</param>
        /// <returns></returns>
        IActionFuture PushAction(IAction action, bool excuteFuture = false);

        void Terminate(IAction sender, ActionEvent actionEvent);
    }
}
