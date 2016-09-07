using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpActionTools.Action;
using HttpActionTools.Core;

namespace HttpActionTools.Actor
{
    public enum HttpActorType
    {
        BuildRequest,
        CancelRequest,
        OnHttpError,
        OnHttpContent,
        OnHttpHeader,
        OnHttpWrite,
        OnHttpRead
    }

    public class HttpActor : IActor
    {
        private readonly IHttpAction _action;
        private readonly HttpActorType _type;

        public HttpActor(IHttpAction httpAction, HttpActorType httpActorType)
        {
            _action = httpAction;
            _type = httpActorType;
        }

        public void Execute()
        {
            if (_action.IsCanceled) return;
            try
            {
                switch (_type)
                {
                    case HttpActorType.BuildRequest:
                    {
                        var request = _action.BuildRequest();
                        _action.HttpService.ExecuteHttpRequestAsync(request, _action.Token);
                        break;
                    }

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
                _context.Logger.LogDebug($"[Action={GetType().Name}, HttpActorType={_type}] Error");

                var qqEx = ex as QQException ?? new QQException(ex);
                _action.OnHttpError(qqEx);
            }
            _context.Logger.LogDebug($"[Action={GetType().Name}, HttpActorType={_type}] End");
        }

        public Task ExecuteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
