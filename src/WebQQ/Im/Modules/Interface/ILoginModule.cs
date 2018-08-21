using System.Threading.Tasks;
using FclEx.Http.Event;

namespace WebQQ.Im.Modules.Interface
{
    public interface ILoginModule : IQQModule
    {
        /// <summary>
        /// 登录，扫码方式
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        ValueTask<ActionEvent> Login(ActionEventListener listener = null);

        /// <summary>
        /// 开始保持在线并检查新消息，即挂QQ
        /// </summary>
        void BeginPoll();
    }
}
