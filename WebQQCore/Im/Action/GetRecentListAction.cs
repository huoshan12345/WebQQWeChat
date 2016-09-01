using System.Collections.Generic;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>获取最近联系人列表</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class GetRecentListAction : AbstractHttpAction
    {
        public GetRecentListAction(IQQContext context, QQActionEventHandler listener) : base(context, listener) { }

        public override QQHttpRequest OnBuildRequest()
        {
            var session = Context.Session;

            var json = new JObject();
            json.Add("vfwebqq", session.Vfwebqq);
            json.Add("clientid", session.ClientId);
            json.Add("psessionid", session.SessionId);

            var req = CreateHttpRequest("POST", QQConstants.URL_GET_RECENT_LIST);
            req.AddPostValue("r", JsonConvert.SerializeObject(json));
            req.AddPostValue("clientid", session.ClientId);
            req.AddPostValue("psessionid", session.SessionId);

            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var json = JObject.Parse(response.GetResponseString());
            var recents = new List<object>();
            var store = Context.Store;
            if (json["retcode"].ToString() == "0")
            {
                var result = json["result"].ToObject<JArray>();
                for (var i = 0; i < result.Count; i++)
                {
                    var rejson = result[i].ToObject<JObject>();
                    switch (rejson["type"].ToObject<int>())
                    {
                        case 0:
                        {	//好友
                            var buddy = store.GetBuddyByUin(rejson["uin"].ToObject<long>());
                            if (buddy != null)
                            {
                                recents.Add(buddy);
                            }
                            break;
                        }

                        case 1:
                        {	//群
                            var group = store.GetGroupByCode(rejson["uin"].ToObject<long>());
                            if (group != null)
                            {
                                recents.Add(group);
                            }
                            break;
                        }

                        case 2:
                        {	//讨论组
                            var discuz = store.GetDiscuzByDid(rejson["uin"].ToObject<long>());
                            if (discuz != null)
                            {
                                recents.Add(discuz);
                            }
                            break;
                        }
                    }
                }
                NotifyActionEvent(QQActionEventType.EvtOK, recents);
            }
            else
            {
                NotifyActionEvent(QQActionEventType.EvtError, new QQException(QQErrorCode.UNEXPECTED_RESPONSE));
            }
        }

    }
}
