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
        private readonly LinkedList<IAction> _queue = new LinkedList<IAction>();

        public IActorDispatcher ActorDispatcher { get; }

        public ActionFuture(IActorDispatcher actorDispatcher, ActionEventListener listener = null)
        {
            _outerListener = listener;
            ActorDispatcher = actorDispatcher;
        }

        public virtual void Cancel()
        {
            _cts.Cancel();
        }

        public CancellationToken Token => _cts.Token;

        public virtual IActionFuture PushAction(IAction action, bool excute = false)
        {
            if (action.ActionFuture != this)
            {
                action.ActionFuture = this;
                action.OnActionEvent += _outerListener;
                action.OnActionEvent += SendEventToFuture;
            }
            _queue.AddLast(action);
            if (excute) ExecuteAsync();
            return this;
        }

        private void ExcuteNextAction()
        {
            if (_queue.Count == 0) return;
            var action = _queue.First.Value;
            _queue.RemoveFirst();
            ActorDispatcher.PushActor(action);
        }

        private void SendEventToFuture(IAction sender, ActionEvent actionEvent)
        {
            switch (actionEvent.Type)
            {
                case ActionEventType.EvtCanceled:
                case ActionEventType.EvtError:
                    Terminate(sender, actionEvent);
                    break;

                case ActionEventType.EvtOK:
                    if (_queue.Count == 0) Terminate(sender, actionEvent); // 如果是最后一个action就终止Future
                    else ExcuteNextAction();
                    break;

                case ActionEventType.EvtRetry:
                    _queue.AddFirst(sender);
                    break;
            }
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
            if (_queue.Count != 0)
            {
                ExcuteNextAction();
            }
            else
            {
                Terminate(null, ActionEvent.EmptyOkEvent);
            }
            return Task.CompletedTask;
        }
    }
}
