using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpActionFrame.Action;
using HttpActionFrame.Event;

namespace WebWeChat.Im
{
    public interface IWeChatClient
    {
        IActionResult Login(ActionEventListener listener);
    }
}
