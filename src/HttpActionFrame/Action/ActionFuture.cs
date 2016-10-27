using System;
using System.Collections.Generic;
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
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly Queue<IAction> _queue = new Queue<IAction>();

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

        public IActionFuture PushAction(IAction action)
        {
            action.ActionFuture = this;
            action.OnActionEvent += _outerListener;
            action.OnActionEvent += SendNonLastEventToFuture;
            _queue.Enqueue(action);
            return this;
        }

        public void ExcuteAction(IAction action)
        {
            ActorDispatcher.PushActor(action);
        }

        private void ExcuteNextAction()
        {
            if (_queue.Count == 0) return;
            var action = _queue.Dequeue();
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
                case ActionEventType.EvtOK:
                {
                    if (terminateWhenOk) Terminate(sender, actionEvent);
                    else ExcuteNextAction();
                    break;
                }
            }
            // sender.OnActionEvent -= _outerListener;
            // sender.OnActionEvent -= SendLastEventToFuture;
            // sender.OnActionEvent -= SendNonLastEventToFuture;
        }

        public void Terminate(IAction sender, ActionEvent actionEvent)
        {
            _queue.Clear();
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
            _cts = CancellationTokenSource.CreateLinkedTokenSource(token);
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

        public Task ExecuteAsync()
        {
            ExcuteNextAction();
            return Task.CompletedTask;
        }
    }
}
