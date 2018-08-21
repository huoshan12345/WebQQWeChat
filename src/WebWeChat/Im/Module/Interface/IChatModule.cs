using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpAction.Event;
using WebWeChat.Im.Bean;
using WebWeChat.Im.Module.Impl;

namespace WebWeChat.Im.Module.Interface
{
    public interface IChatModule : IWeChatModule
    {
        ValueTask<ActionEvent> SendMsg(MessageSent msg, ActionEventListener listener = null);

        ValueTask<ActionEvent> GetRobotReply(RobotType robotType, string input, ActionEventListener listener = null);
    }
}
