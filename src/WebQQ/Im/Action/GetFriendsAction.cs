using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpAction.Core;
using HttpAction.Event;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Bean;
using WebQQ.Im.Core;
using WebQQ.Util;

namespace WebQQ.Im.Action
{
    public class GetFriendsAction : QQAction
    {
        public GetFriendsAction(IQQContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        public override HttpRequestItem BuildRequest()
        {
            var json = new JObject
            {
                {"h", "hello"},
                {"vfwebqq", Session.Vfwebqq},
                {"hash", QQEncryptor.Hash(Account.User.Uin, Session.Ptwebqq)}
            };
            var req = HttpRequestItem.CreateFormRequest(ApiUrls.GetFriends);
            req.AddQueryValue("r", json.ToSimpleString());
            req.Referrer = ApiUrls.ReferrerS;
            return req;
        }

        public override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            /*             
                {
                    retcode: 0, 
                    result: {
                        friends: [
                            {
                                flag: 4, 
                                uin: 3246906007, 
                                categories: 1
                            },
                        ], 
                        marknames: [
                            {
                                uin: 3246906007, 
                                markname: "xxxxxxx", 
                                type: 0
                            }, 
                        ], 
                        categories: [
                            {
                                index: 0, 
                                sort: 1, 
                                name: "我的好友"
                            }, 
                        ], 
                        vipinfo: [
                            {
                                vip_level: 0, 
                                u: 4091731542, 
                                is_vip: 0
                            }, 
                        ], 
                        info: [
                            {
                                face: 0, 
                                flag: 29884998, 
                                nick: "!空*^白^%", 
                                uin: 3246906007
                            }, 
                        ]
                    }
                }             
             */
            var json = response.ResponseString.ToJObject();
            if (json["retcode"].ToString() == "0")
            {
                var result = json["result"];

                // 分组
                //var categories = result["categories"].ToJArray();
                //foreach (var category in categories)
                //{
                //    var obj = new Category
                //    {
                //        Index = category["index"].ToObject<int>(),
                //        Name = category["name"].ToString(),
                //        Sort = category["sort"].ToObject<int>()
                //    };
                //    Store.AddCategory(obj);
                //}
                var categories = result["categories"].ToObject<List<Category>>();
                categories.ForEach(Store.AddCategory);

                // 好友
                //var friends = result["friends"].ToJArray();
                //foreach (var friend in friends)
                //{
                //    var obj = new Friend
                //    {
                //        Uin = friend["uin"].ToLong(),
                //        CategoryIndex = friend["categories"].ToInt(),
                //        Flag = friend["flag"].ToInt(),
                //    };
                //    Store.AddFriend(obj);
                //}
                var friends = result["friends"].ToObject<List<Friend>>();
                friends.ForEach(Store.AddFriend);

                // 好友信息 face/flag/nick/uin
                var infos = result["info"].ToJArray();
                foreach (var info in infos)
                {
                    var uin = info["uin"].ToObject<long>();
                    var friend = Store.GetFriendByUin(uin);
                    friend.Face = info["face"].ToInt();
                    friend.InfoFlag = info["flag"].ToInt();
                    friend.Nickname = info["nick"].ToString();
                }

                // 好友备注 uin/markname/type
                var marknames = result["marknames"].ToObject<JArray>();
                foreach (var markname in marknames)
                {
                    var uin = markname["uin"].ToObject<long>();
                    var friend = Store.GetFriendByUin(uin);
                    friend.MarkName = markname["markname"].ToString();
                    friend.MarknameType = markname["type"].ToInt();
                }

                // 好友vip信息
                var vipInfos = result["vipinfo"].ToObject<JArray>();
                foreach (var vipInfo in vipInfos)
                {
                    var uin = vipInfo["u"].ToLong();
                    var friend = Store.GetFriendByUin(uin);
                    friend.VipLevel = vipInfo["vip_level"].ToInt();
                    friend.IsVip = vipInfo["is_vip"].ToInt() != 0;
                }
                return NotifyActionEventAsync(ActionEventType.EvtOK);
            }
            else
            {
                throw new QQException(QQErrorCode.ResponseError, response.ResponseString);
            }
        }
    }
}
