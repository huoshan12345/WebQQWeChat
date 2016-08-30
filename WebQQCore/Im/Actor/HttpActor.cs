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
        BUILD_REQUEST,
        CANCEL_REQUEST,
        ON_HTTP_ERROR,
        ON_HTTP_FINISH,
        ON_HTTP_HEADER,
        ON_HTTP_WRITE,
        ON_HTTP_READ
    }

    public class HttpActor : QQActor
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
            try
            {
                switch (_type)
                {
                    case HttpActorType.BUILD_REQUEST:
                        var service = _context.GetSerivce<IHttpService>(QQServiceType.HTTP);
                        var request = _action.BuildRequest();
                       service.ExecuteHttpRequest(request, new HttpAdaptor(_context, _action));
                        break;

                    case HttpActorType.CANCEL_REQUEST:
                        _action.CancelRequest();
                        break;

                    case HttpActorType.ON_HTTP_ERROR:
                        _action.OnHttpError(_throwable);
                        break;

                    case HttpActorType.ON_HTTP_FINISH:
                        _action.OnHttpFinish(_response);
                        break;

                    case HttpActorType.ON_HTTP_HEADER:
                        _action.OnHttpHeader(_response);
                        break;

                    case HttpActorType.ON_HTTP_READ:
                        _action.OnHttpRead(_current, _total);
                        break;

                    case HttpActorType.ON_HTTP_WRITE:
                        _action.OnHttpWrite(_current, _total);
                        break;
                }
            }
            catch (Exception e)
            {
                _action.NotifyActionEvent(QQActionEventType.EVT_ERROR, e);
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
            private readonly IQQContext context;
            private readonly IHttpAction action;

            public HttpAdaptor(IQQContext context, IHttpAction action)
            {
                this.context = context;
                this.action = action;
            }


            public void OnHttpFinish(QQHttpResponse response)
            {
                context.PushActor(new HttpActor(HttpActorType.ON_HTTP_FINISH, context, action, response));
            }


            public void OnHttpError(Exception t)
            {
                context.PushActor(new HttpActor(HttpActorType.ON_HTTP_ERROR, context, action, t));
            }


            public void OnHttpHeader(QQHttpResponse response)
            {
                context.PushActor(new HttpActor(HttpActorType.ON_HTTP_HEADER, context, action, response));

            }


            public void OnHttpWrite(long current, long total)
            {
                context.PushActor(new HttpActor(HttpActorType.ON_HTTP_WRITE, context, action, current, total));
            }


            public void OnHttpRead(long current, long total)
            {
                context.PushActor(new HttpActor(HttpActorType.ON_HTTP_READ, context, action, current, total));
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

        public Task ExecuteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
