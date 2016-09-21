using HttpActionFrame.Action;

namespace HttpActionFrame.Event
{
    public delegate void ActionEventListener(IAction sender, ActionEvent actionEvent);

    public interface IActionEventHandler
    {
        event ActionEventListener OnActionEvent;
    }
}
