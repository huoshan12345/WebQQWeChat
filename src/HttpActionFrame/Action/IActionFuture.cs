using System.Threading;
using HttpActionFrame.Event;

namespace HttpActionFrame.Action
{
    public interface IActionFuture: IActionResult
    {
        CancellationToken Token { get; }

        void PushAction(IAction action);

        void PushEndAction(IAction action, bool autoExcute = true);

        void Terminate(IAction sender, ActionEvent actionEvent);

        void BeginExcute();
    }
}
