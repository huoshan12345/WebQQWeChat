using System;
using System.Threading.Tasks;
using HttpAction.Event;

namespace HttpAction.Action
{
    public interface IAction : IActor, IActionEventHandler
    {
        Task<ActionEvent> HandleExceptionAsync(Exception ex);
    }
}
