using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;
using FclEx.Extensions;
using HttpAction.Core;
using HttpAction.Event;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Core;

namespace WebQQ.Im.Actions
{
    /// <summary>
    /// 获取信息类的qqaction的基类
    /// </summary>
    public abstract class WebQQInfoAction : WebQQAction
    {
        protected override EnumRequestType RequestType { get; } = EnumRequestType.Get;

        protected WebQQInfoAction(IQQContext context, ActionEventListener listener = null)
            : base(context, listener) { }

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
