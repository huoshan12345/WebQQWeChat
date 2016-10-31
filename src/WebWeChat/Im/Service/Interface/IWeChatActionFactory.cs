using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility.HttpAction.Action;

namespace WebWeChat.Im.Service.Interface
{
    public interface IWeChatActionFactory: IActionFactory, IWeChatService
    {
    }
}
