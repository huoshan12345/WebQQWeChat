using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility.HttpAction.Core;
using Utility.HttpAction.Event;

namespace Utility.HttpAction.Action
{
    public interface IHttpAction : IAction
    {
        HttpRequestItem BuildRequest();

        ActionEvent HandleResponse(HttpResponseItem response);
    }
}
