using System.Text.RegularExpressions;
using Utility.HttpAction.Core;
using Utility.HttpAction.Event;
using WebWeChat.Im.Core;

namespace WebWeChat.Im.Action
{
    /// <summary>
    /// 获取UUID
    /// </summary>
    public class GetUuidAction : WebWeChatAction
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

        public override ActionEvent HandleResponse(HttpResponseItem responseItem)
        {
            var str = responseItem.ResponseString;
            var match = _reg.Match(str);
            if (match.Success && match.Groups.Count > 2 && match.Groups[1].Value == "200")
            {
                Session.Uuid = match.Groups[2].Value;
                return NotifyActionEvent(ActionEventType.EvtOK);
            }
            return NotifyErrorEvent(WeChatErrorCode.ResponseError);
        }
    }
}
