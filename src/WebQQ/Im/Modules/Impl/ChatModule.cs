using System;
using System.Threading.Tasks;
using FclEx.Http.Event;
using WebQQ.Im.Actions;
using WebQQ.Im.Bean;
using WebQQ.Im.Core;
using WebQQ.Im.Modules.Interface;
using WebQQ.Util;

namespace WebQQ.Im.Modules.Impl
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
