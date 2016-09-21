using System.Threading;
using System.Threading.Tasks;
using HttpActionFrame.Event;

namespace HttpActionFrame.Action
{
    public interface IActionResult
    {
        void Cancel();

        ActionEvent WaitFinalEvent();

        ActionEvent WaitFinalEvent(int second);

        ActionEvent WaitFinalEvent(CancellationToken token);

        Task<ActionEvent> WaitFinalEventAsync();

        Task<ActionEvent> WaitFinalEventAsync(int second);

        Task<ActionEvent> WaitFinalEventAsync(CancellationToken token);
    }
}
