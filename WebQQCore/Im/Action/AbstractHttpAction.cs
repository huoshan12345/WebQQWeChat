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
        public QQContext Context { get; }
        public QQActionFuture ActionFuture { get; set; }
        public Task<QQHttpResponse> ResponseFuture { get; set; }
        public QQActionEventHandler Listener { get; set; }

        protected AbstractHttpAction(QQContext context, QQActionEventHandler listener)
        {
            Context = context;
            Listener = listener;
            _retryTimes = 0;
        }

        public virtual void OnHttpFinish(QQHttpResponse response)
        {
            try
            {
                string type = response.GetContentType() ?? "";
                if ((type.StartsWith("application/x-javascript")
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
            catch (QQException e)
            {
                NotifyActionEvent(QQActionEventType.EVT_ERROR, e);
            }
            catch (JsonException e)
            {
                NotifyActionEvent(QQActionEventType.EVT_ERROR, new QQException(QQErrorCode.JSON_ERROR, e));
            }
            catch (Exception e)
            {
                NotifyActionEvent(QQActionEventType.EVT_ERROR, new QQException(QQErrorCode.UNKNOWN_ERROR, e));
            }
        }

        public virtual void OnHttpError(Exception t)
        {
            _retryTimes = 0;
            if (!DoRetryIt(GetErrorCode(t), t))
            {
                NotifyActionEvent(QQActionEventType.EVT_ERROR, new QQException(GetErrorCode(t), t));
            }
        }

        public virtual void OnHttpWrite(long current, long total)
        {
            QQActionEventArgs.ProgressArgs progress = new QQActionEventArgs.ProgressArgs();
            progress.total = total;
            progress.current = current;
            NotifyActionEvent(QQActionEventType.EVT_WRITE, progress);
        }

        public virtual void OnHttpRead(long current, long total)
        {
            QQActionEventArgs.ProgressArgs progress = new QQActionEventArgs.ProgressArgs();
            progress.total = total;
            progress.current = current;
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
                var ex = (target as Exception) ?? new QQException(QQErrorCode.UNKNOWN_ERROR);
#if DEBUG
                MyLogger.Default.Error($"{GetType().Name} [type={type.GetDescription()}, exception={ex.Message}, stacktrace={ex.StackTrace}]",ex);
#else
                MyLogger.Default.Debug($"{GetType().Name} [type={type.GetDescription()}, exception={ex.Message}", ex);
#endif
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
            try
            {
                return OnBuildRequest();
            }
            catch (JsonException e)
            {
                throw new QQException(QQErrorCode.JSON_ERROR, e);
            }
            catch (QQException e)
            {
                throw new QQException(e.ErrorCode, e);
            }
            catch (Exception e)
            {
                throw new QQException(QQErrorCode.UNKNOWN_ERROR, e);
            }
        }

        public virtual QQHttpRequest CreateHttpRequest(string method, string url)
        {
            IHttpService httpService = Context.GetSerivce<IHttpService>(QQServiceType.HTTP);
            return httpService.CreateHttpRequest(method, url);
        }

        public virtual void OnHttpStatusError(QQHttpResponse response)
        {
            if (!DoRetryIt(QQErrorCode.ERROR_HTTP_STATUS,
                new QQException(QQErrorCode.ERROR_HTTP_STATUS, response.ResponseMessage)))
            {
                throw new QQException(QQErrorCode.ERROR_HTTP_STATUS, response.ResponseMessage);
            }
        }

        /// <summary>
        /// response返回OK时的通知
        /// </summary>
        /// <param name="response"></param>
        public virtual void OnHttpStatusOK(QQHttpResponse response)
        {
            NotifyActionEvent(QQActionEventType.EVT_OK, null);
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

        private bool DoRetryIt(QQErrorCode code, Exception t)
        {
            if (ActionFuture.IsCanceled) return true;
            ++_retryTimes;
            if (_retryTimes < QQConstants.MAX_RETRY_TIMES)
            {
                NotifyActionEvent(QQActionEventType.EVT_RETRY, new QQException(code, t));
                Thread.Sleep(1000);
                Context.PushActor(new HttpActor(HttpActorType.BUILD_REQUEST, Context, this));
                return true;
            }
            return false;
        }

        private QQErrorCode GetErrorCode(Exception e)
        {
            if (e is TimeoutException)
            {
                return QQErrorCode.IO_TIMEOUT;
            }
            else if (e is IOException)
            {
                return QQErrorCode.IO_ERROR;
            }
            else
            {
                return QQErrorCode.UNKNOWN_ERROR;
            }
        }
    }
}
