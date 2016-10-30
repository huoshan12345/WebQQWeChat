using System;
using System.Text.RegularExpressions;
using Utility.HttpAction.Core;
using Utility.HttpAction.Event;
using WebWeChat.Im.Core;

namespace WebWeChat.Im.Action
{
    /// <summary>
    /// 等待扫码登录
    /// </summary>
    public class WatiForLoginAction : WebWeChatAction
    {
        private readonly Regex _regCode = new Regex(@"window.code=(\d+);");
        private readonly Regex _regUrl = new Regex(@"window.redirect_uri=""(\S+?)"";");
        private readonly int _tip;

        public WatiForLoginAction(int tip, IWeChatContext context, ActionEventListener listener = null) 
            : base(context, listener)
        {
            _tip = tip;
        }

        public override HttpRequestItem BuildRequest()
        {
            var req = new HttpRequestItem(HttpMethodType.Post, ApiUrls.CheckQRCode);
            req.AddQueryValue("tip", _tip);
            req.AddQueryValue("uuid", Session.Uuid);
            req.AddQueryValue("_", Timestamp);
            return req;
        }

        public override void OnHttpContent(HttpResponseItem responseItem)
        {
            var str = responseItem.ResponseString;
            var match = _regCode.Match(str);
            if (match.Success)
            {
                var code = match.Groups[1].Value;
                switch (code)
                {
                    case "200":
                        {
                            var m = _regUrl.Match(str);
                            if (m.Success)
                            {
                                Session.LoginUrl = $"{m.Groups[1].Value}&fun=new";
                                Session.BaseUrl = Session.LoginUrl.Substring(0, Session.LoginUrl.LastIndexOf("/", StringComparison.OrdinalIgnoreCase));
                                NotifyActionEvent(ActionEventType.EvtOK);
                                return;
                            }
                            break;
                        }

                    case "201": NotifyActionEvent(ActionEventType.EvtOK); return;
                    case "408": NotifyErrorEvent(WeChatErrorCode.Timeout); return;
                }
            }
            throw WeChatException.CreateException(WeChatErrorCode.ResponseError);
        }
    }
}
