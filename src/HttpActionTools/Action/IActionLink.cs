using System.Threading;
using System.Threading.Tasks;
using HttpActionTools.Actor;
using HttpActionTools.Event;

namespace HttpActionTools.Action
{
    public interface IActionLink
    {
        void Cancel();

        CancellationToken Token { get; }

        void PushAction(IAction action);

        void Terminate(IAction sender, ActionEvent actionEvent);

        ActionEvent WaitFinalEvent();

        ActionEvent WaitFinalEvent(int second);

        ActionEvent WaitFinalEvent(CancellationToken token);

        Task<ActionEvent> WaitFinalEventAsync();

        Task<ActionEvent> WaitFinalEventAsync(int second);

        Task<ActionEvent> WaitFinalEventAsync(CancellationToken token);
    }
}
