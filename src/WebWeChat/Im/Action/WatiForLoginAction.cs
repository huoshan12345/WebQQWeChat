using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FclEx.Helpers;
using HttpAction.Core;
using HttpAction.Event;
using WebWeChat.Im.Action.ActionResult;
using WebWeChat.Im.Core;

namespace WebWeChat.Im.Action
{
    /// <summary>
    /// 等待扫码登录
    /// </summary>
    [Description("等待扫码登录")]
    public class WatiForLoginAction : WebWeChatAction
    {
        private readonly Regex _regCode = new Regex(@"window.code=(\d+);");
        private readonly Regex _regUrl = new Regex(@"window.redirect_uri=""(\S+?)"";");
        private int _tip = 1;

        public WatiForLoginAction(IWeChatContext context, ActionEventListener listener = null)
            : base(context, listener)
        {
        }

        protected override HttpRequestItem BuildRequest()
        {
            var req = new HttpRequestItem(HttpMethodType.Post, ApiUrls.CheckQRCode);
            req.AddQueryValue("loginicon", "true");
            req.AddQueryValue("tip", _tip);
            req.AddQueryValue("uuid", Session.Uuid);
            req.AddQueryValue("r", ~Timestamp);
            req.AddQueryValue("_", Session.Seq++);
            return req;
        }

        protected override Task<ActionEvent> HandleResponse(HttpResponseItem responseItem)
        {
            var str = responseItem.ResponseString;
            var match = _regCode.Match(str);
            if (match.Success)
            {
                var code = match.Groups[1].Value;
                var result = EnumHelper.ParseFromStrNum<WatiForLoginResult>(code);
                switch (result)
                {
                    case WatiForLoginResult.Success:
                        {
                            var m = _regUrl.Match(str);
                            if (m.Success)
                            {
                                Session.LoginUrl = $"{m.Groups[1].Value}&fun=new&version=v2";
                                Session.BaseUrl = Session.LoginUrl.Substring(0, Session.LoginUrl.LastIndexOf("/", StringComparison.OrdinalIgnoreCase));
                            }
                            break;
                        }

                    case WatiForLoginResult.ScanCode:
                        _tip = 0;
                        break;

                    case WatiForLoginResult.QRCodeInvalid:
                        break;
                }
                return NotifyActionEventAsync(ActionEventType.EvtOK, result);
            }
            throw WeChatException.CreateException(WeChatErrorCode.ResponseError);
        }
    }
}
