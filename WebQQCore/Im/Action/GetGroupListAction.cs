using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Im.Service;
using iQQ.Net.WebQQCore.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>获取群列表名称</para>
    /// <para>@author ChenZhiHui</para>
    /// <para>@since 2013-2-21</para>
    /// </summary>
    public class GetGroupListAction : AbstractHttpAction
    {
        public GetGroupListAction(IQQContext context, QQActionEventHandler listener) : base(context, listener) { }

        public override QQHttpRequest OnBuildRequest()
        {
            IHttpService httpService = Context.GetSerivce<IHttpService>(QQServiceType.HTTP);
            QQHttpCookie ptwebqq = httpService.GetCookie("ptwebqq", QQConstants.URL_GET_USER_CATEGORIES);
            QQSession session = Context.Session;
            QQAccount account = Context.Account;

            JObject json = new JObject();
            json.Add("vfwebqq", session.Vfwebqq);
            json.Add("hash", QQEncryptor.GetHash(account.Uin + "", ptwebqq.Value));

            QQHttpRequest req = CreateHttpRequest("POST",
                    QQConstants.URL_GET_GROUP_NAME_LIST);
            req.AddPostValue("r", JsonConvert.SerializeObject(json));

            req.AddHeader("Referer", QQConstants.REFFER);

            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            // {"retcode":0,"result":{"gmasklist":[{"gid":1000,"mask":0},{"gid":1638195794,"mask":0},{"gid":321105219,"mask":0}],
            // "gnamelist":[{"flag":16777217,"name":"iQQ","gid":1638195794,"code":2357062609},{"flag":1048577,"name":"iQQ核心开发区","gid":321105219,"code":640215156}],"gmarklist":[]}}
            QQStore store = Context.Store;
            JObject json = JObject.Parse(response.GetResponseString());

            int retcode = json["retcode"].ToObject<int>();
            if (retcode == 0)
            {
                // 处理好友列表
                JObject results = json["result"].ToObject<JObject>();
                JArray groupJsonList = results["gnamelist"].ToObject<JArray>();	// 群列表

                // 禁止接收群消息标志：正常 0， 接收不提醒 1， 完全屏蔽 2
                JArray groupMaskJsonList = results["gmasklist"].ToObject<JArray>();

                foreach (JToken t in groupJsonList)
                {
                    JObject groupJson = t.ToObject<JObject>();
                    QQGroup group = new QQGroup();
                    group.Gin = groupJson["gid"].ToObject<long>();
                    group.Code = groupJson["code"].ToObject<long>();
                    group.Flag = groupJson["flag"].ToObject<int>();
                    group.Name = groupJson["name"].ToString();
                    //添加到Store
                    store.AddGroup(group);
                }

                foreach (JToken t in groupMaskJsonList)
                {
                    JObject maskObj = t.ToObject<JObject>();
                    long gid = maskObj["gid"].ToObject<long>();
                    int mask = maskObj["mask"].ToObject<int>();
                    QQGroup group = store.GetGroupByGin(gid);
                    if (group != null)
                    {
                        group.Mask = mask;
                    }
                }

                NotifyActionEvent(QQActionEventType.EVT_OK, store.GetGroupList());

            }
            else
            {
                // LOG.warn("unknown retcode: " + retcode);
                NotifyActionEvent(QQActionEventType.EVT_ERROR, null);
            }

        }

    }

}
