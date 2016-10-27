using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebWeChat.Im.Event
{
    public delegate void WeChatNotifyEventListener(IWebWeChatClient sender, WeChatNotifyEvent e);
}
