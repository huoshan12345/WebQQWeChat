using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HttpAction.Core;
using HttpAction.Event;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Bean;
using WebQQ.Im.Core;
using WebQQ.Util;
using FxUtility.Extensions;
using WebQQ.Im.Bean.Friend;

namespace WebQQ.Im.Action
{
    public class GetFriendsAction : WebQQAction
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
                {"hash", QQEncryptor.Hash(Session.User.Uin, Session.Ptwebqq)}
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
            var json = response.ResponseString.ToJToken();
            if (json["retcode"].ToString() == "0")
            {
                var result = json["result"];

                var categories = result["categories"].ToObject<List<Category>>();
                categories.ForEach(Store.AddCategory);

                var friends = result["friends"].ToObject<List<QQFriend>>();
                friends.ForEach(Store.AddFriend);

                // 好友信息 face/flag/nick/uin
                var infos = result["info"].ToObject<FriendBaseInfo[]>();
                foreach (var info in infos)
                {
                    Store.FriendDic.GetAndDo(info.Uin, m => Mapper.Map(info, m));
                }

                // 好友备注 uin/markname/type
                var marknames = result["marknames"].ToObject<FriendMarkName[]>();
                foreach (var markname in marknames)
                {
                    Store.FriendDic.GetAndDo(markname.Uin, m => Mapper.Map(markname, m));
                }

                // vip信息
                var mVipInfo = result["vipinfo"].ToObject<UserVipInfo[]>();
                foreach (var vip in mVipInfo)
                {
                    Store.FriendDic.GetAndDo(vip.Uin, m => Mapper.Map(vip, m));
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
