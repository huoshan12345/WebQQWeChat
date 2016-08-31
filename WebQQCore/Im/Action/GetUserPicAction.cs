using System.IO;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Bean.Content;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>获取聊天图片</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class GetUserPicAction : AbstractHttpAction
    {
        private CFaceItem cface;
        private QQMsg msg;
        private Stream picOut;

        public GetUserPicAction(IQQContext context, QQActionEventHandler listener,
            CFaceItem cface, QQMsg msg, Stream picOut)
            : base(context, listener)
        {

            this.cface = cface;
            this.msg = msg;
            this.picOut = picOut;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            NotifyActionEvent(QQActionEventType.EVT_OK, cface);
        }

        public override QQHttpRequest OnBuildRequest()
        {
            var req = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_GET_CFACE2);

            //		clientid=12202920
            //		count=5
            //		guid=4D72EF8CF64D53DECB31ABC2B601AB23.jpg
            //		lcid=16059	//msg_id
            //		psessionid=8368046764001e636f6e6e7365727665725f77656271714031302e3133332e34312e32303200002a5400000a2c026e04004f95190e6d0000000a40345a4e79386b71416e6d000000280adff44c88196358dadc9fa075334fd6293f7e6a0020a86cad689c240384e54cbb329be8dd5f0c3f
            //		time=1
            //		to=3559750777 //from_uin

            var session = Context.Session;
            req.AddGetValue("clientid", session.ClientId);
            req.AddGetValue("to", msg.From.Uin);
            req.AddGetValue("guid", cface.FileName);
            req.AddGetValue("psessionid", session.SessionId);
            req.AddGetValue("count", "5");
            req.AddGetValue("lcid", msg.Id);
            req.AddGetValue("time", "1");
            req.OutputStream = picOut;
            return req;
        }
    }
}
