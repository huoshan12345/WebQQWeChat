using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebWeChat.Im.Core;

namespace WebWeChat.Im.Service.Interface
{
    public interface IWeChatService
    {
        /// <summary>
        /// 对象上下文
        /// </summary>
        IWeChatContext Context { get; }
    }
}
