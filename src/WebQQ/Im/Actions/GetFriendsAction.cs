using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FclEx.Extensions;
using HttpAction;
using HttpAction.Core;
using HttpAction.Event;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Bean;
using WebQQ.Im.Bean.Friend;
using WebQQ.Im.Core;
using WebQQ.Util;

namespace WebQQ.Im.Actions
{
    public class GetFriendsAction : WebQQInfoAction
    {
        protected override EnumRequestType RequestType { get; } = EnumRequestType.Form; 

        public GetFriendsAction(IQQContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        /*
            {
                "vfwebqq": "172b053b34ebc4445c89db2e59f0a946eaad7daa32140791d641f219161b91dbdf1f34ad463d0cce",
                "hash": "0040000D006400BC"
            }
         */
        protected override void ModifyRequest(HttpRequestItem req)
        {
            var json = new JObject
            {
                {"vfwebqq", Session.Vfwebqq},
                {"hash", QQEncryptor.Hash(Session.User.Uin, Session.Ptwebqq)}
            };
            req.AddData("r", json.ToSimpleString());
            req.Referrer = ApiUrls.ReferrerS;
        }

        protected override void HandleResult(JToken json)
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
        }
    }
}
