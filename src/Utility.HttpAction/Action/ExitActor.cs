using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utility.HttpAction.Event;

namespace Utility.HttpAction.Action
{
    /// <summary>
    /// 一个伪Actor只是为了让ActorLoop停下来
    /// </summary>
    public class ExitActor : IActor
    {
        public Task<ActionEvent> ExecuteAsync(CancellationToken token)
        {
            return Task.FromResult(ActionEvent.EmptyOkEvent);
        }
    }
}
