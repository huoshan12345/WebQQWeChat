using System.Threading;
using System.Threading.Tasks;
using Utility.HttpAction.Event;
using WebWeChat.Im.Action;
using WebWeChat.Im.Module.Interface;

namespace WebWeChat.Im.Module.Impl
{
    public class ContactModule : WeChatModule, IContactModule
    {
        public Task<ActionEventType> GetContact(ActionEventListener listener = null)
        {
            return new WebWeChatActionFuture(Context, listener)
                .PushAction(new GetContactAction(Context)).ExecuteAsync(CancellationToken.None);
        }

        public Task<ActionEventType> GetGroupMember(ActionEventListener listener = null)
        {
            return new WebWeChatActionFuture(Context, listener)
                .PushAction(new BatchGetContactAction()).ExecuteAsync(CancellationToken.None);
        }
    }
}
