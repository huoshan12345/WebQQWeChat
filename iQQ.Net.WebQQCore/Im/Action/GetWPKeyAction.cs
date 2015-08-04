using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
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
        private string sid = "";
 
        public GetWPKeyAction(string sid, QQContext context, QQActionEventHandler listener)
            : base(context, listener)
        {

            this.sid = sid;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            QQHttpRequest req = CreateHttpRequest("GET", QQConstants.URL_GET_WP_KEY);
            req.AddGetValue("r", "0.7975904128979892");
            req.AddGetValue("resp_charset", "UTF8");
            req.AddGetValue("ef", "js");
            req.AddGetValue("sid", sid);
            req.AddGetValue("Referer", "http://mail.qq.com/cgi-bin/frame_html?sid=" + sid);
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            string resp = response.GetResponseString();
            resp = resp.Substring(1, resp.Length - 1);
            JObject json = JObject.Parse(resp);
            if (json["k"] != null)
            {
                NotifyActionEvent(QQActionEventType.EVT_OK, json["k"].ToString());
            }
            else
            {
                NotifyActionEvent(QQActionEventType.EVT_ERROR, QQErrorCode.UNEXPECTED_RESPONSE);
            }
            // System.out.println("GetWPKeyAction: " + response.GetResponseString());
        }

    }
}
