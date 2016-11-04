using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using HttpAction.Event;
using WebWeChat.Im.Action;
using WebWeChat.Im.Bean;
using WebWeChat.Im.Core;
using WebWeChat.Im.Event;
using WebWeChat.Im.Module.Interface;
using HttpAction;

namespace WebWeChat.Im.Module.Impl
{
    public enum RobotType
    {
        Tuling,
    }


    public class ChatModule : WeChatModule, IChatModule
    {
        public ChatModule(IWeChatContext context) : base(context)
        {
        }

        public Task<ActionEvent> SendMsg(MessageSent msg, ActionEventListener listener = null)
        {
            return new SendMsgAction(Context, msg)
                .ExecuteAsyncAuto();
        }

        public Task<ActionEvent> GetRobotReply(RobotType robotType, string input, ActionEventListener listener = null)
        {
            return new GetTuringRobotReplyAction(Context, input)
                .ExecuteAsyncAuto();
        }
    }
}
