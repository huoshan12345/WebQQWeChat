using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpActionFrame.Core;
using HttpActionFrame.Event;
using WebWeChat.Im.Core;

namespace WebWeChat.Im.Action
{
    public class GetQRCodeAction : AbstractWebWeChatAction
    {
        public GetQRCodeAction(IWeChatContext context, ActionEventListener listener) : base(context, listener)
        {
        }

        public override HttpRequestItem BuildRequest()
        {
            throw new NotImplementedException();
        }
    }
}
