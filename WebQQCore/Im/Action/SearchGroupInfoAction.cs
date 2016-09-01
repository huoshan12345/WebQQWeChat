using System;
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

        private readonly QQGroupSearchList _buddy;
  
        public SearchGroupInfoAction(IQQContext context, QQActionEventHandler listener, QQGroupSearchList buddy)
            : base(context, listener)
        {

            _buddy = buddy;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            var session = Context.Session;
            var req = CreateHttpRequest(HttpConstants.Get,
                    QQConstants.URL_SEARCH_GROUP_INFO);

            //我不知道以下4个参数干啥？但是一致！	
            req.AddGetValue("c1", "0");
            req.AddGetValue("c2", "0");
            req.AddGetValue("c3", "0");
            req.AddGetValue("st", "0");

            req.AddGetValue("pg", _buddy.CurrentPage);
            req.AddGetValue("perpage", _buddy.PageSize);
            req.AddGetValue("all", _buddy.KeyStr);

            req.AddGetValue("vfwebqq", session.Vfwebqq);
            req.AddGetValue("t", DateTime.Now.CurrentTimeSeconds());
            req.AddGetValue("type", 1);
            req.AddGetValue("vfcode", "");

            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var json = JObject.Parse(response.GetResponseString());

            if (json["retcode"].ToString() == "0")
            {
                var result = json["result"].ToObject<JArray>();
                for (var index = 0; index < result.Count; index++)
                {   //结果获取;
                    var info = new QQGroupSearchInfo();
                    var ret = result[index].ToObject<JObject>();
                    info.GroupId = ret["GE"].ToObject<long>();  //真实的QQ号
                    info.OwerId = ret["QQ"].ToObject<long>();
                    info.GroupName = ret["TI"].ToString();
                    info.CreateTimeStamp = ret["RQ"].ToObject<long>();  //QQ群创建时间,时间戳形式;
                    info.GroupAliseId = ret["GEX"].ToObject<long>();
                }
            }
            if (json["retcode"].ToString() == "100110") //需要验证码
            {
                this._buddy.NeedVfcode = true;

            }
            NotifyActionEvent(QQActionEventType.EvtOK, _buddy);
        }

    }

}
