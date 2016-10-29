using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HttpActionFrame.Core;
using HttpActionFrame.Event;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Utility.Extensions;
using WebWeChat.Im.Bean;
using WebWeChat.Im.Core;

namespace WebWeChat.Im.Action
{
    public class SyncCheckAction : WebWeChatAction
    {
        private readonly Regex _reg = new Regex(@"window.synccheck={retcode:""(\d+)"",selector:""(\d+)""}");
        private int _hostIndex = 0;

        public SyncCheckAction(ActionEventListener listener = null) : base(listener)
        {
        }

        public override HttpRequestItem BuildRequest()
        {
            var url = Session.SyncUrl;
            if (Session.SyncUrl == null)
            {
                var host = ApiUrls.SyncHosts[_hostIndex];
                url = $"https://{host}/cgi-bin/mmwebwx-bin/synccheck";
                Logger.LogInformation($"测试站点{_hostIndex + 1}: {host}");
            }
            var req = new HttpRequestItem(HttpMethodType.Get, url)
            {
                RawData = Session.BaseRequest.ToQueryString().ToLowerInvariant(),
                Referrer = "https://wx.qq.com/"
            };
            req.AddQueryValue("_", Timestamp);
            req.AddQueryValue("r", Timestamp);
            req.AddQueryValue("synckey", Session.SyncKeyStr);
            return req;
        }

        private async void TestNextHost()
        {
            if (++_hostIndex < ApiUrls.SyncHosts.Length)
            {
                await ExecuteAsync();
            }
            else
            {
                NotifyErrorEvent(WeChatErrorCode.IoError);
            }
        }

        public override void OnHttpContent(HttpResponseItem responseItem)
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
                    TestNextHost();
                    return;
                }

                switch (retcode)
                {
                    case "1100":
                        Session.State = SessionState.Offline; // 在手机上登出了微信
                        result = SyncCheckResult.Offline;
                        break;

                    case "1101":
                        Session.State = SessionState.Kicked; // 在其他地方登录了WEB版微信
                        result = SyncCheckResult.Kicked;
                        break;

                    case "1102":
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
                        ActionFuture.PushAction(this);
                        break;
                }
                NotifyActionEvent(ActionEventType.EvtOK, result);
            }
            else throw WeChatException.CreateException(WeChatErrorCode.ResponseError);
        }

        public override async void OnHttpError(Exception ex)
        {
            var exception = ex as WeChatException ?? new WeChatException(ex);
            // SyncUrl为空说明正在测试host 
            if (Session.SyncUrl == null)
            {
                if (++_retryTimes < MaxReTryTimes)
                {
                    NotifyActionEvent(new ActionEvent(ActionEventType.EvtRetry, exception));
                    await ExecuteAsync();
                }
                else
                {
                    _retryTimes = 0;
                    TestNextHost();
                }
            }
            else base.OnHttpError(exception);
        }
    }
}
