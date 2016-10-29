using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpActionFrame.Action;
using HttpActionFrame.Event;
using WebWeChat.Im.Action;
using WebWeChat.Im.Module.Interface;

namespace WebWeChat.Im.Module.Impl
{
    public class ContactModule : WeChatModule, IContactModule
    {
        public IActionResult GetContact(ActionEventListener listener = null)
        {
            return new WebWeChatActionFuture(Context, listener)
                .PushAction(new GetContactAction(Context), true);
        }

        public IActionResult GetGroupMember(ActionEventListener listener = null)
        {
            return new WebWeChatActionFuture(Context, listener)
                .PushAction(new BatchGetContactAction(), true);
        }
    }
}
