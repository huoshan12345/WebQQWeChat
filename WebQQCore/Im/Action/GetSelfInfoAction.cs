using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;
using iQQ.Net.WebQQCore.Util.Log;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Action
{
    public class GetSelfInfoAction : AbstractHttpAction
    {

        public GetSelfInfoAction(IQQContext context, QQActionEventHandler listener) : base(context, listener)
        {

        }


        public override QQHttpRequest OnBuildRequest()
        {
            var req = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_GET_SELF_INFO);
            req.AddGetValue("t", (DateUtils.NowTimestamp() / 1000).ToString());
            req.AddHeader("Referer", QQConstants.REFERER_S);
            return req;
        }



        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var json = JObject.Parse(response.GetResponseString());
            var account = Context.Account;
            if (json["retcode"].ToString() == "0")
            {
                var obj = json["result"].ToObject<JObject>();
                try
                {
                    account.Birthday = DateUtils.Parse(obj["birthday"].ToObject<JObject>());
                }
                catch (Exception e)
                {
                    MyLogger.Default.Warn($"日期转换失败：{obj["birthday"]}", e);
                    account.Birthday = null;
                }

                account.Occupation = obj["occupation"].ToString();
                account.Phone = obj["phone"].ToString();
                account.Allow = (QQAllow) obj["allow"].ToObject<int>();
                account.College = obj["college"].ToString();
                account.RegTime = obj["reg_time"]?.ToObject<int>() ?? 0;
                account.Uin = obj["uin"].ToObject<long>();
                account.Constel = obj["constel"].ToObject<int>();
                account.Blood = obj["blood"].ToObject<int>();
                account.Homepage = obj["homepage"].ToString();
                account.Stat = obj["stat"]?.ToObject<int>() ?? 0;
                account.Sign = obj["lnick"].ToString();
                account.VipLevel = obj["vip_info"].ToObject<int>(); // VIP等级 0为非VIP
                account.Country = obj["country"].ToString();
                account.City = obj["city"].ToString();
                account.Personal = obj["personal"].ToString();
                account.Nickname = obj["nick"].ToString();
                account.ChineseZodiac = obj["shengxiao"].ToObject<int>();
                account.Email = obj["email"].ToString();
                account.Province = obj["province"].ToString();
                account.Gender = obj["gender"].ToString();
                account.Mobile = obj["mobile"].ToString();
                account.Vfwebqq = obj["vfwebqq"].ToString();
                if (obj["client_type"]!=null)
                {
                    account.ClientType = QQClientType.ValueOfRaw(obj["client_type"].ToString());
                }

            }

            NotifyActionEvent(QQActionEventType.EVT_OK, account);
        }

    }
}
