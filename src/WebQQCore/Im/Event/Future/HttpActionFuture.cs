using iQQ.Net.WebQQCore.Im.Http;

namespace iQQ.Net.WebQQCore.Im.Event.Future
{
    /**
     *
     * 用于异步等待操作完成
     *
     * 提交任何一个操作都会返回这个对象，可以在这个对象上:
     * 1. 同步等待操作完成
     * 3. 取消请求
     *
     * 注意: 千万不要在回调函数里面等待操作完成，否则会把整个客户端挂起
     *
     * @author solosky
     */
    public class HttpActionFuture : AbstractActionFuture
    {
        private readonly IHttpAction _httpAction;

        public HttpActionFuture(IHttpAction action)
            : base(action.Listener)
        {
            _httpAction = action;
            _httpAction.Listener = Listener;
            _httpAction.ActionFuture = this;
        }

        public override bool IsCancelable()
        {
            return _httpAction.IsCancelable();
        }

        public override void Cancel()
        {
            _httpAction.CancelRequest();
            base.Cancel();
        }
    }

}
