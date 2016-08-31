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
 
        public GetBuddyListAction(IQQContext context, QQActionEventHandler listener) : base(context, listener) { }

        public override QQHttpRequest OnBuildRequest()
        {
            var session = Context.Session;
            var account = Context.Account;
            var httpService = Context.GetSerivce<IHttpService>(QQServiceType.HTTP);
            // var ptwebqq = httpService.GetCookie("ptwebqq", QQConstants.URL_GET_USER_CATEGORIES);

            var json = new JObject
            {
                {"h", "hello"},
                {"vfwebqq", session.Vfwebqq},
                {"hash", QQEncryptor.GetHash(account.Uin.ToString(), session.Ptwebqq)}
            };
            // 同上

            var req = CreateHttpRequest("POST", QQConstants.URL_GET_USER_CATEGORIES);
            req.AddPostValue("r", JsonConvert.SerializeObject(json));
            req.AddHeader("Referer", QQConstants.REFERER_S);
            req.AddHeader("Origin", UrlUtils.GetOrigin(QQConstants.URL_GET_USER_CATEGORIES));
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var json = JObject.Parse(response.GetResponseString());
            var str = JsonConvert.SerializeObject(json);


            var retcode = json["retcode"].ToObject<int>();
            if (retcode == 0)
            {
                var store = Context.Store;
                // 处理好友列表
                var results = json["result"].ToObject<JObject>();
                // 获取JSON列表信息
                var jsonCategories = results["categories"].ToObject<JArray>();
                // 获取JSON好友基本信息列表 flag/uin/categories
                var jsonFriends = results["friends"].ToObject<JArray>();
                // face/flag/nick/uin
                var jsonInfo = results["info"].ToObject<JArray>();
                // uin/markname/
                var jsonMarknames = results["marknames"].ToObject<JArray>();
                // vip_level/u/is_vip
                var jsonVipinfo = results["vipinfo"].ToObject<JArray>();

                // 默认好友列表
                var c = new QQCategory() { Index = 0, Name = "我的好友", Sort = 0 };
                store.AddCategory(c);
                // 初始化好友列表
                foreach (var t in jsonCategories)
                {
                    var jsonCategory = t.ToObject<JObject>();
                    var qqc = new QQCategory();
                    qqc.Index = jsonCategory["index"].ToObject<int>();
                    qqc.Name = jsonCategory["name"].ToString();
                    qqc.Sort = jsonCategory["sort"].ToObject<int>();
                    store.AddCategory(qqc);
                }
                // 处理好友基本信息列表 flag/uin/categories
                foreach (var t in jsonFriends)
                {
                    var buddy = new QQBuddy();
                    var jsonFriend = t.ToObject<JObject>();
                    var uin = jsonFriend["uin"].ToObject<long>();
                    buddy.Uin = uin;
                    buddy.Status = QQStatus.OFFLINE;
                    buddy.ClientType = QQClientType.Unknown;
                    // 添加到列表中
                    var category = jsonFriend["categories"].ToObject<int>();
                    var qqCategory = store.GetCategoryByIndex(category);
                    buddy.Category = qqCategory;
                    qqCategory.BuddyList.Add(buddy);

                    // 记录引用
                    store.AddBuddy(buddy);
                }
                // face/flag/nick/uin
                foreach (var t in jsonInfo)
                {
                    var info = t.ToObject<JObject>();
                    var uin = info["uin"].ToObject<long>();
                    var buddy = store.GetBuddyByUin(uin);
                    buddy.Nickname = info["nick"].ToString();
                }
                // uin/markname
                foreach (var t in jsonMarknames)
                {
                    var jsonMarkname = t.ToObject<JObject>();
                    var uin = jsonMarkname["uin"].ToObject<long>();
                    var buddy = store.GetBuddyByUin(uin);
                    if (buddy != null)
                    {
                        buddy.MarkName = jsonMarkname["markname"].ToString();
                    }
                }
                // vip_level/u/is_vip
                foreach (var t in jsonVipinfo)
                {
                    var vipInfo = t.ToObject<JObject>();
                    var uin = vipInfo["u"].ToObject<long>();
                    var buddy = store.GetBuddyByUin(uin);
                    buddy.VipLevel = vipInfo["vip_level"].ToObject<int>();
                    var isVip = vipInfo["is_vip"].ToObject<int>();
                    buddy.IsVip = isVip != 0;
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
