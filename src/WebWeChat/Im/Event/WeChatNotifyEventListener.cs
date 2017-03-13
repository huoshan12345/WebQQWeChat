using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebWeChat.Im.Core;

namespace WebWeChat.Im.Event
{
    public delegate Task WeChatNotifyEventListener(IWebWeChatClient sender, WeChatNotifyEvent e);
}
