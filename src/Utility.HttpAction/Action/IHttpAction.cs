using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility.HttpAction.Core;

namespace Utility.HttpAction.Action
{
    public interface IHttpAction : IAction, IHttpActionListener
    {
        HttpRequestItem BuildRequest();
    }
}
