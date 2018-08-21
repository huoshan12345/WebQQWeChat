using System.Threading.Tasks;
using HttpAction.Event;
using WebWeChat.Im.Bean;
using WebWeChat.Im.Core;
using WebWeChat.Im.Module.Interface;
using HttpAction;
using WebWeChat.Im.Actions;

namespace WebWeChat.Im.Module.Impl
{
    public class ChatModule : WeChatModule, IChatModule
    {
        public ChatModule(IWeChatContext context) : base(context)
        {
        }

        public ValueTask<ActionEvent> SendMsg(MessageSent msg, ActionEventListener listener = null)
        {
            return new SendMsgAction(Context, msg, listener).ExecuteAutoAsync();
        }

        public ValueTask<ActionEvent> GetRobotReply(RobotType robotType, string input, ActionEventListener listener = null)
        {
            return new GetTuringRobotReplyAction(Context, input).ExecuteAutoAsync();
        }
    }
}
