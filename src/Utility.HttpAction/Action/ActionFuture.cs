using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utility.HttpAction.Event;

namespace Utility.HttpAction.Action
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
                if (token.IsCancellationRequested) return ActionEvent.CreateEvent(ActionEventType.EvtCanceled, this);
                var action = _queue.First.Value;
                _queue.RemoveFirst();
                var result = await action.ExecuteAsync(token).ConfigureAwait(false);
                if (result.Type == ActionEventType.EvtRetry || result.Type == ActionEventType.EvtRepeat)
                {
                    _queue.AddFirst(action);
                }
                else if(result.Type != ActionEventType.EvtOK)
                {
                    return result;
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
