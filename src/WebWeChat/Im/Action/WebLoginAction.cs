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
                    <message>OK</message>
                    <skey>xxx</skey>
                    <wxsid>xxx</wxsid>
                    <wxuin>xxx</wxuin>
                    <pass_ticket>xxx</pass_ticket>
                    <isgrayscale>1</isgrayscale>
                </error>             
            */
            var str = responseItem.ResponseString;
            var root = XDocument.Parse(str).Root;
            Session.Skey = root.Element("skey").Value;
            Session.Sid = root.Element("wxsid").Value;
            Session.Uin = root.Element("wxuin").Value;
            Session.PassTicket = root.Element("pass_ticket").Value;

            NotifyActionEvent(ActionEventType.EvtOK);
        }
    }
}
