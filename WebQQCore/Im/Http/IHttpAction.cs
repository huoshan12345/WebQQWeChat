using System.Threading.Tasks;
using iQQ.Net.WebQQCore.Im.Event;

namespace iQQ.Net.WebQQCore.Im.Http
{
    public interface IHttpAction : IQQHttpListener
    {
        QQHttpRequest BuildRequest();

        void CancelRequest();

        bool IsCancelable();

        void NotifyActionEvent(QQActionEventType type, object target);

        QQActionListener Listener { get; set; }

        IQQActionFuture ActionFuture { set; }

        Task<QQHttpResponse> ResponseFuture { set; }
    }

}
