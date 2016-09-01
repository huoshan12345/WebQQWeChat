using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>通过登录得到的sid，获取到wpkey</para>
    /// <para>@author 承∮诺</para>
    /// <para>@since 2014年1月25日</para>
    /// </summary>
    public class GetWPKeyAction : AbstractHttpAction
    {
        private readonly string _sid;
 
        public GetWPKeyAction(string sid, IQQContext context, QQActionEventHandler listener)
            : base(context, listener)
        {

            _sid = sid;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            var req = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_GET_WP_KEY);
            req.AddGetValue("r", "0.7975904128979892");
            req.AddGetValue("resp_charset", "UTF8");
            req.AddGetValue("ef", "js");
            req.AddGetValue("sid", _sid);
            req.AddGetValue("Referer", "http://mail.qq.com/cgi-bin/frame_html?sid=" + _sid);
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var resp = response.GetResponseString();
            resp = resp.Substring(1, resp.Length - 1);
            var json = JObject.Parse(resp);
            if (json["k"] != null)
            {
                NotifyActionEvent(QQActionEventType.EvtOK, json["k"].ToString());
            }
            else
            {
                NotifyActionEvent(QQActionEventType.EvtError, QQErrorCode.UNEXPECTED_RESPONSE);
            }
            // System.out.println("GetWPKeyAction: " + response.GetResponseString());
        }

    }
}
