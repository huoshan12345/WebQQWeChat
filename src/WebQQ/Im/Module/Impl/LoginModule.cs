using System;
using System.Drawing;
using System.Threading.Tasks;
using HttpAction;
using HttpAction.Event;
using WebQQ.Im.Action;
using WebQQ.Im.Core;
using WebQQ.Im.Event;
using WebQQ.Im.Module.Interface;

namespace WebQQ.Im.Module.Impl
{
    /// <summary>
    /// <para>登录模块，处理登录和退出</para>
    /// </summary>
    public class LoginModule : QQModule, ILoginModule
    {
        public void BeginPoll()
        {
            throw new NotImplementedException();
        }

        public LoginModule(IQQContext context) : base(context)
        {
        }

        public Task<ActionEvent> Login(ActionEventListener listener)
        {
            return new QQActionFuture(Context, listener)
                .PushAction<GetQRCodeAction>(async (sender, @event) => // 1.获取二维码
                {
                    if (@event.Type == ActionEventType.EvtOK)
                    {
                        var verify = (Image) @event.Target;
                        await Context.FireNotifyAsync(new QQNotifyEvent(QQNotifyEventType.QRCodeReady, verify));
                    }
                })
                .ExecuteAsync();
        }
    }

}
