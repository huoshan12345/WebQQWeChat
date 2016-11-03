using System.Threading.Tasks;
using Utility.HttpAction.Event;

namespace WebWeChat.Im.Module.Interface
{
    public interface ILoginModule: IWeChatModule
    {
        /// <summary>
        /// 登录，扫码方式
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        Task<ActionEvent> Login(ActionEventListener listener = null);

        /// <summary>
        /// 开始保持微信在线并检查新消息，即挂微信
        /// </summary>
        void BeginSyncCheck();
    }
}
