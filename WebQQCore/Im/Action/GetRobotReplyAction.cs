using System.Text;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Action
{

    public class GetRobotReplyAction : AbstractHttpAction
    {
        private QQMsg input;
        private RobotType robotType;

        public GetRobotReplyAction(IQQContext context, QQActionEventHandler listener, QQMsg input, RobotType robotType)
            : base(context, listener)
        {
            this.input = input;
            this.robotType = robotType;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            QQHttpRequest req = null;
            switch (robotType)
            {
                case RobotType.Tuling:
                {
                    req = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_ROBOT_TULING);
                    req.AddGetValue("key", QQConstants.ROBOT_TULING_KEY);
                    req.AddGetValue("info", input.GetText());
                    // req.AddGetValue("userid", input.From.Uin.ToString());
                    break;
                }

                case RobotType.Moli:
                {
                    req = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_ROBOT_MOLI);
                    req.AddGetValue("limit", "5");
                    req.AddGetValue("question", input.GetText());
                    req.AddGetValue("api_key", QQConstants.ROBOT_MOLI_KEY);
                    req.AddGetValue("api_secret", QQConstants.ROBOT_MOLI_SECRET);
                    // req.AddGetValue("type", "json");
                    break;
                }
            }

            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            switch (robotType)
            {
                case RobotType.Tuling:
                {
                    var json = JObject.Parse(response.GetResponseString());
                    var code = json["code"].ToString();
                    switch (code)
                    {
                        case "40001":
                        case "40002":
                        case "40003":
                        case "40004":
                        case "40005":
                        case "40006":
                        case "40007":
                        NotifyActionEvent(QQActionEventType.EvtError,
                            new QQException(QQErrorCode.UNEXPECTED_RESPONSE, response.GetResponseString()));
                        break;

                        case "100000":
                        {
                            var text = json["text"].ToString();
                            NotifyActionEvent(QQActionEventType.EvtOK, text);
                            break;
                        }

                        case "200000":
                        {
                            var text = json["text"].ToString();
                            var url = json["url"].ToString();
                            var reply = string.Format("{0} \n网址：{1}", text, url);
                            NotifyActionEvent(QQActionEventType.EvtOK, reply);
                            break;
                        }

                        case "302000":
                        {
                            var text = json["text"].ToString();
                            var list = json["list"].ToObject<JArray>();
                            var reply = string.Format("{0}", text);
                            NotifyActionEvent(QQActionEventType.EvtOK, reply);
                            break;
                        }

                        case "305000":
                        {
                            var text = json["text"].ToString();
                            var list = json["list"].ToObject<JArray>();
                            var sb = new StringBuilder();
                            foreach (var t in list)
                            {
                                var item = t.ToObject<JObject>();
                                sb.AppendFormat("车次：{0},起始站：{1},到达站：{2}\n开车时间：{3},到达时间：{4}\n详情地址：{5}\n\n",
                                   item["trainnum"], item["start"], item["terminal"], item["starttime"], item["endtime"], item["detailurl"]);
                            }
                            var reply = string.Format("{0}\n{1}\n", text, sb);
                            NotifyActionEvent(QQActionEventType.EvtOK, reply);
                            break;
                        }

                        case "306000":
                        {
                            var text = json["text"].ToString();
                            var list = json["list"].ToObject<JArray>();
                            var sb = new StringBuilder();
                            foreach (var t in list)
                            {
                                var item = t.ToObject<JObject>();
                                sb.AppendFormat("航班：{0},航班路线：{1}\n起飞时间：{2},到达时间：{3},航班状态{4}\n详情地址：{5}\n\n",
                                   item["flight"], item["route"], item["starttime"], item["endtime"], item["state"], item["detailurl"]);
                            }
                            var reply = string.Format("{0}\n{1}\n", text, sb);
                            NotifyActionEvent(QQActionEventType.EvtOK, reply);
                            break;
                        }

                        case "309000":
                        {
                            var text = json["text"].ToString();
                            var list = json["list"].ToObject<JArray>();
                            var sb = new StringBuilder();
                            foreach (var t in list)
                            {
                                var item = t.ToObject<JObject>();
                                sb.AppendFormat("酒店名称：{0},价格：{1}\n满意度：{2},数量：{3}\n详情地址：{4}\n\n",
                                   item["name"], item["price"], item["satisfaction"], item["count"], item["detailurl"]);
                            }
                            var reply = string.Format("{0}\n{1}", text, sb);
                            NotifyActionEvent(QQActionEventType.EvtOK, reply);
                            break;
                        }

                        default:
                        {
                            var text = json["text"].ToString();
                            NotifyActionEvent(QQActionEventType.EvtOK, text);
                            break;
                        }
                    }
                    break;
                }

                case RobotType.Moli:
                {
                    var result = response.GetResponseString();
                    NotifyActionEvent(QQActionEventType.EvtOK, result);
                    break;
                }
            }

            
        }
    }
}
