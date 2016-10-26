using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpActionFrame.Action;
using HttpActionFrame.Core;
using HttpActionFrame.Event;
using WebWeChat.Im.Core;

namespace WebWeChat.Im.Action
{
    public abstract class AbstractWebWeChatAction: AbstractHttpAction
    {
        protected AbstractWebWeChatAction(IWeChatContext context, ActionEventListener listener) : 
            base(context.GetSerivce<IHttpService>(), listener)
        {
        }
    }
}
