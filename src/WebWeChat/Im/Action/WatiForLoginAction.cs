using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utility.HttpAction.Core;
using Utility.HttpAction.Event;
using WebWeChat.Im.Core;

namespace WebWeChat.Im.Action
{
    /// <summary>
    /// 等待扫码登录
    /// </summary>
    public class WatiForLoginAction : WeChatAction
    {
        private readonly Regex _regCode = new Regex(@"window.code=(\d+);");
        private readonly Regex _regUrl = new Regex(@"window.redirect_uri=""(\S+?)"";");
        private int _tip = 1;

        public WatiForLoginAction(IWeChatContext context, ActionEventListener listener = null) 
            : base(context, listener)
        {
        }

        public override HttpRequestItem BuildRequest()
        {
            var req = new HttpRequestItem(HttpMethodType.Post, ApiUrls.CheckQRCode);
            req.AddQueryValue("tip", _tip);
            req.AddQueryValue("uuid", Session.Uuid);
            req.AddQueryValue("_", Timestamp);
            return req;
        }

        public override Task<ActionEvent> HandleResponse(HttpResponseItem responseItem)
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
                                return NotifyActionEventAsync(ActionEventType.EvtOK);
                            }
                            break;
                        }

                    case "201":
                        _tip = 0;
                        return NotifyActionEventAsync(ActionEventType.EvtRepeat);

                    case "408": return NotifyErrorEventAsync(WeChatErrorCode.Timeout);
                }
            }
            throw WeChatException.CreateException(WeChatErrorCode.ResponseError);
        }
    }
}
