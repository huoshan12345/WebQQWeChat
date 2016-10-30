using HttpActionFrame.Actor;
using HttpActionFrame.Event;

namespace HttpActionFrame.Action
{
    public interface IAction : IActor, IActionEventHandler
    {
        void NotifyActionEvent(ActionEvent actionEvent);

        IActionFuture ActionFuture { get; set; }
    }

    public interface IAction<T> : IActor<T>, IActionEventHandler
    {
        void NotifyActionEvent(ActionEvent actionEvent);
    }
}
