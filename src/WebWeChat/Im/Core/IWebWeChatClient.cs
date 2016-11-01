using System;
using System.Threading.Tasks;
using Utility.HttpAction.Event;

namespace WebWeChat.Im.Core
{
    public interface IWebWeChatClient : IWeChatContext, IDisposable
    {
        /// <summary>
        /// 登录，扫码方式
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        Task<ActionEvent> Login(ActionEventListener listener = null);
    }
}
