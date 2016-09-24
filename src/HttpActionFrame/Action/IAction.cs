using HttpActionFrame.Actor;
using HttpActionFrame.Event;

namespace HttpActionFrame.Action
{
    public interface IAction : IActor, IActionEventHandler
    {
        void NotifyActionEvent(ActionEvent actionEvent);

        IActionFuture ActionFuture { get; set; }
    }
}
