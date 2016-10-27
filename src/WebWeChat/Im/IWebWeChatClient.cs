using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpActionFrame.Action;
using HttpActionFrame.Event;
using WebWeChat.Im.Core;

namespace WebWeChat.Im
{
    public interface IWebWeChatClient : IWeChatContext
    {
        IActionResult Login(ActionEventListener listener = null);
    }
}
