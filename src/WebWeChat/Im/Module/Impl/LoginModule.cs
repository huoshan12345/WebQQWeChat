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
    public class LoginModule : AbstractModule, ILoginModule
    {
        protected IActorDispatcher ActorDispatcher { get; private set; }

        public override void Init(IWeChatContext context)
        {
            base.Init(context);
            ActorDispatcher = context.GetSerivce<IActorDispatcher>();
        }

        public IActionResult Login(ActionEventListener listener)
        {
            var future = new ActionFuture(ActorDispatcher, listener);
            future.PushAction(new GetUuidAction(Context));
            future.PushAction(new GetQRCodeAction(Context, (sender, @event) =>
            {
                if (@event.Type == ActionEventType.EvtOK)
                {
                    var verify = (Image)@event.Target;
                    Context.FireNotify(new WeChatNotifyEvent(WeChatNotifyEventType.QRCodeReady, verify));
                }
            }));
            future.PushAction(new CheckQRCodeAction(1, Context, (sender, @event) =>
            {
                if (@event.Type == ActionEventType.EvtOK)
                {
                    Context.FireNotify(new WeChatNotifyEvent(WeChatNotifyEventType.QRCodeSuccess, null));
                }
            }));
            future.PushAction(new CheckQRCodeAction(0, Context));
            future.PushLastAction(new WebLoginAction(Context, (sender, @event) =>
            {
                if (@event.Type == ActionEventType.EvtOK)
                {
                    Context.FireNotify(new WeChatNotifyEvent(WeChatNotifyEventType.LoginSuccess));
                }
            }));
            return future;
        }
    }
}
