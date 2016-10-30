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

        public virtual async Task<ActionEventType> ExecuteAsync(CancellationToken token)
        {
            while (_queue.Count != 0)
            {
                if (token.IsCancellationRequested) return ActionEventType.EvtCanceled;
                var action = _queue.First.Value;
                _queue.RemoveFirst();
                var result = await action.ExecuteAsync(token);
                if(result == ActionEventType.EvtRetry)
                {
                    _queue.AddFirst(action);
                }
                else if(result != ActionEventType.EvtOK)
                {
                    return result;
                }
            }
            return ActionEventType.EvtOK;
        }

        public virtual IActionFuture PushAction(IAction action)
        {
            action.OnActionEvent += _outerListener;
            _queue.AddLast(action);
            return this;
        }
    }
}
