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
    /// <para>获取好友信息的请求</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class GetFriendInfoAction : AbstractHttpAction
    {
        private QQUser buddy;

        public GetFriendInfoAction(IQQContext context, QQActionEventHandler listener, QQUser buddy)
            : base(context, listener)
        {
            this.buddy = buddy;
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
            req.AddGetValue("tuin", buddy.Uin);
            req.AddGetValue("verifysession", "");	//难道有验证码？？？
            req.AddGetValue("code", "");
            req.AddGetValue("vfwebqq", session.Vfwebqq);
            req.AddGetValue("t", DateTime.Now.CurrentTimeSeconds());

            req.AddHeader("Referer", QQConstants.REFFER);
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var json = JObject.Parse(response.GetResponseString());
            if (json["retcode"].ToString() == "0")
            {
                var obj = json["result"].ToObject<JObject>();
                try
                {
                    buddy.Birthday = DateUtils.Parse(obj["birthday"].ToObject<JObject>());
                }
                catch (FormatException e)
                {
                    DefaultLogger.Warn($"日期转换失败：{obj["birthday"]}", e);
                    buddy.Birthday = null;
                }
                buddy.Occupation = obj["occupation"].ToString();
                buddy.Phone = obj["phone"].ToString();
                buddy.Allow = (QQAllow)obj["allow"].ToObject<int>();

                buddy.College = obj["college"].ToString();
                if (obj["reg_time"] != null)
                {
                    buddy.RegTime = obj["reg_time"].ToObject<int>();
                }
                buddy.Uin = obj["uin"].ToObject<long>();
                buddy.Constel = obj["constel"].ToObject<int>();
                buddy.Blood = obj["blood"].ToObject<int>();
                buddy.Homepage = obj["homepage"].ToString();
                buddy.Stat = obj["stat"].ToObject<int>();
                buddy.VipLevel = obj["vip_info"].ToObject<int>(); // VIP等级 0为非VIP
                buddy.Country = obj["country"].ToString();
                buddy.City = obj["city"].ToString();
                buddy.Personal = obj["personal"].ToString();
                buddy.Nickname = obj["nick"].ToString();
                buddy.ChineseZodiac = obj["shengxiao"].ToObject<int>();
                buddy.Email = obj["email"].ToString();
                buddy.Province = obj["province"].ToString();
                buddy.Gender = obj["gender"].ToString();
                buddy.Mobile = obj["mobile"].ToString();
                if (obj["client_type"] != null)
                {
                    buddy.ClientType = QQClientType.ValueOfRaw(obj["client_type"].ToObject<int>());
                }
            }

            NotifyActionEvent(QQActionEventType.EVT_OK, buddy);
        }
    }
}
