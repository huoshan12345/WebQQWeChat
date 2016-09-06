using System.Text;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Action
{

    public class GetTuringRobotReplyAction : AbstractHttpAction
    {
        private readonly QQMsg _input;

        public GetTuringRobotReplyAction(IQQContext context, QQActionListener listener, QQMsg input)
            : base(context, listener)
        {
            _input = input;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            var req = CreateHttpRequest(HttpConstants.Post, QQConstants.URL_ROBOT_TULING);
            req.AddPostValue("key", QQConstants.ROBOT_TULING_KEY);
            req.AddPostValue("info", _input.GetText());
            req.AddPostValue("userid", _input.From.QQ.ToString());
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var str = response.ResponseString;
            var json = JObject.Parse(str);
            var code = json["code"].ToString();

            switch (code)
            {
                // 文本类
                case "100000":
                {
                    var text = json["text"].ToString();
                    NotifyActionEvent(QQActionEventType.EvtOK, text);
                    break;
                }
                // 链接类，包括列车、航班
                case "200000":
                {
                    var text = json["text"].ToString();
                    var url = json["url"].ToString();
                    var reply = $"{text} \n网址：{url}";
                    NotifyActionEvent(QQActionEventType.EvtOK, reply);
                    break;
                }
                // 新闻类
                case "302000":
                {
                    var text = json["text"].ToString();
                    var list = json["list"].ToObject<JArray>();
                    var reply = new StringBuilder(text);
                    foreach (var item in list)
                    {
                        var article = item["article"].ToString();
                        var source = item["source"].ToString();
                        var icon = item["icon"].ToString();
                        var detailurl = item["detailurl"].ToString();
                        reply.AppendLine(article);
                    }
                    NotifyActionEvent(QQActionEventType.EvtOK, reply.ToString());
                    break;
                }
                // 菜谱类
                case "308000":
                {
                    var text = json["text"].ToString();
                    var list = json["list"].ToObject<JArray>();
                    var reply = new StringBuilder(text);
                    foreach (var item in list)
                    {
                        var name = item["name"].ToString();
                        var icon = item["icon"].ToString();
                        var info = item["info"].ToString();
                        var detailurl = item["detailurl"].ToString();
                        reply.AppendLine($"{name}：info");
                    }
                    NotifyActionEvent(QQActionEventType.EvtOK, reply);
                    break;
                }
                // 异常码
                case "40001": throw new QQException(QQErrorCode.UnexpectedResponse, $"[参数key错误]:{str}");
                case "40002": throw new QQException(QQErrorCode.UnexpectedResponse, $"[请求内容info为空]:{str}");
                case "40004": throw new QQException(QQErrorCode.UnexpectedResponse, $"[当天请求次数已使用完]:{str}");
                case "40007": throw new QQException(QQErrorCode.UnexpectedResponse, $"[数据格式异常]:{str}");

                default:
                {
                    var text = json["text"].ToString();
                    NotifyActionEvent(QQActionEventType.EvtOK, text);
                    break;
                }
            }
        }
    }
}
