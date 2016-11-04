using HttpAction.Action;

namespace HttpAction.Event
{
    public delegate void ActionEventListener(IAction sender, ActionEvent actionEvent);
}
