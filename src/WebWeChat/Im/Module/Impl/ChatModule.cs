using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Utility.HttpAction;
using Utility.HttpAction.Event;
using WebWeChat.Im.Action;
using WebWeChat.Im.Bean;
using WebWeChat.Im.Core;
using WebWeChat.Im.Event;
using WebWeChat.Im.Module.Interface;

namespace WebWeChat.Im.Module.Impl
{
    public class ChatModule : WeChatModule, IChatModule
    {
        public ChatModule(IWeChatContext context) : base(context)
        {
        }

        public Task<ActionEvent> SendMsg(MessageSent msg, ActionEventListener listener = null)
        {
            return new SendMsgAction(Context, msg)
                .ExecuteAsync();
        }
    }
}
