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
            QQSession session = Context.Session;

            JObject json = new JObject();
            json.Add("vfwebqq", session.Vfwebqq);
            json.Add("clientid", session.ClientId);
            json.Add("psessionid", session.SessionId);

            QQHttpRequest req = CreateHttpRequest("POST", QQConstants.URL_GET_RECENT_LIST);
            req.AddPostValue("r", JsonConvert.SerializeObject(json));
            req.AddPostValue("clientid", session.ClientId);
            req.AddPostValue("psessionid", session.SessionId);

            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            JObject json = JObject.Parse(response.GetResponseString());
            List<object> recents = new List<object>();
            QQStore store = Context.Store;
            if (json["retcode"].ToString() == "0")
            {
                JArray result = json["result"].ToObject<JArray>();
                for (int i = 0; i < result.Count; i++)
                {
                    JObject rejson = result[i].ToObject<JObject>();
                    switch (rejson["type"].ToObject<int>())
                    {
                        case 0:
                        {	//好友
                            QQBuddy buddy = store.GetBuddyByUin(rejson["uin"].ToObject<long>());
                            if (buddy != null)
                            {
                                recents.Add(buddy);
                            }
                            break;
                        }

                        case 1:
                        {	//群
                            QQGroup group = store.GetGroupByCode(rejson["uin"].ToObject<long>());
                            if (group != null)
                            {
                                recents.Add(group);
                            }
                            break;
                        }

                        case 2:
                        {	//讨论组
                            QQDiscuz discuz = store.GetDiscuzByDid(rejson["uin"].ToObject<long>());
                            if (discuz != null)
                            {
                                recents.Add(discuz);
                            }
                            break;
                        }
                    }
                }
                NotifyActionEvent(QQActionEventType.EVT_OK, recents);
            }
            else
            {
                NotifyActionEvent(QQActionEventType.EVT_ERROR, new QQException(QQErrorCode.UNEXPECTED_RESPONSE));
            }
        }

    }
}
