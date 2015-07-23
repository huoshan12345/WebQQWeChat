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
    /// <para>获取群信息， 包括群信息和群成员</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class GetGroupInfoAction : AbstractHttpAction
    {
        private QQGroup group;

        public GetGroupInfoAction(QQContext context, QQActionEventHandler listener, QQGroup group)
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
                JObject ginfo = json["ginfo"].ToObject<JObject>();
                group.Memo = ginfo["memo"].ToString();
                group.Level = ginfo["level"].ToObject<int>();
                group.CreateTime = new DateTime(ginfo["createtime"].ToObject<int>());

                JArray members = ginfo["members"].ToObject<JArray>();
                for (int i = 0; i < members.Count; i++)
                {
                    JObject memjson = members[i].ToObject<JObject>();
                    QQGroupMember member = group.GetMemberByUin(memjson["muin"].ToObject<long>());
                    if (member == null)
                    {
                        member = new QQGroupMember();
                        group.Members.Add(member);
                    }
                    member.Uin = memjson["muin"].ToObject<long>();
                    member.Group = group;
                    //memjson["mflag"]; //TODO ...
                }

                //result/minfo
                JArray minfos = json["minfo"].ToObject<JArray>();
                for (int i = 0; i < minfos.Count; i++)
                {
                    JObject minfo = minfos[i].ToObject<JObject>();
                    QQGroupMember member = group.GetMemberByUin(minfo["uin"].ToObject<int>());
                    member.Nickname = minfo["nick"].ToString();
                    member.Province = minfo["province"].ToString();
                    member.Country = minfo["country"].ToString();
                    member.City = minfo["city"].ToString();
                    member.Gender = minfo["gender"].ToString();
                }

                //result/stats
                JArray stats = json["stats"].ToObject<JArray>();
                for (int i = 0; i < stats.Count; i++)
                {
                    // 下面重新设置最新状态
                    JObject stat = stats[i].ToObject<JObject>();
                    QQGroupMember member = group.GetMemberByUin(stat["uin"].ToObject<long>());
                    member.ClientType = QQClientType.ValueOfRaw(stat["client_type"].ToObject<int>());
                    member.Status = QQStatus.ValueOfRaw(stat["stat"].ToObject<int>());
                }

                //results/cards
                if (json["cards"] != null)
                {
                    JArray cards = json["cards"].ToObject<JArray>();
                    for (int i = 0; i < cards.Count; i++)
                    {
                        JObject card = cards[i].ToObject<JObject>();
                        QQGroupMember member = group.GetMemberByUin(card["muin"].ToObject<long>());
                        if (card["card"] != null && member != null)
                        {
                            member.Card = card["card"].ToString();
                        }
                    }
                }

                //results/vipinfo
                JArray vipinfos = json["vipinfo"].ToObject<JArray>();
                for (int i = 0; i < vipinfos.Count; i++)
                {
                    JObject vipinfo = vipinfos[i].ToObject<JObject>();
                    QQGroupMember member = group.GetMemberByUin(vipinfo["u"].ToObject<long>());
                    member.VipLevel = vipinfo["vip_level"].ToObject<int>();
                    member.IsVip = (vipinfo["is_vip"].ToString() != "0");
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
