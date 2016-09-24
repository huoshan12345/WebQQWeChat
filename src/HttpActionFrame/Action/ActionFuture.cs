using System.Threading;
using System.Threading.Tasks;
using HttpActionFrame.Actor;
using HttpActionFrame.Event;

namespace HttpActionFrame.Action
{
    public class ActionFuture : IActionFuture
    {
        private ActionEvent _finalEvent;
        private readonly ActionEventListener _outerListener;
        private readonly ManualResetEvent _waitHandle = new ManualResetEvent(false);
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        public IActorDispatcher ActorDispatcher { get; }
        
        public ActionFuture(IActorDispatcher actorDispatcher, ActionEventListener listener = null)
        {
            _outerListener = listener;
            ActorDispatcher = actorDispatcher;
        }

        public void Cancel()
        {
            _cts.Cancel();
        }

        public CancellationToken Token => _cts.Token;

        public void PushAction(IAction action)
        {
            action.ActionFuture = this;
            action.OnActionEvent += _outerListener;
            action.OnActionEvent += SendNonLastEventToFuture;
            ActorDispatcher.PushActor(action);
        }

        public void PushEndAction(IAction action)
        {
            action.ActionFuture = this;
            action.OnActionEvent += _outerListener;
            action.OnActionEvent += SendLastEventToFuture;
            ActorDispatcher.PushActor(action);
        }

        private void SendLastEventToFuture(IAction sender, ActionEvent actionEvent)
        {
            SendEventToFuture(sender, actionEvent, true);
        }

        private void SendNonLastEventToFuture(IAction sender, ActionEvent actionEvent)
        {
            SendEventToFuture(sender, actionEvent, false);
        }

        private void SendEventToFuture(IAction sender, ActionEvent actionEvent, bool terminateWhenOk)
        {
            switch (actionEvent.Type)
            {
                case ActionEventType.EvtCanceled:
                case ActionEventType.EvtError:
                {
                    Terminate(sender, actionEvent);
                    break;
                }
            }
            if(terminateWhenOk && actionEvent.Type == ActionEventType.EvtOK) Terminate(sender, actionEvent);
        }

        public void Terminate(IAction sender, ActionEvent actionEvent)
        {
            _finalEvent = actionEvent;
            _waitHandle.Set();
        }

        public ActionEvent WaitFinalEvent()
        {
            return WaitFinalEvent(CancellationToken.None);
        }

        public ActionEvent WaitFinalEvent(int second)
        {
            _cts.CancelAfter(second * 1000);
            return WaitFinalEvent(_cts.Token);
        }

        public ActionEvent WaitFinalEvent(CancellationToken token)
        {
            _waitHandle.WaitOne();
            return _finalEvent;
        }

        public Task<ActionEvent> WaitFinalEventAsync()
        {
            return WaitFinalEventAsync(CancellationToken.None);
        }

        public Task<ActionEvent> WaitFinalEventAsync(int second)
        {
            var token = new CancellationTokenSource(second * 1000).Token;
            return WaitFinalEventAsync(token);
        }

        public Task<ActionEvent> WaitFinalEventAsync(CancellationToken token)
        {
            return Task.Run(() => WaitFinalEvent(token), token);
        }

        protected virtual bool IsFinalEvent(ActionEvent Event)
        {
            var type = Event.Type;
            return type == ActionEventType.EvtCanceled
                    || type == ActionEventType.EvtError
                    || type == ActionEventType.EvtOK;
        }
    }
}
