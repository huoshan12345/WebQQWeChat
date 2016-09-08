using System.Drawing;
using HttpActionTools.Action;
using HttpActionTools.Event;
using iQQ.Net.WebQQCore.Im.Action;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;

namespace iQQ.Net.WebQQCore.Im.Module
{
    /// <summary>
    /// <para>登录模块，处理登录和退出</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class LoginModule : AbstractModule
    {
        public IActionLink LoginWithQRCode(ActionEventListener listener)
        {
            return GetQRCode(listener);
        }

        // 1.获取二维码
        private IActionLink GetQRCode(ActionEventListener listener)
        {
            var actionLink = new ActionLink(ActorDispatcher, listener);
            actionLink.PushAction(new GetQRCodeAction(Context, (sender, @event) =>
            {
                if (@event.Type == ActionEventType.EvtOK)
                {
                    var verify = (Image)@event.Target;
                    Context.FireNotify(new QQNotifyEvent(QQNotifyEventType.QrcodeReady, verify));
                }
            }));
            return actionLink;
        }


    }

}
