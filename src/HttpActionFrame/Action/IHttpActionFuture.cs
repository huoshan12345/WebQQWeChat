using HttpActionFrame.Core;

namespace HttpActionFrame.Action
{
    public interface IHttpActionFuture: IActionFuture
    {
        IHttpService HttpService { get; }
    }
}
