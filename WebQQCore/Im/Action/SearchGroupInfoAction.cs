using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>查找群,并获取相应信息</para>
    /// <para>@author 元谷</para>
    /// <para>@since 2013-8-13</para>
    /// </summary>
    public class SearchGroupInfoAction : AbstractHttpAction
    {

        private QQGroupSearchList buddy;
  
        public SearchGroupInfoAction(IQQContext context, QQActionEventHandler listener, QQGroupSearchList buddy)
            : base(context, listener)
        {

            this.buddy = buddy;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            QQSession session = Context.Session;
            QQHttpRequest req = CreateHttpRequest("GET",
                    QQConstants.URL_SEARCH_GROUP_INFO);

            //我不知道以下4个参数干啥？但是一致！	
            req.AddGetValue("c1", "0");
            req.AddGetValue("c2", "0");
            req.AddGetValue("c3", "0");
            req.AddGetValue("st", "0");

            req.AddGetValue("pg", buddy.CurrentPage + "");
            req.AddGetValue("perpage", buddy.PageSize + "");
            req.AddGetValue("all", buddy.KeyStr);

            req.AddGetValue("vfwebqq", session.Vfwebqq);
            req.AddGetValue("t", DateUtils.NowTimestamp() / 1000 + "");
            req.AddGetValue("type", 1 + "");
            req.AddGetValue("vfcode", "");

            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            JObject json = JObject.Parse(response.GetResponseString());

            if (json["retcode"].ToString() == "0")
            {
                JArray result = json["result"].ToObject<JArray>();
                for (int index = 0; index < result.Count; index++)
                {   //结果获取;
                    QQGroupSearchInfo info = new QQGroupSearchInfo();
                    JObject ret = result[index].ToObject<JObject>();
                    info.GroupId = ret["GE"].ToObject<long>();  //真实的QQ号
                    info.OwerId = ret["QQ"].ToObject<long>();
                    info.GroupName = ret["TI"].ToString();
                    info.CreateTimeStamp = ret["RQ"].ToObject<long>();  //QQ群创建时间,时间戳形式;
                    info.GroupAliseId = ret["GEX"].ToObject<long>();
                }
            }
            if (json["retcode"].ToString() == "100110") //需要验证码
            {
                this.buddy.NeedVfcode = true;

            }
            NotifyActionEvent(QQActionEventType.EVT_OK, buddy);
        }

    }

}
