using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>获取QQ账号</para>
    /// <para>@author ChenZhiHui</para>
    /// <para>@since 2013-2-23</para>
    /// </summary>
    public class GetGroupAccoutAction : AbstractHttpAction
    {

        private QQGroup group;

        public GetGroupAccoutAction(IQQContext context, QQActionEventHandler listener, QQGroup group)
            : base(context, listener)
        {
            this.group = group;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            QQSession session = Context.Session;
            // tuin=4245757755&verifysession=&type=1&code=&vfwebqq=**&t=1361631644492
            QQHttpRequest req = CreateHttpRequest("GET",
                    QQConstants.URL_GET_USER_ACCOUNT);
            req.AddGetValue("tuin", group.Code + "");
            req.AddGetValue("vfwebqq", session.Vfwebqq);
            req.AddGetValue("t", DateUtils.NowTimestamp() / 1000 + "");
            req.AddGetValue("verifysession", ""); // 验证码？？
            req.AddGetValue("type", 4 + "");
            req.AddGetValue("code", "");

            req.AddHeader("Referer", QQConstants.REFFER);
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            JObject json = JObject.Parse(response.GetResponseString());
            if (json["retcode"].ToString() == "0")
            {
                JObject obj = json["result"].ToObject<JObject>();
                group.Gid = obj["account"].ToObject<long>();
            }

            NotifyActionEvent(QQActionEventType.EVT_OK, group);
        }
    }
}
