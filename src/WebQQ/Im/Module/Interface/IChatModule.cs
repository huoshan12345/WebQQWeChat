using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HttpAction.Event;
using WebQQ.Im.Bean;
using WebQQ.Util;

namespace WebQQ.Im.Module.Interface
{
    public interface IChatModule : IQQModule
    {
        Task<ActionEvent> SendMsg(Message msg, ActionEventListener listener = null);

        Task<ActionEvent> GetRobotReply(RobotType robotType, string input, ActionEventListener listener = null);
    }
}
