using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpActionFrame.Action;
using HttpActionFrame.Event;

namespace WebWeChat.Im
{
    public interface IWebWeChatClient
    {
        IActionResult Login(ActionEventListener listener = null);
    }
}
