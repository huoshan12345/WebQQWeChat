using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HttpAction;
using HttpAction.Event;
using WebQQ.Im.Action;
using WebQQ.Im.Bean;
using WebQQ.Im.Core;
using WebQQ.Im.Module.Interface;
using WebQQ.Util;

namespace WebQQ.Im.Module.Impl
{
    public class ChatModule : QQModule, IChatModule
    {
        public ChatModule(IQQContext context) : base(context)
        {
        }

        public Task<ActionEvent> SendMsg(Message msg, ActionEventListener listener = null)
        {
            return new SendMsgAction(Context, msg, listener).ExecuteAsyncAuto();
        }

        public Task<ActionEvent> GetRobotReply(RobotType robotType, string input, ActionEventListener listener = null)
        {
            throw new NotImplementedException();
            // return new GetTuringRobotReplyAction(Context, input).ExecuteAsyncAuto();
        }
    }
}
