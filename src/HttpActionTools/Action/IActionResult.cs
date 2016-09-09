using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HttpActionTools.Event;

namespace HttpActionTools.Action
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
