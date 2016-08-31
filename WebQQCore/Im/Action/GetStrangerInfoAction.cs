using System;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>获取陌生人信息</para>
    /// <para>@author ChenZhiHui</para>
    /// <para>@since 2013-4-21</para>
    /// </summary>
    public class GetStrangerInfoAction : AbstractHttpAction
    {
        private QQUser user;

        public GetStrangerInfoAction(IQQContext context, QQActionEventHandler listener,
                QQUser user)
            : base(context, listener)
        {
            this.user = user;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            var session = Context.Session;
            var req = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_GET_STRANGER_INFO);
            req.AddGetValue("tuin", user.Uin);
            req.AddGetValue("verifysession", "");	// ?
            req.AddGetValue("gid", "0");
            req.AddGetValue("code", "");
            req.AddGetValue("vfwebqq", session.Vfwebqq);
            req.AddGetValue("t", DateTime.Now.CurrentTimeSeconds());
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            /*
             * {"retcode":0,"result":
             * {"face":0,"birthday":{"month":0,"year":0,"day":0},
             * "phone":"","occupation":"","allow":1,"college":"","uin":2842465527,"blood":0,
             * "constel":0,"homepage":"","stat":10,"country":"","city":"","personal":"","nick":"平凡",
             * "shengxiao":0,"email":"","token":"d04e802bda6ff115e31c3792199f15aa74f92eb435e75d93",
             * "client_type":1,"province":"","gender":"male","mobile":"-"}}
             */
            try
            {
                var json = JObject.Parse(response.GetResponseString());
                if (json["retcode"].ToString() == "0")
                {
                    var obj = json["result"].ToObject<JObject>();
                    try
                    {
                        user.Birthday = DateUtils.Parse(obj["birthday"].ToObject<JObject>());
                    }
                    catch (FormatException e)
                    {
                        MyLogger.Default.Warn($"日期转换失败：{obj["birthday"]}", e);
                        user.Birthday = null;
                    }
                    user.Occupation = obj["occupation"].ToString();
                    user.Phone = obj["phone"].ToString();
                    user.Allow = (QQAllow) obj["allow"].ToObject<int>();
                    user.College = obj["college"].ToString();
                    if (obj["reg_time"] != null)
                    {
                        user.RegTime = obj["reg_time"].ToObject<int>();
                    }
                    user.Uin = obj["uin"].ToObject<long>();
                    user.Constel = obj["constel"].ToObject<int>();
                    user.Blood = obj["blood"].ToObject<int>();
                    user.Homepage = obj["homepage"].ToString();
                    user.Stat = obj["stat"].ToObject<int>();
                    if (obj["vip_info"] != null)
                    {
                        user.VipLevel = obj["vip_info"].ToObject<int>(); // VIP等级 0为非VIP
                    }
                    user.Country = obj["country"].ToString();
                    user.City = obj["city"].ToString();
                    user.Personal = obj["personal"].ToString();
                    user.Nickname = obj["nick"].ToString();
                    user.ChineseZodiac = obj["shengxiao"].ToObject<int>();
                    user.Email = obj["email"].ToString();
                    user.Province = obj["province"].ToString();
                    user.Gender = obj["gender"].ToString();
                    user.Mobile = obj["mobile"].ToString();
                    if (obj["client_type"] != null)
                    {
                        user.ClientType = QQClientType.ValueOfRaw(obj["client_type"].ToObject<int>());
                    }
                }
            }
            catch (Exception e)
            {
                MyLogger.Default.Warn(e.Message, e);
            }
            NotifyActionEvent(QQActionEventType.EVT_OK, user);
        }

    }

}
