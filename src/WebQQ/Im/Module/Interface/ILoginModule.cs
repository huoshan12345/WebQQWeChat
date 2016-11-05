using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpAction.Event;

namespace WebQQ.Im.Module.Interface
{
    public interface ILoginModule : IQQModule
    {
        /// <summary>
        /// 登录，扫码方式
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        Task<ActionEvent> Login(ActionEventListener listener = null);

        /// <summary>
        /// 开始保持在线并检查新消息，即挂微信
        /// </summary>
        void BeginPoll();
    }
}
