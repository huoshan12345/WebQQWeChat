using WebWeChat.Im.Core;
using System.Xml.Linq;
using Utility.HttpAction.Core;
using Utility.HttpAction.Event;

namespace WebWeChat.Im.Action
{
    public class WebLoginAction : WebWeChatAction
    {
        /// <summary>
        /// 获取登录参数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="listener"></param>
        public WebLoginAction(IWeChatContext context, ActionEventListener listener = null)
            : base(context, listener)
        {
        }

        public override HttpRequestItem BuildRequest()
        {
            return new HttpRequestItem(HttpMethodType.Get, Session.LoginUrl);
        }

        public override void OnHttpContent(HttpResponseItem responseItem)
        {
            /*
                <error>
                  <ret>0</ret>
                  <message/>
                  <skey>@crypt_c498484a_beffad67aa727e24f7c669c51d5c895f</skey>
                  <wxsid>Lr2AUrW1+FSCmtHZ</wxsid>
                  <wxuin>463678295</wxuin>
                  <pass_ticket>SSR%2BWEx6yJf8MTN2G2XjsWtRpXWQ0J6wBHc5BeHGL3gATmsW%2FMiFX0GBqWrmm7dN</pass_ticket>
                  <isgrayscale>1</isgrayscale>
                </error>            
            */
            var str = responseItem.ResponseString;
            var root = XDocument.Parse(str).Root;
            Session.Skey = root.Element("skey").Value;
            Session.Sid = root.Element("wxsid").Value;
            Session.Uin = root.Element("wxuin").Value;
            Session.PassTicket = root.Element("pass_ticket").Value;

            Session.State = SessionState.Online;

            NotifyActionEvent(ActionEventType.EvtOK);
        }
    }
}
