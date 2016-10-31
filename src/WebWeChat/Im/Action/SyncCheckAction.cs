using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Extensions.Logging;
using Utility.Extensions;
using Utility.HttpAction.Core;
using Utility.HttpAction.Event;
using WebWeChat.Im.Core;

namespace WebWeChat.Im.Action
{
    public class SyncCheckAction : WeChatAction
    {
        private readonly Regex _reg = new Regex(@"window.synccheck={retcode:""(\d+)"",selector:""(\d+)""}");
        private int _hostIndex = 0;

        public SyncCheckAction(IWeChatContext context, ActionEventListener listener = null)
            : base(context, listener)
        {
        }

        public override HttpRequestItem BuildRequest()
        {
            var url = Session.SyncUrl;
            if (Session.SyncUrl == null)
            {
                var host = ApiUrls.SyncHosts[_hostIndex];
                url = $"https://{host}/cgi-bin/mmwebwx-bin/synccheck";
                Logger.LogDebug($"测试站点{_hostIndex + 1}: {host}");
            }
            else
            {
                Logger.LogInformation("Begin SyncCheck...");
            }
            var req = new HttpRequestItem(HttpMethodType.Get, url)
            {
                // 此处需要将key都变成小写，否则提交会失败
                RawData = Session.BaseRequest.ToDictionary(pair => pair.Key.ToLower(), pair => pair.Value).ToQueryString(),
            };
            req.AddQueryValue("r", Timestamp);
            req.AddQueryValue("synckey", Session.SyncKeyStr);
            req.AddQueryValue("_", Timestamp);

            return req;
        }

        private ActionEvent TestNextHost()
        {
            if (++_hostIndex < ApiUrls.SyncHosts.Length)
            {
                return ActionEvent.EmptyRepeatEvent;
            }
            else
            {
                return NotifyErrorEvent(WeChatErrorCode.IoError);
            }
        }

        public override ActionEvent HandleResponse(HttpResponseItem responseItem)
        {
            var str = responseItem.ResponseString;
            var match = _reg.Match(str);
            if (match.Success)
            {
                var retcode = match.Groups[1].Value;
                var selector = match.Groups[2].Value;
                var result = SyncCheckResult.Nothing;

                if (Session.SyncUrl == null && retcode != "0")
                {
                    return TestNextHost();
                }

                switch (retcode)
                {
                    case "1100":
                        Session.State = SessionState.Offline; // 在手机上登出了微信
                        result = SyncCheckResult.Offline;
                        break;

                    case "1101": // 参数错误
                        Session.State = SessionState.Kicked; // 在其他地方登录了WEB版微信
                        result = SyncCheckResult.Kicked;
                        break;

                    case "1102": // cookie错误
                        break;

                    case "0":
                        Session.SyncUrl = responseItem.RequestItem.RawUrl;
                        switch (selector)
                        {
                            case "2": // 新消息
                                result = SyncCheckResult.NewMsg;
                                break;

                            case "6": // 红包
                                result = SyncCheckResult.RedEnvelope;
                                break;

                            case "7": // 在手机上玩微信
                                result = SyncCheckResult.UsingPhone;
                                break;

                            case "0": // 什么都没有
                            default:
                                break;
                        }
                        break;
                }
                return NotifyActionEvent(ActionEventType.EvtOK, result);
            }
            else throw WeChatException.CreateException(WeChatErrorCode.ResponseError);
        }

        public override ActionEvent HandleException(Exception ex)
        {
            // SyncUrl为空说明正在测试host
            if (Session.SyncUrl == null)
            {
                if (++RetryTimes < MaxReTryTimes)
                {
                    return NotifyActionEvent(ActionEvent.CreateEvent(ActionEventType.EvtRetry, ex));
                }
                else
                {
                    RetryTimes = 0;
                    return TestNextHost();
                }
            }
            else return base.HandleException(ex);
        }
    }
}
