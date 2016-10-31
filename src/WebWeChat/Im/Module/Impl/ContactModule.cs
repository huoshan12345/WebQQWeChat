using System.Threading;
using System.Threading.Tasks;
using Utility.HttpAction.Event;
using WebWeChat.Im.Action;
using WebWeChat.Im.Core;
using WebWeChat.Im.Module.Interface;

namespace WebWeChat.Im.Module.Impl
{
    public class ContactModule : WeChatModule, IContactModule
    {
        public Task<ActionEvent> GetContact(ActionEventListener listener = null)
        {
            return new GetContactAction(Context, listener).ExecuteAsync(CancellationToken.None);
        }

        public Task<ActionEvent> GetGroupMember(ActionEventListener listener = null)
        {
            return new BatchGetContactAction(Context, listener).ExecuteAsync(CancellationToken.None);
        }

        public ContactModule(IWeChatContext context) : base(context)
        {
        }
    }
}
