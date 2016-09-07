using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HttpActionTools.Actor;
using HttpActionTools.Event;

namespace HttpActionTools.Action
{
    public interface IAction : IActor
    {
        IAction Begin();

        bool IsCanceled { get; }

        ActionEvent WaitFinalEvent();

        ActionEvent WaitFinalEvent(int second);

        ActionEvent WaitFinalEvent(CancellationToken token);

        Task<ActionEvent> WaitFinalEventAsync();

        Task<ActionEvent> WaitFinalEventAsync(int second);

        Task<ActionEvent> WaitFinalEventAsync(CancellationToken token);

        CancellationToken Token { get; }

        void Cancel();
    }
}
