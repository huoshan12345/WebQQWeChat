using System.Drawing;
using HttpActionFrame.Action;
using HttpActionFrame.Event;
using iQQ.Net.WebQQCore.Im.Action;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;

namespace iQQ.Net.WebQQCore.Im.Module
{
    /// <summary>
    /// <para>登录模块，处理登录和退出</para>
    /// </summary>
    public class LoginModule : AbstractModule
    {
        public IActionResult LoginWithQRCode(ActionEventListener listener)
        {
            return GetQRCode(listener);
        }

        // 1.获取二维码
        private IActionResult GetQRCode(ActionEventListener listener)
        {
            var actionLink = new ActionFuture(ActorDispatcher, listener);
            actionLink.PushEndAction(new GetQRCodeAction(Context, (sender, @event) =>
            {
                if (@event.Type == ActionEventType.EvtOK)
                {
                    var verify = (Image) @event.Target;
                    Context.FireNotify(new QQNotifyEvent(QQNotifyEventType.QrcodeReady, verify));
                }
            }));// .ExcuteAsync();
            return actionLink;
        }

        private void DoCheckQRCode(IActionFuture future)
        {
            
        }


    }

}
