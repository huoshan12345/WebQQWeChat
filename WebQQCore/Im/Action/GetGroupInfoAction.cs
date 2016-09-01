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
        private readonly QQGroup _group;

        public GetGroupInfoAction(IQQContext context, QQActionEventHandler listener, QQGroup group)
            : base(context, listener)
        {
            this._group = group;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            var req = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_GET_GROUP_INFO_EXT);
            req.AddGetValue("gcode", _group.Code);
            req.AddGetValue("vfwebqq", Context.Account.Vfwebqq);
            req.AddGetValue("t", DateTime.Now.CurrentTimeMillis());
            req.AddHeader("Referer", QQConstants.REFERER_S);
            // req.AddHeader("Origin", QQConstants.ORIGIN_S);

            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var str = response.GetResponseString();
            var json = JObject.Parse(str);

            if (json["retcode"].ToString() == "0")
            {
                json = json["result"].ToObject<JObject>();
                var ginfo = json["ginfo"].ToObject<JObject>();
                _group.Memo = ginfo["memo"].ToString();
                _group.Level = ginfo["level"].ToObject<int>();
                _group.CreateTime = new DateTime(ginfo["createtime"].ToObject<int>());

                var members = ginfo["members"].ToObject<JArray>();
                for (var i = 0; i < members.Count; i++)
                {
                    var memjson = members[i].ToObject<JObject>();
                    var member = _group.GetMemberByUin(memjson["muin"].ToObject<long>());
                    if (member == null)
                    {
                        member = new QQGroupMember();
                        _group.Members.Add(member);
                    }
                    member.Uin = memjson["muin"].ToObject<long>();
                    member.Group = _group;
                    //memjson["mflag"]; //TODO ...
                }

                //result/minfo
                var minfos = json["minfo"].ToObject<JArray>();
                foreach (var token in minfos)
                {
                    var minfo = token.ToObject<JObject>();
                    var member = _group.GetMemberByUin(minfo["uin"].ToObject<int>());
                    member.Nickname = minfo["nick"].ToString();
                    member.Province = minfo["province"].ToString();
                    member.Country = minfo["country"].ToString();
                    member.City = minfo["city"].ToString();
                    member.Gender = minfo["gender"].ToString();
                }

                //result/stats
                var stats = json["stats"].ToObject<JArray>();
                for (var i = 0; i < stats.Count; i++)
                {
                    // 下面重新设置最新状态
                    var stat = stats[i].ToObject<JObject>();
                    var member = _group.GetMemberByUin(stat["uin"].ToObject<long>());
                    member.ClientType = QQClientType.ValueOfRaw(stat["client_type"].ToObject<int>());
                    member.Status = QQStatus.ValueOfRaw(stat["stat"].ToObject<int>());
                }

                //results/cards
                if (json["cards"] != null)
                {
                    var cards = json["cards"].ToObject<JArray>();
                    for (var i = 0; i < cards.Count; i++)
                    {
                        var card = cards[i].ToObject<JObject>();
                        var member = _group.GetMemberByUin(card["muin"].ToObject<long>());
                        if (card["card"] != null && member != null)
                        {
                            member.Card = card["card"].ToString();
                        }
                    }
                }

                //results/vipinfo
                var vipinfos = json["vipinfo"].ToObject<JArray>();
                for (var i = 0; i < vipinfos.Count; i++)
                {
                    var vipinfo = vipinfos[i].ToObject<JObject>();
                    var member = _group.GetMemberByUin(vipinfo["u"].ToObject<long>());
                    member.VipLevel = vipinfo["vip_level"].ToObject<int>();
                    member.IsVip = (vipinfo["is_vip"].ToString() != "0");
                }

                NotifyActionEvent(QQActionEventType.EvtOK, _group);
            }
            else
            {
                // NotifyActionEvent(QQActionEventType.EVT_ERROR, QQErrorCode.UNEXPECTED_RESPONSE);
                throw new QQException(QQErrorCode.UNEXPECTED_RESPONSE, str);
            }
        }

    }
}
