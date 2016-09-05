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
    /// <para>批量获取群成员在线状态</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class GetGroupMemberStatusAction : AbstractHttpAction
    {
        private readonly QQGroup _group;

        public GetGroupMemberStatusAction(IQQContext context, QQActionListener listener, QQGroup group)
            : base(context, listener)
        {
            _group = group;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var json = JObject.Parse(response.GetResponseString());
            if (json["retcode"].ToString() == "0")
            {
                json = json["result"].ToObject<JObject>();

                // 消除所有成员状态，如果不在线的，webqq是不会返回的。
                foreach (var member in _group.Members)
                {
                    member.Status = QQStatus.OFFLINE;
                    member.ClientType = QQClientType.Unknown;
                }

                //result/stats
                var stats = json["stats"].ToObject<JArray>();
                for (var i = 0; i < stats.Count; i++)
                {
                    // 下面重新设置最新状态
                    var stat = stats[i].ToObject<JObject>();
                    var member = _group.GetMemberByUin(stat["uin"].ToObject<long>());
                    if (member != null)
                    {
                        member.ClientType = QQClientTypeInfo.ValueOfRaw(stat["client_type"].ToObject<int>());
                        member.Status = QQStatus.ValueOfRaw(stat["stat"].ToObject<int>());
                    }
                }

                NotifyActionEvent(QQActionEventType.EvtOK, _group);
            }
            else
            {
                NotifyActionEvent(QQActionEventType.EvtError, QQErrorCode.UnexpectedResponse);
            }
        }

        public override QQHttpRequest OnBuildRequest()
        {
            var req = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_GET_GROUP_INFO_EXT);
            req.AddGetValue("gcode", _group.Code);
            req.AddGetValue("vfwebqq", Context.Session.Vfwebqq);
            req.AddGetValue("t", DateTime.Now.CurrentTimeSeconds());
            return req;
        }


    }
}
