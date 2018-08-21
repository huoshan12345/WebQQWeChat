using System.Threading.Tasks;
using HttpAction.Event;
using WebWeChat.Im.Core;
using WebWeChat.Im.Module.Interface;
using HttpAction;
using WebWeChat.Im.Actions;

namespace WebWeChat.Im.Module.Impl
{
    public class ContactModule : WeChatModule, IContactModule
    {
        public ValueTask<ActionEvent> GetContact(ActionEventListener listener = null)
        {
            // 如果直接new一个Action并执行的话也可以，但是不能自动重试
            return new WebWeChatActionFuture(Context, listener)
                .PushAction<GetContactAction>()
                .ExecuteAsync();
        }

        public ValueTask<ActionEvent> GetGroupMember(ActionEventListener listener = null)
        {
            return new WebWeChatActionFuture(Context, listener)
               .PushAction<BatchGetContactAction>()
               .ExecuteAsync();
        }

        public ContactModule(IWeChatContext context) : base(context)
        {
        }
    }
}
