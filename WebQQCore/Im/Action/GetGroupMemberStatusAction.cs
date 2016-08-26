using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>批量获取群成员在线状态</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class GetGroupMemberStatusAction : AbstractHttpAction
    {
        private QQGroup group;

        public GetGroupMemberStatusAction(IQQContext context, QQActionEventHandler listener, QQGroup group)
            : base(context, listener)
        {
            this.group = group;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            JObject json = JObject.Parse(response.GetResponseString());
            if (json["retcode"].ToString() == "0")
            {
                json = json["result"].ToObject<JObject>();

                // 消除所有成员状态，如果不在线的，webqq是不会返回的。
                foreach (QQGroupMember member in group.Members)
                {
                    member.Status = QQStatus.OFFLINE;
                    member.ClientType = QQClientType.Unknown;
                }

                //result/stats
                JArray stats = json["stats"].ToObject<JArray>();
                for (int i = 0; i < stats.Count; i++)
                {
                    // 下面重新设置最新状态
                    JObject stat = stats[i].ToObject<JObject>();
                    QQGroupMember member = group.GetMemberByUin(stat["uin"].ToObject<long>());
                    if (member != null)
                    {
                        member.ClientType = QQClientType.ValueOfRaw(stat["client_type"].ToObject<int>());
                        member.Status = QQStatus.ValueOfRaw(stat["stat"].ToObject<int>());
                    }
                }

                NotifyActionEvent(QQActionEventType.EVT_OK, group);
            }
            else
            {
                NotifyActionEvent(QQActionEventType.EVT_ERROR, QQErrorCode.UNEXPECTED_RESPONSE);
            }
        }

        public override QQHttpRequest OnBuildRequest()
        {
            QQHttpRequest req = CreateHttpRequest("GET", QQConstants.URL_GET_GROUP_INFO_EXT);
            req.AddGetValue("gcode", group.Code + "");
            req.AddGetValue("vfwebqq", Context.Session.Vfwebqq);
            req.AddGetValue("t", DateUtils.NowTimestamp() / 1000 + "");
            return req;
        }


    }
}
