using HttpActionFrame.Core;

namespace HttpActionFrame.Action
{
    public interface IHttpAction : IAction, IHttpActionListener
    {
        HttpRequestItem BuildRequest();
    }
}
