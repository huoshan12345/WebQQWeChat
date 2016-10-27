using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpActionFrame.Action;
using HttpActionFrame.Core;
using HttpActionFrame.Event;
using WebWeChat.Im.Core;
using System.Xml.Linq;

namespace WebWeChat.Im.Action
{
    public class WebLoginAction : AbstractWebWeChatAction
    {
        public WebLoginAction(IWeChatContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        public override HttpRequestItem BuildRequest()
        {
            return new HttpRequestItem(HttpMethodType.Get, Session.LoginUrl);
        }

        public override void OnHttpContent(HttpResponseItem responseItem)
        {
            var str = responseItem.ResponseString;

            var root = XDocument.Parse(str).Root;
            var skey = root.Element("skey");
            var wxsid = root.Element("wxsid");
            var wxuin = root.Element("wxuin");
            var passTicket = root.Element("pass_ticket");



            NotifyActionEvent(ActionEventType.EvtOK);
        }
    }
}
