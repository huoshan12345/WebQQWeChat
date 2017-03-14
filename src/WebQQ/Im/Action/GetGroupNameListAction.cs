using System.Collections.Generic;
using System.Threading.Tasks;
using FclEx.Extensions;
using HttpAction.Core;
using HttpAction.Event;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Core;
using WebQQ.Util;
using WebQQ.Im.Bean;
using WebQQ.Im.Bean.Group;


namespace WebQQ.Im.Action
{
    public class GetGroupNameListAction : WebQQAction
    {
        public GetGroupNameListAction(IQQContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        public override HttpRequestItem BuildRequest()
        {
            var req = HttpRequestItem.CreateFormRequest(ApiUrls.GetGroupNameList);
            var json = new JObject
            {
                {"vfwebqq", Session.Vfwebqq},
                {"hash", QQEncryptor.Hash(Session.User.Uin, Session.Ptwebqq)}
            };
            req.AddQueryValue("r", json.ToSimpleString());
            req.Referrer = ApiUrls.Referrer;
            return req;
        }

        public override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            /*
                {
                    retcode: 0, 
                    result: {
                        gmasklist: [
                            {
                                gid: 1000, 
                                mask: "3"
                            }
                        ], 
                        gnamelist: [
                            {
                                flag: 17957889, 
                                name: " 獨①無Ⅱ", 
                                gid: 809647823, 
                                code: 3675041622
                            },            
                        ], 
                        gmarklist: [
                            {
                                uin: 2944307192,
                                markname: "郑问"
                            }
                        ]
                    }
                }
             */
            var json = response.ResponseString.ToJToken();
            if (json["retcode"].ToString() == "0")
            {
                var result = json["result"];

                var groups = result["gnamelist"].ToObject<List<QQGroup>>();
                groups.ForEach(Store.AddGroup);

                // 用不上
                //var gMaskList = result["gmasklist"].ToJArray();
                //foreach (var gMask in gMaskList)
                //{
                //    var gid = gMask["gid"].ToLong();
                //    var group = Store.GetGroupByGid(gid);
                //    if (group.IsNotNull())
                //        group.Mask = gMask["mask"].ToInt();
                //}

                // 群备注
                var gMarkList = result["gmarklist"].ToJArray();
                foreach (var gMark in gMarkList)
                {
                    var gid = gMark["uin"].ToLong();
                    Store.GroupDic.GetAndDo(gid, group => group.MarkName = gMark["markname"].ToString());
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
