using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HttpAction.Event;

namespace HttpAction.Action
{
    public class ActionFuture : IActionFuture
    {
        private readonly ActionEventListener _outerListener;
        private readonly LinkedList<IAction> _queue = new LinkedList<IAction>();

        public ActionFuture(ActionEventListener listener = null)
        {
            _outerListener = listener;
        }

        public virtual async Task<ActionEvent> ExecuteAsync(CancellationToken token)
        {
            ActionEvent lastEvent = null;
            while (_queue.Count != 0)
            {
                if (token.IsCancellationRequested)
                {
                    _queue.Clear();
                    return ActionEvent.CreateEvent(ActionEventType.EvtCanceled, this);
                }
                var action = _queue.First.Value;
                var result = await action.ExecuteAsync(token).ConfigureAwait(false); ;
                switch (result.Type)
                {
                    case ActionEventType.EvtOK:
                        _queue.RemoveFirst();
                        break;

                    case ActionEventType.EvtError:
                    case ActionEventType.EvtCanceled:
                        _queue.Clear();
                        return result;

                    case ActionEventType.EvtRetry:
                    case ActionEventType.EvtRepeat:
                        continue;
                }
                lastEvent = result;
            }
            return lastEvent ?? ActionEvent.EmptyOkEvent;
        }

        public virtual IActionFuture PushAction(IAction action)
        {
            action.OnActionEvent += _outerListener;
            _queue.AddLast(action);
            return this;
        }
    }
}
