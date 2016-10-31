using System.Threading.Tasks;
using Utility.HttpAction.Event;

namespace WebWeChat.Im.Module.Interface
{
    public interface IContactModule
    {
        Task<ActionEvent> GetContact(ActionEventListener listener = null);

        Task<ActionEvent> GetGroupMember(ActionEventListener listener = null);
    }
}
