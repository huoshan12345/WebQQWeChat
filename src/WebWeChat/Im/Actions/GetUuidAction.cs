using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HttpAction.Core;
using HttpAction.Event;
using HttpAction;
using WebWeChat.Im.Core;

namespace WebWeChat.Im.Actions
{
    /// <summary>
    /// 获取UUID
    /// </summary>
    [Description("获取UUID")]
    public class GetUuidAction : WebWeChatAction
    {
        private readonly Regex _reg = new Regex(@"window.QRLogin.code = (\d+); window.QRLogin.uuid = ""(\S+?)""");

        public GetUuidAction(IWeChatContext context, ActionEventListener listener = null) : base(context, listener)
        {
            Session.Seq = Timestamp;
        }

        protected override HttpReq BuildRequest()
        {
            var req = new HttpReq(HttpMethodType.Post, ApiUrls.GetUuid);
            req.AddQueryValue("appid", ApiUrls.Appid);
            req.AddQueryValue("fun", "new");
            req.AddQueryValue("lang", "zh_CN");
            req.AddQueryValue("_", Session.Seq++);
            return req;
        }

        protected override ValueTask<ActionEvent> HandleResponse(HttpRes responseItem)
        {
            var str = responseItem.ResponseString;
            var match = _reg.Match(str);
            if (match.Success && match.Groups.Count > 2 && match.Groups[1].Value == "200")
            {
                Session.Uuid = match.Groups[2].Value;
                return NotifyOkEventAsync();
            }
            return NotifyErrorEventAsync(WeChatErrorCode.ResponseError);
        }
    }
}
