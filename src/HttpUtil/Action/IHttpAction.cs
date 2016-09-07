using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HttpActionTools.Core;
using HttpActionTools.Event;

namespace HttpActionTools.Action
{
    public interface IHttpAction : IAction, IHttpActionListener
    {
        HttpRequestItem BuildRequest();

        void NotifyActionEvent(ActionEvent actionEvent);


    }
}
