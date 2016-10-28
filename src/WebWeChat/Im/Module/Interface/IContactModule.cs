using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using HttpActionFrame.Action;
using HttpActionFrame.Event;
using WebWeChat.Im.Action;
using WebWeChat.Im.Event;

namespace WebWeChat.Im.Module.Interface
{
    public interface IContactModule
    {
        IActionResult GetContact(ActionEventListener listener = null);

        IActionResult GetGroupMember(ActionEventListener listener = null);
    }
}
