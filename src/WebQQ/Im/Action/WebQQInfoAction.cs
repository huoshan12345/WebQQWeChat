using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FclEx.Extensions;
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
    public abstract class WebQQInfoAction : WebQQAction
    {
        private static readonly ConcurrentDictionary<Type, string> _urlApiDic = new ConcurrentDictionary<Type, string>();

        protected WebQQInfoAction(IQQContext context, ActionEventListener listener = null)
            : base(context, listener) { }

        protected override HttpRequestItem BuildRequest()
        {
            var actionType = this.GetType();
            var url = _urlApiDic.GetOrAdd(actionType, key =>
            {
                var actionName = actionType.Name.Replace("Action", "");
                return typeof(ApiUrls).GetField(actionName).GetValue(null).ToString();
            });
            var req = HttpRequestItem.CreateGetRequest(url);
            ModifyRequest(req);
            return req;
        }

        protected virtual void ModifyRequest(HttpRequestItem req) { }

        protected override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            var json = response.ResponseString.ToJToken();
            if (json["retcode"].ToString() == "0")
            {
                HandleResult(json);
                return NotifyOkEventAsync();
            }
            else
            {
                throw new QQException(QQErrorCode.ResponseError, response.ResponseString);
            }
        }

        protected virtual void HandleResult(JToken json) { }
    }
}
