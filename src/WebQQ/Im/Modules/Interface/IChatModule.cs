using System.Threading.Tasks;
using FclEx.Http.Event;
using WebQQ.Im.Bean;
using WebQQ.Util;

namespace WebQQ.Im.Modules.Interface
{
    public interface IChatModule : IQQModule
    {
        ValueTask<ActionEvent> SendMsg(Message msg, ActionEventListener listener = null);

        ValueTask<ActionEvent> GetRobotReply(RobotType robotType, string input, ActionEventListener listener = null);
    }
}
