using WebWeChat.Im.Bean;
using WebWeChat.Im.Core;

namespace WebWeChat.Im.Module.Impl
{
    public class AccountModule : WeChatModule
    {
        public ContactMember User { get; set; }

        public AccountModule(IWeChatContext context) : base(context)
        {
        }
    }
}
