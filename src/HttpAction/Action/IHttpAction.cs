using System.Threading.Tasks;
using HttpAction.Core;
using HttpAction.Event;

namespace HttpAction.Action
{
    public interface IHttpAction : IAction
    {
        HttpRequestItem BuildRequest();

        Task<ActionEvent> HandleResponse(HttpResponseItem response);
    }
}
