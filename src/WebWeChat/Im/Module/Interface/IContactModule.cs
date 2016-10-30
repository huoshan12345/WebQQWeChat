using System.Threading.Tasks;
using Utility.HttpAction.Event;

namespace WebWeChat.Im.Module.Interface
{
    public interface IContactModule
    {
        Task<ActionEventType> GetContact(ActionEventListener listener = null);

        Task<ActionEventType> GetGroupMember(ActionEventListener listener = null);
    }
}
