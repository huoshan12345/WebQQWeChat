using System;
using System.Threading.Tasks;
using iQQ.Net.WebQQCore.Im.Action;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Im.Service;

namespace iQQ.Net.WebQQCore.Im.Actor
{
    public enum HttpActorType
    {
        BuildRequest,
        CancelRequest,
        OnHttpError,
        OnHttpFinish,
        OnHttpHeader,
        OnHttpWrite,
        OnHttpRead
    }

    public class HttpActor : IQQActor
    {
        private readonly HttpActorType _type;
        private readonly IQQContext _context;
        private readonly IHttpAction _action;
        private readonly QQHttpResponse _response;
        private readonly Exception _throwable;
        private readonly long _current;
        private readonly long _total;

        public void Execute()
        {
            if (_action.ActionFuture.IsCanceled)
            {
                return;
            }
            try
            {
                switch (_type)
                {
                    case HttpActorType.BuildRequest:
                        var service = _context.GetSerivce<IHttpService>(QQServiceType.HTTP);
                        var adaptor = new HttpAdaptor(_context, _action);
                        var request = _action.BuildRequest();
                        service.ExecuteHttpRequest(request, adaptor);
                        break;

                    case HttpActorType.CancelRequest:
                        _action.CancelRequest();
                        break;

                    case HttpActorType.OnHttpError:
                        _action.OnHttpError(_throwable);
                        break;

                    case HttpActorType.OnHttpFinish:
                        _action.OnHttpFinish(_response);
                        break;

                    case HttpActorType.OnHttpHeader:
                        _action.OnHttpHeader(_response);
                        break;

                    case HttpActorType.OnHttpRead:
                        _action.OnHttpRead(_current, _total);
                        break;

                    case HttpActorType.OnHttpWrite:
                        _action.OnHttpWrite(_current, _total);
                        break;
                }
            }
            // 统一异常处理
            catch (Exception ex)
            {
                var qqEx = ex as QQException ?? new QQException(ex);
                // _action.NotifyActionEvent(QQActionEventType.EVT_ERROR, qqEx);
                _action.OnHttpError(qqEx);
            }
        }

        public HttpActor(HttpActorType type, IQQContext context, IHttpAction action)
        {
            _type = type;
            _context = context;
            _action = action;
        }

        public HttpActor(HttpActorType type, IQQContext context, IHttpAction action, QQHttpResponse response)
        {
            _type = type;
            _context = context;
            _action = action;
            _response = response;
        }

        public HttpActor(HttpActorType type, IQQContext context, IHttpAction action, Exception throwable)
        {
            _type = type;
            _context = context;
            _action = action;
            _throwable = throwable;
        }

        public HttpActor(HttpActorType type, IQQContext context, IHttpAction action, long current, long total)
        {
            _type = type;
            _context = context;
            _action = action;
            _current = current;
            _total = total;
        }


        public class HttpAdaptor : IQQHttpListener
        {
            private readonly IQQContext _context;
            private readonly IHttpAction _action;

            public HttpAdaptor(IQQContext context, IHttpAction action)
            {
                _context = context;
                _action = action;
            }

            public void OnHttpFinish(QQHttpResponse response)
            {
                _context.PushActor(new HttpActor(HttpActorType.OnHttpFinish, _context, _action, response));
            }

            public void OnHttpError(Exception t)
            {
                _context.PushActor(new HttpActor(HttpActorType.OnHttpError, _context, _action, t));
            }

            public void OnHttpHeader(QQHttpResponse response)
            {
                _context.PushActor(new HttpActor(HttpActorType.OnHttpHeader, _context, _action, response));
            }

            public void OnHttpWrite(long current, long total)
            {
                _context.PushActor(new HttpActor(HttpActorType.OnHttpWrite, _context, _action, current, total));
            }

            public void OnHttpRead(long current, long total)
            {
                _context.PushActor(new HttpActor(HttpActorType.OnHttpRead, _context, _action, current, total));
            }
        }

        public override string ToString()
        {
            return "HttpActor [type=" + _type + ", action=" + _action + "]";
        }

        public QQActorType Type
        {
            get
            {
                if (_action is PollMsgAction)
                {
                    return QQActorType.PollMsgActor;
                }
                else if (_action is GetRobotReplyAction)
                {
                    return QQActorType.GetRobotReply;
                }
                else
                {
                    return QQActorType.SimpleActor;
                }
            }
        }

        public Task ExecuteAsync() => Task.Run(() => Execute());
    }
}
