using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HttpActionTools.Actor;
using HttpActionTools.Event;

namespace HttpActionTools.Action
{
    public interface IActionLink: IActionResult
    {
        void ExcuteAsync();

        CancellationToken Token { get; }

        IActionLink PushAction(IAction action);

        void Terminate(IAction sender, ActionEvent actionEvent);
    }
}
