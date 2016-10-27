using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpActionFrame.Action;
using HttpActionFrame.Core;
using HttpActionFrame.Event;
using Utility.Extensions;
using WebWeChat.Im.Core;
using WebWeChat.Im.Module.Impl;

namespace WebWeChat.Im.Action
{
    public abstract class AbstractWebWeChatAction: AbstractHttpAction
    {
        protected SessionModule Session { get; private set; }
        protected long Timestamp => DateTime.Now.ToTimestamp();

        protected AbstractWebWeChatAction(IWeChatContext context, ActionEventListener listener) : 
            base(context.GetSerivce<IHttpService>(), listener)
        {
            Session = context.GetModule<SessionModule>();
        }
    }
}
