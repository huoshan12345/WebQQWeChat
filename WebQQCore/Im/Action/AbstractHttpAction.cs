using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using iQQ.Net.WebQQCore.Im.Actor;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Im.Service;
using iQQ.Net.WebQQCore.Util.Log;
using Newtonsoft.Json;

namespace iQQ.Net.WebQQCore.Im.Action
{
    public abstract class AbstractHttpAction : IHttpAction
    {
        private int _retryTimes;
        public IQQContext Context { get; }
        public QQActionFuture ActionFuture { get; set; }
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
            try
            {
                var type = response.GetContentType();
                if (type != null && (type.StartsWith("application/x-javascript")
                        || type.StartsWith("application/json")
                        || type.Contains("text")
                        ) && response.GetContentLength() > 0)
                {
                    MyLogger.Default.Debug(response.GetResponseString());
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
            catch (QQException ex)
            {
                NotifyActionEvent(QQActionEventType.EVT_ERROR, ex);
                // throw;
            }
            catch (JsonException ex)
            {
                NotifyActionEvent(QQActionEventType.EVT_ERROR, new QQException(QQErrorCode.JSON_ERROR, ex));
                // throw new QQException(QQErrorCode.JSON_ERROR, e);
            }
            catch (Exception ex)
            {
                NotifyActionEvent(QQActionEventType.EVT_ERROR, new QQException(QQErrorCode.UNKNOWN_ERROR, ex));
                // throw new QQException(QQErrorCode.UNKNOWN_ERROR, e);
            }
        }

        public virtual void OnHttpError(Exception ex)
        {
            _retryTimes = 0;
            var qqEx = ex as QQException ?? new QQException(ex);
            if (!DoRetryIt(qqEx))
            {
                NotifyActionEvent(QQActionEventType.EVT_ERROR, qqEx);
            }
        }

        public virtual void OnHttpWrite(long current, long total)
        {
            QQActionEventArgs.ProgressArgs progress = new QQActionEventArgs.ProgressArgs
            {
                total = total,
                current = current
            };
            NotifyActionEvent(QQActionEventType.EVT_WRITE, progress);
        }

        public virtual void OnHttpRead(long current, long total)
        {
            QQActionEventArgs.ProgressArgs progress = new QQActionEventArgs.ProgressArgs
            {
                total = total,
                current = current
            };
            NotifyActionEvent(QQActionEventType.EVT_READ, progress);
        }

        public virtual void OnHttpHeader(QQHttpResponse response) { }

        public virtual void CancelRequest()
        {
            // ResponseFuture.Cancel(true);
            ResponseFuture.Dispose();
            NotifyActionEvent(QQActionEventType.EVT_CANCELED, null);
        }

        public virtual void NotifyActionEvent(QQActionEventType type, object target)
        {
            switch (type)
            {
                case QQActionEventType.EVT_ERROR:
                    var ex = target as QQException ?? (target is Exception ? new QQException((Exception)target) : new QQException(QQErrorCode.UNKNOWN_ERROR));
                    MyLogger.Default.Error($"{GetType().Name} [type={type.GetDescription()}, exception={ex}]", ex);
                    break;

                case QQActionEventType.EVT_CANCELED:
                    MyLogger.Default.Info($"{GetType().Name} [type={type.GetDescription()}, target={target}]");
                    break;

                default:
                    MyLogger.Default.Debug($"{GetType().Name} [type={type.GetDescription()}, target={target}]");
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
            IHttpService httpService = Context.GetSerivce<IHttpService>(QQServiceType.HTTP);
            return httpService.CreateHttpRequest(method, url);
        }

        public virtual void OnHttpStatusError(QQHttpResponse response)
        {
            var ex = new QQException(QQErrorCode.ERROR_HTTP_STATUS, response.ResponseMessage);
            if (!DoRetryIt(ex))
            {
                // throw ex;
                NotifyActionEvent(QQActionEventType.EVT_ERROR, ex);
            }
        }

        /// <summary>
        /// response返回OK时的通知
        /// </summary>
        /// <param name="response"></param>
        public virtual void OnHttpStatusOK(QQHttpResponse response)
        {
            NotifyActionEvent(QQActionEventType.EVT_OK, response);
        }

        /// <summary>
        /// 建立Request时的通知
        /// </summary>
        /// <returns></returns>
        public virtual QQHttpRequest OnBuildRequest()
        {
            return null;
        }

        public virtual bool IsCancelable()
        {
            return false;
        }

        private bool DoRetryIt(QQErrorCode code, Exception ex)
        {
            return DoRetryIt(new QQException(code, ex));
        }

        private bool DoRetryIt(QQException ex)
        {
            if (ActionFuture.IsCanceled) return true;
            ++_retryTimes;
            if (_retryTimes < QQConstants.MAX_RETRY_TIMES)
            {
                NotifyActionEvent(QQActionEventType.EVT_RETRY, ex);
                Thread.Sleep(1000);
                Context.PushActor(new HttpActor(HttpActorType.BUILD_REQUEST, Context, this));
                return true;
            }
            return false;
        }


    }
}
