using System.Threading.Tasks;
using FclEx.Extensions;
using HttpAction.Core;
using FclEx.Http.Event;
using HttpAction;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Bean;
using WebQQ.Im.Core;

namespace WebQQ.Im.Actions
{
    public class GetSelfInfoAction : WebQQInfoAction
    {
        public GetSelfInfoAction(IQQContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        protected override void ModifyRequest(HttpRequestItem req)
        {
            req.AddData("t", Timestamp);
            req.Referrer = ApiUrls.Referrer;
        }

        protected override void HandleResult(JToken json)
        {
            /*
                {
                    retcode: 0, 
                    result: {
                        birthday: {
                            month: 11, 
                            year: 1990, 
                            day: 14
                        }, 
                        face: 252, 
                        phone: "83544376", 
                        occupation: "计算机/互联网/IT", 
                        allow: 1, 
                        college: "北京师大附中", 
                        uin: 89009143, 
                        blood: 2, 
                        constel: 12, 
                        lnick: "努力去香港", 
                        vfwebqq: "7454728c49b49db8eba34dad1df1704bffab25ce52f037516fed029e05a681473d01af3d85172a96", 
                        homepage: "http://tieba.baidu.com/f?kw=%C0%EE%BE%AD", 
                        vip_info: 7, 
                        city: "西城", 
                        country: "中国", 
                        personal: "", 
                        shengxiao: 7, 
                        nick: "月光双刀", 
                        email: "89009143@qq.com", 
                        province: "北京", 
                        account: 89009143, 
                        gender: "male", 
                        mobile: "152********"
                    }
                }             
             */

            var info = json["result"].ToObject<SelfInfo>();
            info.MapTo(Session.User);
        }
    }
}
