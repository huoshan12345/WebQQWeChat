using System.Threading;
using HttpActionFrame.Actor;
using HttpActionFrame.Event;

namespace HttpActionFrame.Action
{
    public interface IActionFuture: IActionResult, IActor
    {
        CancellationToken Token { get; }

        IActionFuture PushAction(IAction action);

        void ExcuteAction(IAction action);

        void Terminate(IAction sender, ActionEvent actionEvent);
    }
}
