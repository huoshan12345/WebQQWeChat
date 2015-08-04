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
 
        public GetDiscuzInfoAction(QQContext context, QQActionEventHandler listener, QQDiscuz discuz)
            : base(context, listener)
        {

            this.discuz = discuz;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            QQSession session = Context.Session;
            QQHttpRequest req = CreateHttpRequest("GET", QQConstants.URL_GET_DISCUZ_INFO);
            req.AddGetValue("clientid", session.ClientId + "");
            req.AddGetValue("psessionid", session.SessionId);
            req.AddGetValue("vfwebqq", session.Vfwebqq);
            req.AddGetValue("t", DateUtils.NowTimestamp() + "");
            req.AddGetValue("did", discuz.Did + "");
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            JObject json = JObject.Parse(response.GetResponseString());
            QQStore store = Context.Store;
            if (json["retcode"].ToString() == "0")
            {
                JObject result = json["result"].ToObject<JObject>();

                //result/info
                JObject info = result["info"].ToObject<JObject>();
                discuz.Name = info["discu_name"].ToString();
                discuz.Owner = info["discu_owner"].ToObject<long>();

                //result/mem_list
                JArray memlist = result["mem_info"].ToObject<JArray>();
                foreach (JToken t in memlist)
                {
                    JObject memjson = t.ToObject<JObject>();
                    QQDiscuzMember member = discuz.GetMemberByUin(memjson["uin"].ToObject<long>());
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
                JArray statlist = result["mem_status"].ToObject<JArray>();
                foreach (JToken t in statlist)
                {
                    // 下面重新设置最新状态
                    JObject statjson = t.ToObject<JObject>();
                    QQUser member = discuz.GetMemberByUin(statjson["uin"].ToObject<long>());
                    if (statjson["client_type"] != null && member != null)
                    {
                        member.ClientType = QQClientType.ValueOfRaw(statjson["client_type"].ToObject<int>());
                        member.Status = QQStatus.ValueOfRaw(statjson["status"].ToString());
                    }
                }

                //result/mem_info
                JArray infolist = result["mem_info"].ToObject<JArray>();
                foreach (JToken t in infolist)
                {
                    JObject infojson = t.ToObject<JObject>();
                    QQUser member = discuz.GetMemberByUin(infojson["uin"].ToObject<long>());
                    member.Nickname = infojson["nick"].ToString();
                }

                NotifyActionEvent(QQActionEventType.EVT_OK, store.GetDiscuzList());
            }
            else
            {
                NotifyActionEvent(QQActionEventType.EVT_ERROR, new QQException(QQErrorCode.UNEXPECTED_RESPONSE));
            }
        }
    }
}
