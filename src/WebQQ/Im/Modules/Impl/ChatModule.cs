using System;
using System.Threading.Tasks;
using FclEx.Http;
using FclEx.Http.Event;
using WebQQ.Im.Actions;
using WebQQ.Im.Bean;
using WebQQ.Im.Core;
using WebQQ.Im.Modules.Interface;
using WebQQ.Util;

namespace WebQQ.Im.Modules.Impl
{
    internal class ChatModule : QQModule, IChatModule
    {
        public ChatModule(IQQContext context) : base(context)
        {
        }

        public ValueTask<ActionEvent> SendMsg(Message msg, ActionEventListener listener = null)
        {
            return new SendMsgAction(Context, msg, listener).ExecuteAutoAsync();
        }

        public ValueTask<ActionEvent> GetRobotReply(RobotType robotType, string input, ActionEventListener listener = null)
        {
            throw new NotImplementedException();
            // return new GetTuringRobotReplyAction(Context, input).ExecuteAutoAsync();
        }
    }
}
