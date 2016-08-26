using System;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Im.Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Action
{
    public class ChannelLoginAction : AbstractHttpAction
    {
        private readonly QQStatus _status;

        public ChannelLoginAction(IQQContext context, QQActionEventHandler listener, QQStatus status)
            : base(context, listener)
        {
            this._status = status;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            var httpService = Context.GetSerivce<IHttpService>(QQServiceType.HTTP);
            var session = Context.Session;
            session.Ptwebqq = httpService.GetCookie("ptwebqq", QQConstants.URL_CHANNEL_LOGIN).Value;
            var json = new JObject
            {
                {"ptwebqq", httpService.GetCookie("ptwebqq", QQConstants.URL_CHANNEL_LOGIN).Value},
                {"clientid", session.ClientId.ToString()},
                {"psessionid", session.SessionId},
                {"status", _status.Value},
                {"passwd_sig", ""}
            };

            var req = CreateHttpRequest("POST", QQConstants.URL_CHANNEL_LOGIN);
            req.AddPostValue("r", JsonConvert.SerializeObject(json));
            //req.AddPostValue("clientid", session.ClientId);
            //req.AddPostValue("psessionid", session.SessionId ?? "null");
            req.AddHeader("Referer", QQConstants.REFFER);
            req.AddHeader("Origin", QQConstants.ORIGIN);
            req.AddHeader("Connection", "Keep-Alive");

            return req;
        }

        

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            //{"retcode":0,"result":{"uin":236557647,"cip":1991953329,"index":1075,"port":51494,"status":"online","vfwebqq":"41778677efd86bae2ed575eea02349046a36f3f53298a34b97d75297ec1e67f6ee5226429daa6aa7","psessionid":"8368046764001d636f6e6e7365727665725f77656271714031302e3133332e342e31373200005b9200000549016e04004f95190e6d0000000a4052347371696a62724f6d0000002841778677efd86bae2ed575eea02349046a36f3f53298a34b97d75297ec1e67f6ee5226429daa6aa7","user_state":0,"f":0}}

            var str = response.GetResponseString();
            var json = JObject.Parse(str);
            var session = Context.Session;
            var account = Context.Account;
            if (json["retcode"].ToString() == "0")
            {
                var ret = json["result"].ToObject<JObject>();
                account.Uin = ret["uin"].ToObject<long>();
                account.QQ = ret["uin"].ToObject<long>();
                session.SessionId = ret["psessionid"].ToString();
                // session.Vfwebqq = ret["vfwebqq"].ToString();
                account.Status = QQStatus.ValueOfRaw(ret["status"].ToString());
                session.State = QQSessionState.ONLINE;
                session.Index = ret["index"].ToObject<int>();
                session.Port = ret["port"].ToObject<int>();
                NotifyActionEvent(QQActionEventType.EVT_OK, null);
            }
            else
            {
                NotifyActionEvent(QQActionEventType.EVT_ERROR, new QQException(QQErrorCode.INVALID_RESPONSE));	//TODO ..
            }
        }

    }

}
