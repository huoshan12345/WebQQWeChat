using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Utility.HttpAction.Core;
using Utility.HttpAction.Event;
using WebWeChat.Im.Core;
using WebWeChat.Im.Util;

namespace WebWeChat.Im.Action
{
    public class GetTuringRobotReplyAction : WeChatAction
    {
        private readonly string _input;

        public GetTuringRobotReplyAction(IWeChatContext context, string input, ActionEventListener listener = null)
            : base(context, listener)
        {
            _input = input;
        }

        public override HttpRequestItem BuildRequest()
        {
            var req = new HttpRequestItem(HttpMethodType.Post, ApiUrls.TulingRobot);
            req.AddQueryValue("key", Config["TulingApiKey"]);
            req.AddQueryValue("info", _input);
            req.AddQueryValue("userid", Account.User.UserName);
            return req;
        }

        public override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            var str = response.ResponseString;
            var json = str.ToJsonObj();
            var code = json["code"].ToString();
            var reply = "";

            switch (code)
            {
                // 文本类
                case "100000":
                    reply = json["text"].ToString();
                    break;

                // 链接类，包括列车、航班
                case "200000":
                    {
                        var text = json["text"].ToString();
                        var url = json["url"].ToString();
                        reply = $"{text} \n网址：{url}";
                        break;
                    }

                // 新闻类
                case "302000":
                    {
                        var text = json["text"].ToString();
                        var list = json["list"].ToObject<JArray>();
                        var sb = new StringBuilder(text);
                        foreach (var item in list)
                        {
                            var article = item["article"].ToString();
                            var source = item["source"].ToString();
                            var icon = item["icon"].ToString();
                            var detailurl = item["detailurl"].ToString();
                            sb.AppendLine(article);
                        }
                        reply = sb.ToString();
                        break;
                    }

                // 菜谱类
                case "308000":
                    {
                        var text = json["text"].ToString();
                        var list = json["list"].ToObject<JArray>();
                        var sb = new StringBuilder(text);
                        foreach (var item in list)
                        {
                            var name = item["name"].ToString();
                            var icon = item["icon"].ToString();
                            var info = item["info"].ToString();
                            var detailurl = item["detailurl"].ToString();
                            sb.AppendLine($"{name}：info");
                        }
                        reply = sb.ToString();
                        break;
                    }

                // 异常码
                case "40001": throw WeChatException.CreateException(WeChatErrorCode.ResponseError, $"[参数key错误]:{str}");
                case "40002": throw WeChatException.CreateException(WeChatErrorCode.ResponseError, $"[请求内容info为空]:{str}");
                case "40004": throw WeChatException.CreateException(WeChatErrorCode.ResponseError, $"[当天请求次数已使用完]:{str}");
                case "40007": throw WeChatException.CreateException(WeChatErrorCode.ResponseError, $"[数据格式异常]:{str}");

                default:
                    reply = json["text"].ToString();
                    break;
            }
            return NotifyOkActionEventAsync(reply);
        }
    }
}
