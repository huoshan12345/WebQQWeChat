using System.Threading.Tasks;
using HttpAction.Event;

namespace WebWeChat.Im.Module.Interface
{
    public interface IContactModule: IWeChatModule
    {
        Task<ActionEvent> GetContact(ActionEventListener listener = null);

        Task<ActionEvent> GetGroupMember(ActionEventListener listener = null);
    }
}
