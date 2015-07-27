using System;
using System.Collections.Generic;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Im.Module;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Action
{

    /// <summary>
    /// <para>轮询Poll消息</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class PollMsgAction : AbstractHttpAction
    {
        public PollMsgAction(QQContext context, QQActionEventHandler listener) : base(context, listener) { }

        public override QQHttpRequest OnBuildRequest()
        {
            QQSession session = Context.Session;
            JObject json = new JObject();
            json.Add("clientid", session.ClientId);
            json.Add("psessionid", session.SessionId);
            json.Add("key", 0); // 暂时不知道什么用的
            json.Add("ids", new JArray()); // 同上

            QQHttpRequest req = CreateHttpRequest("POST", QQConstants.URL_POLL_MSG);
            req.AddPostValue("r", JsonConvert.SerializeObject(json));
            req.AddPostValue("clientid", session.ClientId + "");
            req.AddPostValue("psessionid", session.SessionId);
            req.ReadTimeout = 60 * 1000;
            req.ConnectTimeout = 60 * 1000;
            req.AddHeader("Referer", QQConstants.REFFER);

            return req;
        }

        public override void OnHttpFinish(QQHttpResponse response)
        {
            //如果返回的内容为空，认为这次PollMsg仍然成功
            if (response.GetContentLength() == 0)
            {
                // LOG.debug("PollMsgAction: empty response!!!!");
                NotifyActionEvent(QQActionEventType.EVT_OK, new List<QQNotifyEvent>());
            }
            else
            {
                base.OnHttpFinish(response);
            }
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            QQStore store = Context.Store;
            List<QQNotifyEvent> notifyEvents = new List<QQNotifyEvent>();
            JObject json = JObject.Parse(response.GetResponseString());
            int retcode = json["retcode"].ToObject<int>();
            if (retcode == 0)
            {
                //有可能为  {"retcode":0,"result":"ok"}
                if (json["result"] is JArray)
                {
                    JArray results = json["result"].ToObject<JArray>();
                    // 消息下载来的列表中是倒过来的，那我直接倒过来取，编位回来
                    for (int i = results.Count - 1; i >= 0; i--)
                    {
                        JObject poll = results[i].ToObject<JObject>();
                        string pollType = poll["poll_type"].ToString();
                        JObject pollData = poll["value"].ToObject<JObject>();

                        switch (pollType)
                        {
                            case "input_notify":
                            {
                                long fromUin = pollData["from_uin"].ToObject<long>();
                                QQBuddy qqBuddy = store.GetBuddyByUin(fromUin);
                                notifyEvents.Add(new QQNotifyEvent(QQNotifyEventType.BUDDY_INPUT, qqBuddy));
                                break;
                            }
                            case "message":
                            {
                                // 好友消息
                                notifyEvents.Add(ProcessBuddyMsg(pollData));
                                break;
                            }
                            case "group_message":
                            {
                                // 群消息
                                notifyEvents.Add(ProcessGroupMsg(pollData));
                                break;
                            }
                            case "discu_message":
                            {
                                // 讨论组消息
                                notifyEvents.Add(ProcessDiscuzMsg(pollData));
                                break;
                            }
                            case "sess_message":
                            {
                                // 临时会话消息
                                notifyEvents.Add(ProcessSessionMsg(pollData));
                                break;
                            }
                            case "shake_message":
                            {
                                // 窗口震动
                                long fromUin = pollData["from_uin"].ToObject<long>();
                                QQBuddy user = Context.Store.GetBuddyByUin(fromUin);
                                notifyEvents.Add(new QQNotifyEvent(QQNotifyEventType.SHAKE_WINDOW, user));
                                break;
                            }
                            case "kick_message":
                            {
                                // 被踢下线
                                Context.Account.Status = QQStatus.OFFLINE;
                                Context.Session.State = QQSessionState.KICKED;
                                notifyEvents.Add(new QQNotifyEvent(QQNotifyEventType.KICK_OFFLINE,
                                    pollData["reason"].ToString()));
                                break;
                            }
                            case "buddies_status_change":
                            {
                                // 群消息
                                notifyEvents.Add(ProcessBuddyStatusChange(pollData));
                                break;
                            }
                            case "system_message"://好友添加
                            {
                                QQNotifyEvent processSystemMessage = ProcessSystemMsg(pollData);
                                if (processSystemMessage != null)
                                {
                                    notifyEvents.Add(processSystemMessage);
                                }
                                break;
                            }
                            case "group_web_message"://发布了共享文件
                            {
                                QQNotifyEvent processSystemMessage = ProcessGroupWebMsg(pollData);
                                if (processSystemMessage != null)
                                {
                                    notifyEvents.Add(processSystemMessage);

                                }
                                break;
                            }
                            case "sys_g_msg"://被踢出了群
                            {
                                QQNotifyEvent processSystemMessage = ProcessSystemGroupMsg(pollData);
                                if (processSystemMessage != null)
                                {
                                    notifyEvents.Add(processSystemMessage);
                                }
                                break;
                            }
                            default:
                            {
                                QQException ex = new QQException(QQErrorCode.UNKNOWN_ERROR, "unknown pollType: " + pollType);
                                NotifyActionEvent(QQActionEventType.EVT_ERROR, ex);
                                break;
                            }
                        }
                    }
                }
                // end recode == 0
            }
            else if (retcode == 102)
            {
                // 接连正常，没有消息到达 {"retcode":102,"errmsg":""}
                // 继续进行下一个消息请求

            }
            else if (retcode == 110 || retcode == 109)
            { // 客户端主动退出
                Context.Session.State = QQSessionState.OFFLINE;
            }
            else if (retcode == 116)
            {
                // 需要更新Ptwebqq值，暂时不知道干嘛用的
                // {"retcode":116,"p":"2c0d8375e6c09f2af3ce60c6e081bdf4db271a14d0d85060"}
                // if (a.retcode === 116) alloy.portal.Ptwebqq = a.p)
                Context.Session.Ptwebqq = json["p"].ToString();
            }
            else if (retcode == 121 || retcode == 120 || retcode == 100)
            {	// 121,120 : ReLinkFailure		100 : NotRelogin
                // 服务器需求重新认证
                // {"retcode":121,"t":"0"}
                /*			LOG.info("**** NEED_REAUTH retcode: " + retcode + " ****");*/
                Context.Session.State = QQSessionState.OFFLINE;
                QQException ex = new QQException(QQErrorCode.INVALID_LOGIN_AUTH);
                NotifyActionEvent(QQActionEventType.EVT_ERROR, ex);
                return;
                //notifyEvents.Add(new QQNotifyEvent(QQNotifyEventType.NEED_REAUTH, null));
            }
            else
            {
                // 返回错误，核心遇到未知recode
                // Context.Session.State = QQSessionState.ERROR);
                notifyEvents.Add(new QQNotifyEvent(QQNotifyEventType.UNKNOWN_ERROR, json));
            }
            NotifyActionEvent(QQActionEventType.EVT_OK, notifyEvents);
        }

        /// <summary>
        /// 处理好友状态变化
        /// </summary>
        /// <param name="pollData"></param>
        /// <returns></returns>
        public QQNotifyEvent ProcessBuddyStatusChange(JObject pollData)
        {
            try
            {
                long uin = pollData["uin"].ToObject<long>();
                QQBuddy buddy = Context.Store.GetBuddyByUin(uin);
                if (buddy == null)
                {
                    return new QQNotifyEvent(QQNotifyEventType.BUDDY_STATUS_CHANGE, null);
                }
                string status = pollData["status"].ToString();
                int clientType = pollData["client_type"].ToObject<int>();
                buddy.Status = QQStatus.ValueOfRaw(status);
                buddy.ClientType = QQClientType.ValueOfRaw(clientType);
                return new QQNotifyEvent(QQNotifyEventType.BUDDY_STATUS_CHANGE, buddy);
            }
            catch (Exception ex)
            {
                return new QQNotifyEvent(QQNotifyEventType.NET_ERROR, ex);
            }
        }

        /// <summary>
        /// 处理好友信息
        /// </summary>
        /// <param name="pollData"></param>
        /// <returns></returns>
        public QQNotifyEvent ProcessBuddyMsg(JObject pollData)
        {
            try
            {
                QQStore store = Context.Store;
                long fromUin = pollData["from_uin"].ToObject<long>();
                QQMsg msg = new QQMsg();
                msg.Id = pollData["msg_id"].ToObject<long>();
                msg.Id2 = pollData["msg_id2"].ToObject<long>();
                msg.ParseContentList(JsonConvert.SerializeObject(pollData["content"]));
                msg.Type = QQMsgType.BUDDY_MSG;
                msg.To = Context.Account;
                msg.From = store.GetBuddyByUin(fromUin);
                long ticks = pollData["time"].ToObject<long>() * 1000;
                msg.Date = ticks > DateTime.MaxValue.Ticks ? DateTime.Now : new DateTime(ticks);
                if (msg.From == null)
                {
                    QQUser member = store.GetStrangerByUin(fromUin); // 搜索陌生人列表
                    if (member == null)
                    {
                        member = new QQHalfStranger();
                        member.Uin = fromUin;
                        store.AddStranger((QQStranger)member);

                        // 获取用户信息
                        UserModule userModule = Context.GetModule<UserModule>(QQModuleType.USER);
                        userModule.GetUserInfo(member, null);
                    }
                    msg.From = member;
                }
                return new QQNotifyEvent(QQNotifyEventType.CHAT_MSG, msg);
            }
            catch (Exception ex)
            {
                return new QQNotifyEvent(QQNotifyEventType.NET_ERROR, ex);
            }
        }

        /// <summary>
        /// 处理群消息
        /// </summary>
        /// <param name="pollData"></param>
        /// <returns></returns>
        public QQNotifyEvent ProcessGroupMsg(JObject pollData)
        {
            // {"retcode":0,"result":[{"poll_type":"group_message",
            // "value":{"msg_id":6175,"from_uin":3924684389,"to_uin":1070772010,"msg_id2":992858,"msg_type":43,"reply_ip":176621921,
            // "group_code":3439321257,"send_uin":1843694270,"seq":875,"time":1365934781,"info_seq":170125666,"content":[["font",{"size":10,"color":"3b3b3b","style":[0,0,0],"name":"\u5FAE\u8F6F\u96C5\u9ED1"}],"eeeeeeeee "]}}]}

            try
            {
                QQStore store = Context.Store;
                QQMsg msg = new QQMsg();
                msg.Id = pollData["msg_id"].ToObject<long>();
                msg.Id2 = pollData["msg_id2"].ToObject<long>();
                long fromUin = pollData["send_uin"].ToObject<long>();
                long groupCode = pollData["group_code"].ToObject<long>();
                long groupID = pollData["info_seq"].ToObject<long>(); // 真实群号码
                QQGroup group = store.GetGroupByCode(groupCode);
                if (group == null)
                {
                    GroupModule groupModule = Context.GetModule<GroupModule>(QQModuleType.GROUP);
                    group = new QQGroup();
                    group.Code = groupCode;
                    group.Gid = groupID;
                    // put to store
                    store.AddGroup(group);
                    groupModule.GetGroupInfo(group, null);
                }
                if (group.Gid <= 0)
                {
                    group.Gid = groupID;
                }

                msg.ParseContentList(JsonConvert.SerializeObject(pollData["content"]));
                msg.Type = QQMsgType.GROUP_MSG;
                msg.To = Context.Account;
                long ticks = pollData["time"].ToObject<long>() * 1000;
                msg.Date = ticks > DateTime.MaxValue.Ticks ? DateTime.Now : new DateTime(ticks);
                msg.Group = group;
                msg.From = group.GetMemberByUin(fromUin);

                if (msg.From == null)
                {
                    QQGroupMember member = new QQGroupMember();
                    member.Uin = fromUin;
                    msg.From = member;
                    group.Members.Add(member);
                    // 获取用户信息
                    UserModule userModule = Context.GetModule<UserModule>(QQModuleType.USER);
                    userModule.GetUserInfo(member, null);
                }
                return new QQNotifyEvent(QQNotifyEventType.CHAT_MSG, msg);
            }
            catch (Exception ex)
            {
                return new QQNotifyEvent(QQNotifyEventType.NET_ERROR, ex);
            }
        }

        /// <summary>
        /// 处理讨论组信息
        /// </summary>
        /// <param name="pollData"></param>
        /// <returns></returns>
        public QQNotifyEvent ProcessDiscuzMsg(JObject pollData)
        {
            try
            {
                QQStore store = Context.Store;

                QQMsg msg = new QQMsg();
                long fromUin = pollData["send_uin"].ToObject<long>();
                long did = pollData["did"].ToObject<long>();

                msg.ParseContentList(JsonConvert.SerializeObject(pollData["content"]));
                msg.Type = QQMsgType.DISCUZ_MSG;
                msg.Discuz = store.GetDiscuzByDid(did);
                msg.To = Context.Account;
                long ticks = pollData["time"].ToObject<long>() * 1000;
                msg.Date = ticks > DateTime.MaxValue.Ticks ? DateTime.Now : new DateTime(ticks);

                if (msg.Discuz == null)
                {
                    QQDiscuz discuz = new QQDiscuz();
                    discuz.Did = did;
                    store.AddDiscuz(discuz);
                    msg.Discuz = store.GetDiscuzByDid(did);

                    DiscuzModule discuzModule = Context.GetModule<DiscuzModule>(QQModuleType.DISCUZ);
                    discuzModule.GetDiscuzInfo(discuz, null);
                }
                msg.From = msg.Discuz.GetMemberByUin(fromUin);

                if (msg.From == null)
                {
                    QQDiscuzMember member = new QQDiscuzMember();
                    member.Uin = fromUin;
                    msg.From = member;
                    msg.Discuz.Members.Add(member);
                    // 获取用户信息
                    UserModule userModule = Context.GetModule<UserModule>(QQModuleType.USER);
                    userModule.GetUserInfo(member, null);
                }
                return new QQNotifyEvent(QQNotifyEventType.CHAT_MSG, msg);
            }
            catch (Exception ex)
            {
                return new QQNotifyEvent(QQNotifyEventType.NET_ERROR, ex);
            }
        }

        /// <summary>
        /// 处理临时会话消息
        /// </summary>
        /// <param name="pollData"></param>
        /// <returns></returns>
        public QQNotifyEvent ProcessSessionMsg(JObject pollData)
        {
            // {"retcode":0,"result":[{"poll_type":"sess_message",
            // "value":{"msg_id":25144,"from_uin":167017143,"to_uin":1070772010,"msg_id2":139233,"msg_type":140,"reply_ip":176752037,"time":1365931836,"id":2581801127,"ruin":444674479,"service_type":1,
            // "flags":{"text":1,"pic":1,"file":1,"audio":1,"video":1},"content":[["font",{"size":9,"color":"000000","style":[0,0,0],"name":"Tahoma"}],"2\u8F7D3 ",["face",1]," "]}}]}
            try
            {
                QQStore store = Context.Store;
                QQMsg msg = new QQMsg();
                long fromUin = pollData["from_uin"].ToObject<long>();
                long fromQQ = pollData["ruin"].ToObject<long>();// 真实QQ
                int serviceType = pollData["service_type"].ToObject<int>(); // Group:0,Discuss:1
                long typeId = pollData["id"].ToObject<long>(); // Group ID or Discuss ID

                msg.ParseContentList(JsonConvert.SerializeObject(pollData["content"]));
                msg.Type = QQMsgType.SESSION_MSG;
                msg.To = Context.Account;
                long ticks = pollData["time"].ToObject<long>() * 1000;
                msg.Date = ticks > DateTime.MaxValue.Ticks ? DateTime.Now : new DateTime(ticks);

                QQUser user = store.GetBuddyByUin(fromUin); // 首先看看是不是自己的好友
                if (user != null)
                {
                    msg.Type = QQMsgType.BUDDY_MSG; // 是自己的好友
                }
                else
                {
                    if (serviceType == 0)
                    { // 是群成员
                        QQGroup group = store.GetGroupByCode(typeId);
                        if (group == null)
                        {
                            group = new QQGroup();
                            group.Code = typeId;
                            // 获取群信息
                            GroupModule groupModule = Context.GetModule<GroupModule>(QQModuleType.GROUP);
                            groupModule.GetGroupInfo(group, null);
                        }
                        foreach (QQUser u in group.Members)
                        {
                            if (u.Uin == fromUin)
                            {
                                user = u;
                                break;
                            }
                        }
                    }
                    else if (serviceType == 1)
                    { // 是讨论组成员
                        QQDiscuz discuz = store.GetDiscuzByDid(typeId);
                        if (discuz == null)
                        {
                            discuz = new QQDiscuz();
                            discuz.Did = typeId;

                            // 获取讨论组信息
                            DiscuzModule discuzModule = Context.GetModule<DiscuzModule>(QQModuleType.DISCUZ);
                            discuzModule.GetDiscuzInfo(discuz, null);
                        }


                        foreach (QQUser u in discuz.Members)
                        {
                            if (u.Uin == fromUin)
                            {
                                user = u;
                                break;
                            }
                        }
                    }
                    else
                    {
                        user = store.GetStrangerByUin(fromUin); // 看看陌生人列表中有木有
                    }
                    if (user == null)
                    { // 还没有就新建一个陌生人，原理来说不应该这样。后面我就不知道怎么回复这消息了，但是消息是不能丢失的
                        user = new QQStranger();
                        user.QQ = pollData["ruin"].ToObject<long>();
                        user.Uin = fromUin;
                        user.Nickname = pollData["ruin"] + "";
                        store.AddStranger((QQStranger)user);

                        // 获取用户信息
                        UserModule userModule = Context.GetModule<UserModule>(QQModuleType.USER);
                        userModule.GetStrangerInfo(user, null);
                    }
                }
                user.QQ = fromQQ; // 带上QQ号码
                msg.From = user;
                return new QQNotifyEvent(QQNotifyEventType.CHAT_MSG, msg);
            }
            catch (Exception ex)
            {
                return new QQNotifyEvent(QQNotifyEventType.NET_ERROR, ex);
            }
        }


        public QQNotifyEvent ProcessSystemMsg(JObject pollData)
        {
            string type = pollData["type"].ToString().ToLower();
            if (type == "verify_required")
            {	//好友请求
                JObject target = new JObject();
                target.Add("type", "verify_required");	//通知类型（好友请求）
                target.Add("from_uin", pollData["from_uin"].ToString());	//哪个人请求
                target.Add("msg", pollData["msg"].ToString());	//请求添加好友原因
                return new QQNotifyEvent(QQNotifyEventType.BUDDY_NOTIFY, target.ToString());
            }
            return null;
        }

        /// <summary>
        /// 处理其他人上传文件的通知
        /// </summary>
        /// <param name="pollData"></param>
        /// <returns></returns>
        public QQNotifyEvent ProcessGroupWebMsg(JObject pollData)
        {
            //{"retcode":0,"result":[{"poll_type":"group_web_message","value":{"msg_id":25082,"from_uin":802292893,"to_uin":3087958343,"msg_id2":343597,"msg_type":45,"reply_ip":176756769,"group_code":898704454,"group_type":1,"ver":1,"send_uin":3014857601,"xml":"\u003c?xml version=\"1.0\" encoding=\"utf-8\"?\u003e\u003cd\u003e\u003cn t=\"h\" u=\"2519967390\"/\u003e\u003cn t=\"t\" s=\"\u5171\u4EAB\u6587\u4EF6\"/\u003e\u003cn t=\"b\"/\u003e\u003cn t=\"t\" s=\"IMG_1193.jpg\"/\u003e\u003c/d\u003e"}}]}
            JObject target = new JObject();
            target.Add("type", "share_file");	//通知类型（共享群文件消息）
            target.Add("file", pollData["xml"].ToString());	//共享的文件信息
            target.Add("sender", pollData["send_uin"].ToObject<long>());	//共享者
            return new QQNotifyEvent(QQNotifyEventType.GROUP_NOTIFY, target.ToString());
        }


        public QQNotifyEvent ProcessSystemGroupMsg(JObject pollData)
        {
            //{"retcode":0,"result":[{"poll_type":"sys_g_msg","value":{"msg_id":39855,"from_uin":802292893,"to_uin":3087958343,"msg_id2":518208,"msg_type":34,"reply_ip":176757073,"type":"group_leave","gcode":898704454,"t_gcode":310070477,"op_type":3,"old_member":3087958343,"t_old_member":"","admin_uin":1089498579,"t_admin_uin":"","admin_nick":"\u521B\u5EFA\u8005"}}]}
            string type = pollData["type"].ToString().ToLower();
            if (type == "group_leave")
            {
                JObject target = new JObject();
                target.Add("type", "group_leave");	//通知类型（离群消息）
                target.Add("group_code", pollData["gcode"].ToObject<long>());	//从那个群（群临时编号）
                target.Add("group_num", pollData["t_gcode"].ToObject<long>());	//从那个群（群号）
                target.Add("admin_uin", pollData["admin_uin"].ToObject<long>());	//被哪个人踢
                target.Add("admin_nick", pollData["admin_nick"].ToString());
                return new QQNotifyEvent(QQNotifyEventType.GROUP_NOTIFY, target.ToString());
            }
            return null;
        }
    }

}
