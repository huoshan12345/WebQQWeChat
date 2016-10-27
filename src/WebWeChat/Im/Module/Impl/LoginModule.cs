using System.Drawing;
using HttpActionFrame.Action;
using HttpActionFrame.Actor;
using HttpActionFrame.Event;
using WebWeChat.Im.Action;
using WebWeChat.Im.Core;
using WebWeChat.Im.Event;
using WebWeChat.Im.Module.Interface;

namespace WebWeChat.Im.Module.Impl
{
    public class LoginModule: AbstractModule, ILoginModule
    {
        protected IActorDispatcher ActorDispatcher { get; private set; }

        public override void Init(IWeChatContext context)
        {
            base.Init(context);
            ActorDispatcher = context.GetSerivce<IActorDispatcher>();
        }

        public IActionResult Login(ActionEventListener listener)
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
                    var verify = (Image)@event.Target;
                    Context.FireNotify(new WeChatNotifyEvent(WeChatNotifyEventType.QRcodeReady, verify));
                }
            }));// .ExcuteAsync();
            return actionLink;
        }
    }
}
