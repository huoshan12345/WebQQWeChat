using System;
using System.Drawing;
using System.Text.RegularExpressions;
using HttpActionFrame.Core;
using HttpActionFrame.Event;
using WebWeChat.Im.Core;

namespace WebWeChat.Im.Action
{
    public class GetUuidAction : AbstractWebWeChatAction
    {
        private readonly Regex _reg = new Regex(@"window.QRLogin.code = (\d+); window.QRLogin.uuid = ""(\S+?)""");

        public GetUuidAction(IWeChatContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        public override HttpRequestItem BuildRequest()
        {
            var req = new HttpRequestItem(HttpMethodType.Post, ApiUrls.GetUuid);
            req.AddQueryValue("appid", ApiUrls.Appid);
            req.AddQueryValue("fun", "new");
            req.AddQueryValue("lang", "zh_CN");
            req.AddQueryValue("_", Timestamp);
            return req;
        }

        public override void OnHttpContent(HttpResponseItem responseItem)
        {
            var str = responseItem.ResponseString;
            var match = _reg.Match(str);
            if (match.Success)
            {
                Session.Uuid = match.Groups[2].Value;
                NotifyActionEvent(ActionEventType.EvtOK, null);
            }
            else
            {
               // NotifyActionEvent(ActionEventType.EvtError, );
            }
        }
    }
}
