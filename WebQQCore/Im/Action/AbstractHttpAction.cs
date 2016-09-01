using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using iQQ.Net.WebQQCore.Im.Actor;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Im.Service;
using iQQ.Net.WebQQCore.Util;
using iQQ.Net.WebQQCore.Util.Extensions;
using Newtonsoft.Json;

namespace iQQ.Net.WebQQCore.Im.Action
{
    public abstract class AbstractHttpAction : IHttpAction
    {
        private int _retryTimes;
        public IQQContext Context { get; }
        public IQQActionFuture ActionFuture { get; set; }
        public Task<QQHttpResponse> ResponseFuture { get; set; }
        public QQActionEventHandler Listener { get; set; }

        protected AbstractHttpAction(IQQContext context, QQActionEventHandler listener)
        {
            Context = context;
            Listener = listener;
            _retryTimes = 0;
        }

        public virtual void OnHttpFinish(QQHttpResponse response)
        {
            var type = response.GetContentType();
            if (type != null && (type.StartsWith("application/x-javascript")
                    || type.StartsWith("application/json")
                    || type.Contains("text")
                    ) && response.GetContentLength() > 0)
            {
                // DefaultLogger.Debug(response.GetResponseString());
            }

            if (response.ResponseCode == QQHttpResponse.S_OK)
            {
                OnHttpStatusOK(response);
            }
            else
            {
                OnHttpStatusError(response);
            }
        }

        public virtual void OnHttpError(Exception ex)
        {
            var qqEx = ex as QQException ?? new QQException(ex);
            if (!DoRetryIt(qqEx, QQConstants.MAX_RETRY_TIMES))
            {
                NotifyActionEvent(QQActionEventType.EvtError, qqEx);
            }
        }

        public virtual void OnHttpWrite(long current, long total)
        {
            var progress = new ProgressArgs
            {
                Total = total,
                Current = current
            };
            NotifyActionEvent(QQActionEventType.EvtWrite, progress);
        }

        public virtual void OnHttpRead(long current, long total)
        {
            var progress = new ProgressArgs
            {
                Total = total,
                Current = current
            };
            NotifyActionEvent(QQActionEventType.EvtRead, progress);
        }

        public virtual void OnHttpHeader(QQHttpResponse response) { }

        public virtual void CancelRequest()
        {
            // ResponseFuture.Cancel(true);
            ResponseFuture.Dispose();
            NotifyActionEvent(QQActionEventType.EvtCanceled, null);
        }

        public virtual void NotifyActionEvent(QQActionEventType type, object target)
        {
            switch (type)
            {
                case QQActionEventType.EvtError:
                {
                    var ex = (QQException)target;
                    DefaultLogger.Error($"{GetType().Name} [type={type.GetFullDescription()}, errorcode={ex.ErrorCode}, exception={ex.Message}]{Environment.NewLine}{ex.StackTrace}", ex);
                    break;
                }

                case QQActionEventType.EvtRetry:
                {
                    var ex = (QQException)target;
                    DefaultLogger.Warn($"{GetType().Name} [type={type.GetFullDescription()}, 重试次数={_retryTimes}, target={ex.Message}]");
                    break;
                }

                case QQActionEventType.EvtCanceled:
                DefaultLogger.Info($"{GetType().Name} [type={type.GetFullDescription()}, target={target}]");
                break;

                case QQActionEventType.EvtOK:
                case QQActionEventType.EvtWrite:
                case QQActionEventType.EvtRead:
                default:
                break;
            }
            Listener?.Invoke(ActionFuture, new QQActionEvent(type, target));
        }

        public virtual QQHttpRequest BuildRequest()
        {
            return OnBuildRequest();
        }

        public virtual QQHttpRequest CreateHttpRequest(string method, string url)
        {
            var httpService = Context.GetSerivce<IHttpService>(QQServiceType.HTTP);
            return httpService.CreateHttpRequest(method, url);
        }

        public virtual void OnHttpStatusError(QQHttpResponse response)
        {
            var ex = new QQException(QQErrorCode.ERROR_HTTP_STATUS, response.ResponseMessage);
            if (!DoRetryIt(ex, QQConstants.MAX_RETRY_TIMES))
            {
                // NotifyActionEvent(QQActionEventType.EVT_ERROR, ex);
                throw ex;
            }
        }

        /// <summary>
        /// response返回OK时的通知
        /// </summary>
        /// <param name="response"></param>
        public virtual void OnHttpStatusOK(QQHttpResponse response)
        {
            _retryTimes = 0; // 成功了重置重试次数
            NotifyActionEvent(QQActionEventType.EvtOK, response);
        }

        /// <summary>
        /// 建立Request时的通知
        /// </summary>
        /// <returns></returns>
        public virtual QQHttpRequest OnBuildRequest() => null;

        public virtual bool IsCancelable() => false;

        private bool DoRetryIt(QQException ex, int maxTimes)
        {
            if (ActionFuture.IsCanceled) return true;
            if (++_retryTimes > maxTimes) return false;
            NotifyActionEvent(QQActionEventType.EvtRetry, ex);
            Thread.Sleep(1000);
            Context.PushActor(new HttpActor(HttpActorType.BUILD_REQUEST, Context, this));
            return true;
        }


    }
}
