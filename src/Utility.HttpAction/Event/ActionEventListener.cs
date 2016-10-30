using Utility.HttpAction.Action;

namespace Utility.HttpAction.Event
{
    public delegate void ActionEventListener(IAction sender, ActionEvent actionEvent);
}
