using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpActionTools.Core;

namespace HttpActionTools.Action
{
    public interface IHttpActionLink: IActionLink
    {
        IHttpService HttpService { get; }
    }
}
