namespace HttpAction.Event
{
    public interface IActionEventHandler
    {
        event ActionEventListener OnActionEvent;
    }
}
