using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Utility.Extensions;
using Utility.HttpAction.Core;
using Utility.HttpAction.Event;
using WebWeChat.Im.Action.ActionResult;
using WebWeChat.Im.Core;
using Utility.HttpAction;
using WebWeChat.Im.Module.Impl;

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

        private Task<ActionEvent> TestNextHost()
        {
            if (++_hostIndex < ApiUrls.SyncHosts.Length)
            {
                return Task.FromResult(ActionEvent.EmptyRepeatEvent);
            }
            else
            {
                return NotifyErrorEventAsync(WeChatErrorCode.IoError);
            }
        }

        public override Task<ActionEvent> HandleResponse(HttpResponseItem responseItem)
        {
            var str = responseItem.ResponseString;
            var match = _reg.Match(str);
            if (match.Success)
            {
                var retcode = match.Groups[1].Value;

                if (Session.SyncUrl == null)
                {
                    // retcode
                    // 1100-
                    // 1101-参数错误
                    // 1102-cookie错误
                    if (retcode != "0") return TestNextHost();
                    else
                    {
                        Session.SyncUrl = responseItem.RequestItem.RawUrl;
                        return this.ExecuteAsync();
                    }

                }

                SyncCheckResult result;
                switch (retcode)
                {
                    case "1100":
                    case "1101": // 在手机上登出了微信
                        Session.State = SessionState.Offline;
                        result = (SyncCheckResult)int.Parse(retcode);
                        break;

                    case "0":
                        var selector = match.Groups[2].Value;
                        result = (SyncCheckResult)int.Parse(selector);
                        break;

                    default:
                        throw WeChatException.CreateException(WeChatErrorCode.ResponseError);
                }
                return NotifyActionEventAsync(ActionEventType.EvtOK, result);
            }
            else throw WeChatException.CreateException(WeChatErrorCode.ResponseError);
        }

        public override Task<ActionEvent> HandleExceptionAsync(Exception ex)
        {
            // SyncUrl为空说明正在测试host
            if (Session.SyncUrl == null)
            {
                if (++RetryTimes < MaxReTryTimes)
                {
                    return NotifyActionEventAsync(ActionEvent.CreateEvent(ActionEventType.EvtRetry, ex));
                }
                else
                {
                    RetryTimes = 0;
                    return TestNextHost();
                }
            }
            else return base.HandleExceptionAsync(ex);
        }
    }
}
