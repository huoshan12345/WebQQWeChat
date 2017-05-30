using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FclEx.Extensions;
using HttpAction.Core;
using HttpAction.Event;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Bean;
using WebQQ.Im.Bean.Content;
using WebQQ.Im.Bean.Discussion;
using WebQQ.Im.Bean.Friend;
using WebQQ.Im.Bean.Group;
using WebQQ.Im.Core;
using WebQQ.Im.Event;
using WebQQ.Im.Module.Impl;
using WebQQ.Util;

namespace WebQQ.Im.Action
{
    /// <summary>
    /// 轮询，用来获取新消息
    /// </summary>
    public class PollMsgAction : WebQQInfoAction
    {
        private static readonly ConcurrentDictionary<string, MethodInfo> _methodDic = new ConcurrentDictionary<string, MethodInfo>();

        private static readonly QQNotifyEvent _pollEvent = QQNotifyEvent.CreateEvent(QQNotifyEventType.PollSuccess);

        public PollMsgAction(IQQContext context, ActionEventListener listener = null) : base(context, listener)
        {
        }

        protected override void ModifyRequest(HttpRequestItem req)
        {
            Logger.LogInformation("Begin poll...");

            req.Method = HttpMethodType.Post;
            var json = new JObject
            {
                {"clientid", Session.ClientId},
                {"psessionid", Session.SessionId},
                {"key", ""},
                {"ptwebqq", Session.Ptwebqq}
            };
            req.AddQueryValue("r", json.ToSimpleString());
            req.Referrer = "https://d1.web2.qq.com/cfproxy.html?v=20151105001&callback=1";
        }

        protected override Task<ActionEvent> HandleResponse(HttpResponseItem response)
        {
            var json = response.ResponseString.ToJToken();
            var retcode = json["retcode"].ToInt();
            switch (retcode)
            {
                case 0: return HandlePollMsg(json["result"]);

                case 102:
                    // 接连正常，没有消息到达 {"retcode":102,"errmsg":""}
                    // 继续进行下一个消息请求
                    return NotifyOkEventAsync();

                case 110:
                case 109:
                    {
                        // 客户端主动退出
                        var msg = $"**** 客户端主动退出 retcode: {retcode} ****";
                        Logger.LogWarning(msg);
                        Session.State = SessionState.Offline;
                        return NotifyOkEventAsync(QQNotifyEvent.CreateEvent(QQNotifyEventType.NetError, msg));
                    }


                case 121:
                case 120:
                case 100:
                    {
                        // 客户端主动退出
                        var msg = $"**** 服务器需求重新认证 retcode: {retcode} ****";
                        Logger.LogWarning(msg);
                        Session.State = SessionState.Offline;
                        return NotifyOkEventAsync(QQNotifyEvent.CreateEvent(QQNotifyEventType.NetError, msg));
                    }

                case 103: // 此时需要登录Smart QQ，确认能收到消息后点击设置-退出登录，就会恢复正常了
                    {
                        // 客户端主动退出
                        var msg = $"**** 需要登录Smart QQ retcode: {retcode} ****";
                        Logger.LogWarning(msg);
                        // Session.State = SessionState.Offline;
                        // return NotifyOkEventAsync(QQNotifyEvent.CreateEvent(QQNotifyEventType.NetError, msg));
                        return NotifyOkEventAsync();
                    }
            }
            throw new QQException(QQErrorCode.ResponseError, response.ResponseString);
        }

        protected override Task<ActionEvent> HandleExceptionAsync(Exception ex)
        {
            if (Session.State == SessionState.Online && ex is TaskCanceledException)
            {
                return Task.FromResult(ActionEvent.EmptyRepeatEvent);
            }
            else if (ex is QQException qqEx)
            {
                if (qqEx.ErrorCode == QQErrorCode.InvalidLoginAuth)
                {
                    RetryTimes = MaxReTryTimes;
                }
            }
            return base.HandleExceptionAsync(ex);
        }

        private Task<ActionEvent> HandlePollMsg(JToken result)
        {
            var notifyEvents = new List<QQNotifyEvent>();
            if (result is JArray array)
            {
                foreach (var item in array)
                {
                    var type = item["poll_type"].ToObject<PollType>();
                    var value = item["value"];
                    switch (type)
                    {
                        case PollType.Message:
                            HandleMessage(value, notifyEvents);
                            break;

                        case PollType.GroupMessage:
                            HandleGroupMessage(value, notifyEvents);
                            break;

                        case PollType.InputNotify:
                        case PollType.DiscussionMessage:
                        case PollType.SessionMessage:
                        case PollType.ShakeMessage:
                        case PollType.KickMessage:
                        case PollType.BuddiesStatusChange:
                        case PollType.SystemMessage:
                        case PollType.GroupWebMessage:
                        case PollType.SysGroupMsg:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            if (!notifyEvents.Any()) notifyEvents.Add(_pollEvent);

            return NotifyOkEventAsync(notifyEvents);
        }

        private void HandleMessage(JToken resultValue, List<QQNotifyEvent> events)
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
            var msg = resultValue.ToObject<FriendMessage>();
            msg.Friend = Store.GetOrAddFriendByUin(msg.FromUin, u =>
            {
                events.Add(QQNotifyEvent.CreateEvent(QQNotifyEventType.NeedUpdateFriends));
                return new QQFriend() { Uin = u };
            });
            msg.Contents = ContentFatory.ParseContents(resultValue["content"].ToJArray());

            events.Add(QQNotifyEvent.CreateEvent(QQNotifyEventType.ChatMsg, msg));
        }

        private void HandleGroupMessage(JToken resultValue, List<QQNotifyEvent> events)
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
            var needUpdateGroup = false;

            var msg = resultValue.ToObject<GroupMessage>();
            msg.Group = Store.GetOrAddGroupByGid(msg.GroupCode, u =>
            {
                needUpdateGroup = true;
                return new QQGroup { Gid = u };
            }); // 此处的GroupCode实际上是Group的gid
            msg.Contents = ContentFatory.ParseContents(resultValue["content"].ToJArray());
            msg.User = msg.Group.Members.GetOrAdd(msg.SendUin, m =>
            {
                // needUpdateGroup = true;
                return new GroupMember { Uin = msg.SendUin };
            });

            if(needUpdateGroup) events.Add(QQNotifyEvent.CreateEvent(QQNotifyEventType.NeedUpdateGroups, msg.Group));

            events.Add(QQNotifyEvent.CreateEvent(QQNotifyEventType.GroupMsg, msg));
        }
    }
}
