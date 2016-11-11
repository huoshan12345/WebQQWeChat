using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HttpAction.Core;
using HttpAction.Event;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Core;
using WebQQ.Util;

namespace WebQQ.Im.Action
{
    /// <summary>
    /// 获取信息类的qqaction的基类
    /// </summary>
    public abstract class WebQQInfoAction : QQAction
    {
        private static readonly ConcurrentDictionary<Type, string> UrlApiDic = new ConcurrentDictionary<Type, string>();

        protected WebQQInfoAction(IQQContext context, ActionEventListener listener = null)
            : base(context, listener) { }

        public override HttpRequestItem BuildRequest()
        {
            var actionType = this.GetType();
            var url = UrlApiDic.GetOrAdd(actionType, key =>
            {
                var actionName = actionType.Name.Replace("Action", "");
                return typeof(ApiUrls).GetField(actionName).GetValue(null).ToString();
            });
            var req = HttpRequestItem.CreateGetRequest(url);
            ModifyRequest(req);
            return req;
        }

        protected virtual void ModifyRequest(HttpRequestItem req) { }

        public override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            var json = response.ResponseString.ToJToken();
            if (json["retcode"].ToString() == "0")
            {
                HandleResult(json);
                return NotifyActionEventAsync(ActionEventType.EvtOK);
            }
            else
            {
                throw new QQException(QQErrorCode.ResponseError, response.ResponseString);
            }
        }

        protected virtual void HandleResult(JToken json) { }
    }
}
