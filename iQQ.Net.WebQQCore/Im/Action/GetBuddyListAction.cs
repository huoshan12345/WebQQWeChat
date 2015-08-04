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
    /// <para>获取好友列表</para>
    /// <para>@author ChenZhiHui</para>
    /// <para>@since 2013-2-21</para>
    /// </summary>
    public class GetBuddyListAction : AbstractHttpAction
    {
 
        public GetBuddyListAction(QQContext context, QQActionEventHandler listener) : base(context, listener) { }

        public override QQHttpRequest OnBuildRequest()
        {
            QQSession session = Context.Session;
            QQAccount account = Context.Account;
            IHttpService httpService = Context.GetSerivce<IHttpService>(QQServiceType.HTTP);
            QQHttpCookie ptwebqq = httpService.GetCookie("ptwebqq", QQConstants.URL_GET_USER_CATEGORIES);

            JObject json = new JObject();
            json.Add("h", "hello");
            json.Add("vfwebqq", session.Vfwebqq); // 同上
            json.Add("hash", QQEncryptor.GetHash(account.Uin + "", ptwebqq.Value));

            QQHttpRequest req = CreateHttpRequest("POST", QQConstants.URL_GET_USER_CATEGORIES);
            req.AddPostValue("r", JsonConvert.SerializeObject(json));
            req.AddHeader("Referer", QQConstants.REFFER);
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            JObject json = JObject.Parse(response.GetResponseString());
            string str = JsonConvert.SerializeObject(json);


            int retcode = json["retcode"].ToObject<int>();
            if (retcode == 0)
            {
                QQStore store = Context.Store;
                // 处理好友列表
                JObject results = json["result"].ToObject<JObject>();
                // 获取JSON列表信息
                JArray jsonCategories = results["categories"].ToObject<JArray>();
                // 获取JSON好友基本信息列表 flag/uin/categories
                JArray jsonFriends = results["friends"].ToObject<JArray>();
                // face/flag/nick/uin
                JArray jsonInfo = results["info"].ToObject<JArray>();
                // uin/markname/
                JArray jsonMarknames = results["marknames"].ToObject<JArray>();
                // vip_level/u/is_vip
                JArray jsonVipinfo = results["vipinfo"].ToObject<JArray>();

                // 默认好友列表
                QQCategory c = new QQCategory() { Index = 0, Name = "我的好友", Sort = 0 };
                store.AddCategory(c);
                // 初始化好友列表
                foreach (JToken t in jsonCategories)
                {
                    JObject jsonCategory = t.ToObject<JObject>();
                    QQCategory qqc = new QQCategory();
                    qqc.Index = jsonCategory["index"].ToObject<int>();
                    qqc.Name = jsonCategory["name"].ToString();
                    qqc.Sort = jsonCategory["sort"].ToObject<int>();
                    store.AddCategory(qqc);
                }
                // 处理好友基本信息列表 flag/uin/categories
                foreach (JToken t in jsonFriends)
                {
                    QQBuddy buddy = new QQBuddy();
                    JObject jsonFriend = t.ToObject<JObject>();
                    long uin = jsonFriend["uin"].ToObject<long>();
                    buddy.Uin = uin;
                    buddy.Status = QQStatus.OFFLINE;
                    buddy.ClientType = QQClientType.UNKNOWN;
                    // 添加到列表中
                    int category = jsonFriend["categories"].ToObject<int>();
                    QQCategory qqCategory = store.GetCategoryByIndex(category);
                    buddy.Category = qqCategory;
                    qqCategory.BuddyList.Add(buddy);

                    // 记录引用
                    store.AddBuddy(buddy);
                }
                // face/flag/nick/uin
                foreach (JToken t in jsonInfo)
                {
                    JObject info = t.ToObject<JObject>();
                    long uin = info["uin"].ToObject<long>();
                    QQBuddy buddy = store.GetBuddyByUin(uin);
                    buddy.Nickname = info["nick"].ToString();
                }
                // uin/markname
                foreach (JToken t in jsonMarknames)
                {
                    JObject jsonMarkname = t.ToObject<JObject>();
                    long uin = jsonMarkname["uin"].ToObject<long>();
                    QQBuddy buddy = store.GetBuddyByUin(uin);
                    if (buddy != null)
                    {
                        buddy.MarkName = jsonMarkname["markname"].ToString();
                    }
                }
                // vip_level/u/is_vip
                foreach (JToken t in jsonVipinfo)
                {
                    JObject vipInfo = t.ToObject<JObject>();
                    long uin = vipInfo["u"].ToObject<long>();
                    QQBuddy buddy = store.GetBuddyByUin(uin);
                    buddy.VipLevel = vipInfo["vip_level"].ToObject<int>();
                    int isVip = vipInfo["is_vip"].ToObject<int>();
                    if (isVip != 0)
                    {
                        buddy.IsVip = true;
                    }
                    else
                    {
                        buddy.IsVip = false;
                    }
                }

                NotifyActionEvent(QQActionEventType.EVT_OK, store.GetCategoryList());

            }
            else
            {
                // LOG.warn("unknown retcode: " + retcode);
                NotifyActionEvent(QQActionEventType.EVT_ERROR, new QQException(QQErrorCode.ERROR_HTTP_STATUS, "unknown retcode: " + retcode));
            }
        }
    }
}
