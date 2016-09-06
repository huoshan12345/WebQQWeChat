using System;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>获取好友信息的请求</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class GetFriendInfoAction : AbstractHttpAction
    {
        private readonly QQUser _buddy;

        public GetFriendInfoAction(IQQContext context, QQActionListener listener, QQUser buddy)
            : base(context, listener)
        {
            _buddy = buddy;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            /*
                tuin	236557647
                verifysession	
                code	
                vfwebqq	efa425e6afa21b3ca3ab8db97b65afa0535feb4af47a38cadcf1a4b1650169b4b4eee9955f843990
                t	1346856270187                         
             */
            var session = Context.Session;
            var req = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_GET_FRIEND_INFO);
            req.AddGetValue("tuin", _buddy.Uin);
            req.AddGetValue("verifysession", "");	//难道有验证码？？？
            req.AddGetValue("code", "");
            req.AddGetValue("vfwebqq", session.Vfwebqq);
            req.AddGetValue("t", DateTime.Now.CurrentTimeMillis());

            req.AddHeader("Referer", QQConstants.REFFER);
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            /*
                 {
                    "retcode": 0,
                    "result": {
                        "face": 603,
                        "birthday": {
                            "month": 8,
                            "year": 1895,
                            "day": 15
                        },
                        "occupation": "其他",
                        "phone": "110",
                        "allow": 1,
                        "college": "aaa",
                        "uin": 1382902354,
                        "constel": 7,
                        "blood": 5,
                        "homepage": "木有",
                        "stat": 20,
                        "vip_info": 6,
                        "country": "乍得",
                        "city": "",
                        "personal": "这是简介",
                        "nick": "ABCD",
                        "shengxiao": 11,
                        "email": "352323245@qq.com",
                        "province": "",
                        "gender": "female",
                        "mobile": "139********"
                    }
                }             
            */
            var json = JObject.Parse(response.GetResponseString());
            if (json["retcode"].ToString() == "0")
            {
                var obj = json["result"].ToObject<JObject>();
                _buddy.Birthday = DateUtils.Parse(obj["birthday"].ToObject<JObject>());
                _buddy.Occupation = obj["occupation"].ToString();
                _buddy.Phone = obj["phone"].ToString();
                _buddy.Allow = (QQAllow)obj["allow"].ToObject<int>();

                _buddy.College = obj["college"].ToString();
                if (obj["reg_time"] != null)
                {
                    _buddy.RegTime = obj["reg_time"].ToObject<int>();
                }
                _buddy.Uin = obj["uin"].ToObject<long>();
                _buddy.Constel = obj["constel"].ToObject<int>();
                _buddy.Blood = obj["blood"].ToObject<int>();
                _buddy.Homepage = obj["homepage"].ToString();
                _buddy.Stat = obj["stat"].ToObject<int>();
                _buddy.VipLevel = obj["vip_info"].ToObject<int>(); // VIP等级 0为非VIP
                _buddy.Country = obj["country"].ToString();
                _buddy.City = obj["city"].ToString();
                _buddy.Personal = obj["personal"].ToString();
                _buddy.Nickname = obj["nick"].ToString();
                _buddy.ChineseZodiac = obj["shengxiao"].ToObject<int>();
                _buddy.Email = obj["email"].ToString();
                _buddy.Province = obj["province"].ToString();
                _buddy.Gender = obj["gender"].ToString();
                _buddy.Mobile = obj["mobile"].ToString();
                if (obj["client_type"] != null)
                {
                    _buddy.ClientType = QQClientTypeInfo.ValueOfRaw(obj["client_type"].ToObject<int>());
                }
            }

            NotifyActionEvent(QQActionEventType.EvtOK, _buddy);
        }
    }
}
