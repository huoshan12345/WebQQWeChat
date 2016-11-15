using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HttpAction.Core;
using HttpAction.Event;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Bean;
using WebQQ.Im.Bean.Content;
using WebQQ.Im.Core;
using WebQQ.Im.Event;
using WebQQ.Im.Module.Impl;
using WebQQ.Util;

namespace WebQQ.Im.Action
{
    public class PollMsgAction : WebQQInfoAction
    {
        private static readonly ConcurrentDictionary<string, MethodInfo> MethodDic = new ConcurrentDictionary<string, MethodInfo>();

        public PollMsgAction(IQQContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        protected override void ModifyRequest(HttpRequestItem req)
        {
            var json = new JObject
            {
                {"clientid", Session.ClientId},
                {"psessionid", Session.SessionId},
                {"key", ""},
                {"ptwebqq", Session.Ptwebqq}
            };
            req.AddQueryValue("r", json.ToSimpleString());
            req.Referrer = ApiUrls.Referrer;
        }

        public override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            var json = response.ResponseString.ToJToken();
            var retcode = json["retcode"].ToInt();
            switch (retcode)
            {
                case 0: return HandlePollMsg(json["result"]);
            }
            throw new QQException(QQErrorCode.ResponseError, response.ResponseString);
        }

        public override Task<ActionEvent> HandleExceptionAsync(Exception ex)
        {
            if (Session.State == SessionState.Online && (ex as QQException)?.ErrorCode == QQErrorCode.Timeout)
            {
                return Task.FromResult(ActionEvent.EmptyRepeatEvent);
            }
            else
            {
                return base.HandleExceptionAsync(ex);
            }
        }

        private Task<ActionEvent> HandlePollMsg(JToken result)
        {
            var array = result as JArray;
            var notifyEvents = new List<QQNotifyEvent>();
            if (array != null)
            {
                foreach (var item in array)
                {
                    var type = item["poll_type"].ToObject<PollType>();
                    var value = item["value"];
                    switch (type)
                    {
                        case PollType.InputNotify:
                            break;

                        case PollType.Message:
                            notifyEvents.Add(QQNotifyEvent.CreateEvent(QQNotifyEventType.ChatMsg, HandleMessage(value)));
                            break;

                        case PollType.GroupMessage:
                            notifyEvents.Add(QQNotifyEvent.CreateEvent(QQNotifyEventType.GroupMsg, HandleGroupMessage(value)));
                            break;

                        case PollType.DiscussionMessage:
                            break;
                        case PollType.SessionMessage:
                            break;
                        case PollType.ShakeMessage:
                            break;
                        case PollType.KickMessage:
                            break;
                        case PollType.BuddiesStatusChange:
                            break;
                        case PollType.SystemMessage:
                            break;
                        case PollType.GroupWebMessage:
                            break;
                        case PollType.SysGroupMsg:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            return NotifyActionEventAsync(ActionEventType.EvtOK, notifyEvents);
        }

        private Message HandleMessage(JToken resultValue)
        {
            /*
                {
                    "result": [
                        {
                            "poll_type": "message",
                            "value": {
                                "content": [
                                    [
                                        "font",
                                        {
                                            "color": "000000",
                                            "name": "微软雅黑",
                                            "size": 10,
                                            "style": [
                                                0,
                                                0,
                                                0
                                            ]
                                        }
                                    ],
                                    [
                                        "face",
                                        107
                                    ],
                                    "xxx",
                                    [
                                        "face",
                                        107
                                    ]
                                ],
                                "from_uin": 3219658576,
                                "msg_id": 1019,
                                "msg_type": 0,
                                "time": 1479178662,
                                "to_uin": 89009143
                            }
                        }
                    ],
                    "retcode": 0
                }             
             */
            var msg = resultValue.ToObject<Message>();
            msg.Type = MessageType.Friend;
            msg.Contents = ContentFatory.ParseContents(resultValue["content"].ToJArray());
            return msg;
        }

        private Message HandleGroupMessage(JToken resultValue)
        {
            /*
                 {
                    "result": [
                        {
                            "poll_type": "group_message",
                            "value": {
                                "content": [
                                    [
                                        "font",
                                        {
                                            "color": "000000",
                                            "name": "微软雅黑",
                                            "size": 10,
                                            "style": [
                                                0,
                                                0,
                                                0
                                            ]
                                        }
                                    ],
                                    "。。。"
                                ],
                                "from_uin": 3258316418,
                                "group_code": 3258316418,
                                "msg_id": 18537,
                                "msg_type": 0,
                                "send_uin": 2515522700,
                                "time": 1479175440,
                                "to_uin": 89009143
                            }
                        }
                    ],
                    "retcode": 0
                }
             */
            var msg = resultValue.ToObject<Message>();
            msg.Type = MessageType.Group;
            msg.Contents = ContentFatory.ParseContents(resultValue["content"].ToJArray());
            return msg;
        }
    }
}
