using System.Threading;
using System.Threading.Tasks;
using HttpActionFrame.Actor;
using HttpActionFrame.Event;

namespace HttpActionFrame.Action
{
    public class ActionFuture : IActionFuture
    {
        private readonly ManualResetEvent _waitHandle = new ManualResetEvent(false);
        private ActionEvent _finalEvent;
        private readonly ActionEventListener _outerListener;
        private readonly CancellationTokenSource _cts;
        public IActorDispatcher ActorDispatcher { get; }


        public ActionFuture(IActorDispatcher actorDispatcher, ActionEventListener listener = null)
        {
            _outerListener = listener;
            _cts = new CancellationTokenSource();
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
            action.OnActionEvent += SendEventToLink;
            ActorDispatcher.PushActor(action);
        }

        private void SendEventToLink(IAction sender, ActionEvent actionEvent)
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
