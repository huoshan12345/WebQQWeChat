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
    /// <para>获取讨论组信息，讨论组成员</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class GetDiscuzInfoAction : AbstractHttpAction
    {
        private QQDiscuz discuz;
 
        public GetDiscuzInfoAction(IQQContext context, QQActionListener listener, QQDiscuz discuz)
            : base(context, listener)
        {

            this.discuz = discuz;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            var session = Context.Session;
            var req = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_GET_DISCUZ_INFO);
            req.AddGetValue("clientid", session.ClientId);
            req.AddGetValue("psessionid", session.SessionId);
            req.AddGetValue("vfwebqq", session.Vfwebqq);
            req.AddGetValue("t", DateTime.Now.CurrentTimeSeconds());
            req.AddGetValue("did", discuz.Did);
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var json = JObject.Parse(response.GetResponseString());
            var store = Context.Store;
            if (json["retcode"].ToString() == "0")
            {
                var result = json["result"].ToObject<JObject>();

                //result/info
                var info = result["info"].ToObject<JObject>();
                discuz.Name = info["discu_name"].ToString();
                discuz.Owner = info["discu_owner"].ToObject<long>();

                //result/mem_list
                var memlist = result["mem_info"].ToObject<JArray>();
                foreach (var t in memlist)
                {
                    var memjson = t.ToObject<JObject>();
                    var member = discuz.GetMemberByUin(memjson["uin"].ToObject<long>());
                    if (member == null)
                    {
                        member = new QQDiscuzMember();
                        discuz.AddMemeber(member);
                    }
                    member.Uin = memjson["uin"].ToObject<long>();
                    member.QQ = memjson["uin"].ToObject<long>();	//这里有用户真实的QQ号
                    member.Nickname = memjson["nick"].ToString();
                    member.Discuz = discuz;
                }

                // 消除所有成员状态，如果不在线的，webqq是不会返回的。
                discuz.ClearStatus();
                //result/mem_status
                var statlist = result["mem_status"].ToObject<JArray>();
                foreach (var t in statlist)
                {
                    // 下面重新设置最新状态
                    var statjson = t.ToObject<JObject>();
                    QQUser member = discuz.GetMemberByUin(statjson["uin"].ToObject<long>());
                    if (statjson["client_type"] != null && member != null)
                    {
                        member.ClientType = QQClientType.ValueOfRaw(statjson["client_type"].ToObject<int>());
                        member.Status = QQStatus.ValueOfRaw(statjson["status"].ToString());
                    }
                }

                //result/mem_info
                var infolist = result["mem_info"].ToObject<JArray>();
                foreach (var t in infolist)
                {
                    var infojson = t.ToObject<JObject>();
                    QQUser member = discuz.GetMemberByUin(infojson["uin"].ToObject<long>());
                    member.Nickname = infojson["nick"].ToString();
                }

                NotifyActionEvent(QQActionEventType.EvtOK, store.GetDiscuzList());
            }
            else
            {
                NotifyActionEvent(QQActionEventType.EvtError, new QQException(QQErrorCode.UnexpectedResponse));
            }
        }
    }
}
