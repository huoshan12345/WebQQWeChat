using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HttpActionTools.Actor;
using HttpActionTools.Event;

namespace HttpActionTools.Action
{
    public class ActionLink : IActionLink, IActionEventHandler
    {
        private readonly ManualResetEvent _waitHandle = new ManualResetEvent(false);
        private readonly ManualResetEvent _excuteHandle = new ManualResetEvent(false);
        private readonly object _syncObj = new object();
        private bool _isWaiting;
        private ActionEvent _finalEvent;
        private readonly ActionEventListener _outerListener;
        private CancellationTokenSource _cts;
        public IActorDispatcher ActorDispatcher { get; }
        private readonly Queue<IAction> _actions;


        public ActionLink(IActorDispatcher actorDispatcher, ActionEventListener listener = null)
        {
            _cts = new CancellationTokenSource();
            _actions = new Queue<IAction>();
            ActorDispatcher = actorDispatcher;
            _outerListener = listener;
            OnActionEvent += listener;
        }

        public void Cancel()
        {
            _cts.Cancel();
        }

        private void Excute()
        {
            while (_actions.Count != 0)
            {
                var action = _actions.Dequeue();
                ActorDispatcher.PushActor(action);
                action.OnActionEvent += SendEventToLink;
                _excuteHandle.WaitOne();
                action.OnActionEvent -= SendEventToLink;
            }
            _waitHandle.Set();
        }

        public void ExcuteAsync()
        {
            /*
             * Passing this token into the Task constructor associates it with this task.
                1. If the token has cancellation requested prior to the Task starting to execute, the Task won't execute. 
                    Rather than transitioning to Running, it'll immediately transition to Canceled. 
                    This avoids the costs of running the task if it would just be canceled while running anyway.
                2. If the body of the task is also monitoring the cancellation token and throws an OperationCanceledException containing that token 
                    (which is what ThrowIfCancellationRequested does), then when the task sees that OCE, it checks whether the OCE's token matches the Task's token. 
                    If it does, that exception is viewed as an acknowledgement of cooperative cancellation and the Task transitions to the Canceled state 
                    (rather than the Faulted state).             
             */
            Task.Run(() => Excute(), Token);
        }

        public CancellationToken Token => _cts.Token;

        public IActionLink PushAction(IAction action)
        {
            action.ActionLink = this;
            _actions.Enqueue(action);
            return this;
        }

        public void Terminate(IAction sender, ActionEvent actionEvent)
        {
            _actions.Clear();
            _finalEvent = actionEvent;
            _waitHandle.Set();
            _excuteHandle.Set();
        }

        private void SendEventToLink(IAction sender, ActionEvent actionEvent)
        {
            switch (actionEvent.Type)
            {
                case ActionEventType.EvtOK:
                {
                    _excuteHandle.Set();
                    break;
                }
                case ActionEventType.EvtCanceled:
                case ActionEventType.EvtError:
                {
                    Terminate(sender, actionEvent);
                    break;
                }
                case ActionEventType.EvtWrite:
                case ActionEventType.EvtRead:
                case ActionEventType.EvtRetry:
                default:
                {
                    break;
                }
            }
            OnActionEvent?.Invoke(sender, actionEvent);
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
            var ex = new Exception("this action is still waiting...");
            if (_isWaiting) throw ex;
            lock (_syncObj)
            {
                if (_isWaiting) throw ex;
                if (_cts.Token != token) _cts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, token);
                _isWaiting = true;
                _waitHandle.WaitOne();
                _isWaiting = false;
                return _finalEvent;
            }
        }

        public Task<ActionEvent> WaitFinalEventAsync()
        {
            return Task.Run(() => WaitFinalEvent(), CancellationToken.None);
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

        public event ActionEventListener OnActionEvent;
    }
}
