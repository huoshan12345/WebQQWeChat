using System;
using System.Collections.Generic;
using System.IO;
using iQQ.Net.WebQQCore.Im.Actor;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Bean.Content;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Event.Future;
using iQQ.Net.WebQQCore.Im.Module;
using iQQ.Net.WebQQCore.Im.Service;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore.Im
{
    public class WebQQClient : IQQClient, IQQContext
    {
        private readonly Dictionary<QQServiceType, IQQService> _services;
        private readonly Dictionary<QQModuleType, IQQModule> _modules;
        private readonly IQQActorDispatcher _actorDispatcher;

        /// <summary>
        /// 构造方法，初始化模块和服务
        /// </summary>
        /// <param name="username">账号</param>
        /// <param name="password">密码</param>
        /// <param name="notifyListener">监听器</param>
        /// <param name="actorDispatcher">线程执行器</param>
        public WebQQClient(string username, string password,
                QQNotifyHandler notifyListener, IQQActorDispatcher actorDispatcher)
        {
            _modules = new Dictionary<QQModuleType, IQQModule>();
            _services = new Dictionary<QQServiceType, IQQService>();

            _modules.Add(QQModuleType.LOGIN, new LoginModule());
            _modules.Add(QQModuleType.PROC, new ProcModule());
            _modules.Add(QQModuleType.USER, new UserModule());
            _modules.Add(QQModuleType.BUDDY, new BuddyModule());
            _modules.Add(QQModuleType.CATEGORY, new CategoryModule());
            _modules.Add(QQModuleType.GROUP, new GroupModule());
            _modules.Add(QQModuleType.CHAT, new ChatModule());
            _modules.Add(QQModuleType.DISCUZ, new DiscuzModule());
            _modules.Add(QQModuleType.EMAIL, new EmailModule());
            _services.Add(QQServiceType.HTTP, new HttpService());

            Account = new QQAccount
            {
                Username = username,
                Password = password
            };
            Session = new QQSession();
            Store = new QQStore();
            NotifyListener = notifyListener;
            _actorDispatcher = actorDispatcher;

            Init();
        }

        /// <summary>
        /// 获取某个类型的模块
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public T GetModule<T>(QQModuleType type) where T : IQQModule
        {
            return (T)_modules[type]; // 获取失败将抛出异常
        }

        /// <summary>
        /// 获取某个类型的服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public T GetSerivce<T>(QQServiceType type) where T : IQQService
        {
            return (T)_services[type]; // 获取失败将抛出异常
        }

        /// <summary>
        /// 设置HTTP的用户信息
        /// </summary>
        /// <param name="userAgent"></param>
        public void SetHttpUserAgent(string userAgent)
        {
            var http = GetSerivce<IHttpService>(QQServiceType.HTTP);
            http.UserAgent = userAgent;
        }

        /// <summary>
        /// 设置代理
        /// </summary>
        /// <param name="proxyType"></param>
        /// <param name="proxyHost"></param>
        /// <param name="proxyPort"></param>
        /// <param name="proxyAuthUser"></param>
        /// <param name="proxyAuthPassword"></param>
        public void SetHttpProxy(ProxyType proxyType, string proxyHost,
                int proxyPort, string proxyAuthUser, string proxyAuthPassword)
        {
            var http = GetSerivce<IHttpService>(QQServiceType.HTTP);
            http.SetHttpProxy(proxyType, proxyHost, proxyPort,
                                proxyAuthUser, proxyAuthPassword);
        }


        public QQClientType ClientType => QQClientType.WebQQ;

        /// <summary>
        /// 获取自己的账号实体
        /// </summary>
        public QQAccount Account { get; set; }

        /// <summary>
        /// 获取会话信息
        /// </summary>
        public QQSession Session { get; set; }

        /// <summary>
        /// 获取QQ存储信息，包括获取过后的好友/群好友；还有一些其它的认证信息
        /// </summary>
        public QQStore Store { get; set; }

        public QQNotifyHandler NotifyListener { get; set; }

        /// <summary>
        /// 放入一个QQActor到队列，将会在线程执行器里面执行
        /// </summary>
        /// <param name="actor"></param>
        public void PushActor(QQActor actor)
        {
            _actorDispatcher.PushActor(actor);
        }

        /// <summary>
        /// 初始化所有模块和服务
        /// </summary>
        private void Init()
        {
            try
            {
                foreach (var type in _services.Keys)
                {
                    var service = _services[type];
                    service.Init(this);
                }

                foreach (var type in _modules.Keys)
                {
                    var module = _modules[type];
                    module.Init(this);
                }

                _actorDispatcher.Init(this);
                Store.Init(this);
            }
            catch (QQException e)
            {
                MyLogger.Default.Warn("初始化模块和服务失败", e);
            }
        }

        /// <summary>
        /// 销毁所有模块和服务
        /// </summary>
        public void Destroy()
        {
            try
            {
                foreach (var module in _modules.Values)
                {
                    module.Destroy();
                }

                foreach (var service in _services.Values)
                {
                    service.Destroy();
                }

                // _actorDispatcher.Destroy();
                Store.Destroy();
            }
            catch (QQException e)
            {
                MyLogger.Default.Warn("销毁所有模块和服务失败", e);
            }
        }

        public IQQActionFuture GetSelfInfo(QQActionEventHandler listener)
        {
            var mod = GetModule<LoginModule>(QQModuleType.LOGIN);
            return mod.GetSelfInfo(listener);
        }

        public void InitExtendQQModel(IQQModule qqModule)
        {
            qqModule.Init(this);
        }

        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="status"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture Login(QQStatus status, QQActionEventHandler listener)
        {
            //检查客户端状态，是否允许登陆
            if (Session.State == QQSessionState.ONLINE)
            {
                throw new Exception("client is aready online !!!");
            }
            Account.Status = status;
            Session.State = QQSessionState.LOGINING;

            var procModule = GetModule<ProcModule>(QQModuleType.PROC);
            return procModule.Login(listener);
        }

        public void SaveCookie(string fileName = "")
        {
            var http = GetSerivce<IHttpService>(QQServiceType.HTTP);
            http.SaveCookie(fileName);
        }

        public void ReadCookie(string fileName = "")
        {
            var http = GetSerivce<IHttpService>(QQServiceType.HTTP);
            http.ReadCookie(fileName);
        }

        /// <summary>
        /// 重新登录
        /// </summary>
        /// <param name="status"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture Relogin(QQStatus status, QQActionEventHandler listener)
        {
            if (Session.State == QQSessionState.ONLINE)
            {
                throw new Exception("client is aready online !!!");
            }

            Account.Status = status;
            Session.State = QQSessionState.LOGINING;
            var procModule = GetModule<ProcModule>(QQModuleType.PROC);
            return procModule.Relogin(status, listener);
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="listener"></param>
        public void GetCaptcha(QQActionEventHandler listener)
        {
            var loginModule = GetModule<LoginModule>(QQModuleType.LOGIN);
            loginModule.GetCaptcha(Account.Uin, listener);
        }

        /// <summary>
        /// 通知事件
        /// </summary>
        /// <param name="Event"></param>
        public void FireNotify(QQNotifyEvent Event)
        {
            if (NotifyListener != null)
            {
                try
                {
                    NotifyListener(this, Event);
                }
                catch (Exception e)
                {
                    MyLogger.Default.Warn("FireNotify Error!!", e);
                }
            }
            // 重新登录成功，重新poll
            if (Event.Type == QQNotifyEventType.RELOGIN_SUCCESS)
            {
                BeginPollMsg();
            }
        }

        /// <summary>
        /// 轮询QQ消息
        /// </summary>
        public void BeginPollMsg()
        {
            if (Session.State == QQSessionState.OFFLINE)
            {
                throw new Exception("client is aready offline !!!");
            }

            var procModule = GetModule<ProcModule>(QQModuleType.PROC);
            procModule.DoPollMsg();

            // 轮询邮件
            //             EmailModule emailModule = GetModule<EmailModule>(QQModuleType.EMAIL);
            //             emailModule.DoPoll();
        }

        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture GetBuddyList(QQActionEventHandler listener)
        {
            var categoryModule = GetModule<CategoryModule>(QQModuleType.CATEGORY);
            return categoryModule.GetCategoryList(listener);
        }

        /// <summary>
        /// 获取群列表
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture GetGroupList(QQActionEventHandler listener)
        {
            var groupModule = GetModule<GroupModule>(QQModuleType.GROUP);
            return groupModule.GetGroupList(listener);
        }

        /// <summary>
        /// 获取在线好友列表
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture GetOnlineList(QQActionEventHandler listener)
        {
            var buddyModule = GetModule<BuddyModule>(QQModuleType.BUDDY);
            return buddyModule.GetOnlineBuddy(listener);
        }

        /// <summary>
        /// 获取最近联系人列表
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture GetRecentList(QQActionEventHandler listener)
        {
            var buddyModule = GetModule<BuddyModule>(QQModuleType.BUDDY);
            return buddyModule.GetRecentList(listener);
        }

        /// <summary>
        /// 获取QQ号码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture GetUserQQ(QQUser user, QQActionEventHandler listener)
        {
            var userModule = GetModule<UserModule>(QQModuleType.USER);
            return userModule.GetUserAccount(user, listener);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture Logout(QQActionEventHandler listener)
        {
            if (Session.State == QQSessionState.OFFLINE)
            {
                // MyLogger.Default.Warn($"{Account.Username}: client is aready offline !!!");
                throw new Exception("client is aready offline !!!");
            }

            var procModule = GetModule<ProcModule>(QQModuleType.PROC);
            return procModule.DoLogout((sender, Event) =>
            {
                // 无论退出登录失败还是成功，都需要释放资源
                if (Event.Type == QQActionEventType.EVT_OK
                    || Event.Type == QQActionEventType.EVT_ERROR)
                {
                    Session.State = QQSessionState.OFFLINE;
                    Destroy();
                }
                listener?.Invoke(sender, Event);
            });
        }

        /// <summary>
        /// 提交验证码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="verifyEvent"></param>
        public void SubmitVerify(string code, QQNotifyEvent verifyEvent)
        {
            var verify =(ImageVerify)verifyEvent.Target;

            if (verify.Type == ImageVerify.VerifyType.LOGIN)
            {
                var mod = GetModule<ProcModule>(QQModuleType.PROC);
                mod.LoginWithVerify(code, (ProcActionFuture)verify.Future);
            }
        }

        /// <summary>
        /// 获取自己或者好友信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture GetUserInfo(QQUser user, QQActionEventHandler listener)
        {
            var mod = GetModule<UserModule>(QQModuleType.USER);
            return mod.GetUserInfo(user, listener);
        }

        /// <summary>
        /// 改变状态
        /// </summary>
        /// <param name="status"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture ChangeStatus(QQStatus status, QQActionEventHandler listener)
        {
            var userModule = GetModule<UserModule>(QQModuleType.USER);
            return userModule.ChangeStatus(status, listener);
        }

        /// <summary>
        /// 获取群列表
        /// </summary>
        /// <param name="group"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture GetGroupFace(QQGroup group, QQActionEventHandler listener)
        {
            var mod = GetModule<GroupModule>(QQModuleType.GROUP);
            return mod.GetGroupFace(group, listener);
        }

        /// <summary>
        /// 获取群信息
        /// </summary>
        /// <param name="group"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture GetGroupInfo(QQGroup group, QQActionEventHandler listener)
        {
            var mod = GetModule<GroupModule>(QQModuleType.GROUP);
            return mod.GetGroupInfo(group, listener);
        }

        /// <summary>
        /// 获取群号码
        /// </summary>
        /// <param name="group"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture GetGroupGid(QQGroup group, QQActionEventHandler listener)
        {
            var mod = GetModule<GroupModule>(QQModuleType.GROUP);
            return mod.GetGroupGid(group, listener);
        }

        /// <summary>
        /// 获取用户头像
        /// </summary>
        /// <param name="user"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture GetUserFace(QQUser user, QQActionEventHandler listener)
        {
            var mod = GetModule<UserModule>(QQModuleType.USER);
            return mod.GetUserFace(user, listener);
        }

        /// <summary>
        /// 获取用户签名
        /// </summary>
        /// <param name="user"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture GetUserSign(QQUser user, QQActionEventHandler listener)
        {
            var mod = GetModule<UserModule>(QQModuleType.USER);
            return mod.GetUserSign(user, listener);
        }

        /// <summary>
        /// 获取等级
        /// </summary>
        /// <param name="user"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture GetUserLevel(QQUser user, QQActionEventHandler listener)
        {
            var mod = GetModule<UserModule>(QQModuleType.USER);
            return mod.GetUserLevel(user, listener);
        }

        /// <summary>
        /// 获取陌生人信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture GetStrangerInfo(QQUser user, QQActionEventHandler listener)
        {
            var mod = GetModule<UserModule>(QQModuleType.USER);
            return mod.GetStrangerInfo(user, listener);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture SendMsg(QQMsg msg, QQActionEventHandler listener)
        {
            var mod = GetModule<ChatModule>(QQModuleType.CHAT);
            return mod.SendMsg(msg, listener);
        }

        public IQQActionFuture GetRobotReply(QQMsg input, RobotType robotType, QQActionEventHandler listener)
        {
            var mod = GetModule<ChatModule>(QQModuleType.CHAT);
            return mod.GetRobotReply(input, robotType, listener);
        }

        /// <summary>
        /// 发送抖屏
        /// </summary>
        /// <param name="user"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture SendShake(QQUser user, QQActionEventHandler listener)
        {
            var mod = GetModule<ChatModule>(QQModuleType.CHAT);
            return mod.SendShake(user, listener);
        }

        /// <summary>
        /// 获取离线图片
        /// </summary>
        /// <param name="offpic"></param>
        /// <param name="msg"></param>
        /// <param name="picout"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture GetOffPic(OffPicItem offpic, QQMsg msg, Stream picout,
                        QQActionEventHandler listener)
        {
            var mod = GetModule<ChatModule>(QQModuleType.CHAT);
            return mod.GetOffPic(offpic, msg, picout, listener);
        }

        public IQQActionFuture GetUserPic(CFaceItem cface, QQMsg msg,
                        Stream picout, QQActionEventHandler listener)
        {
            var mod = GetModule<ChatModule>(QQModuleType.CHAT);
            return mod.GetUserPic(cface, msg, picout, listener);
        }

        /// <summary>
        /// 获取群聊天图片
        /// </summary>
        /// <param name="cface"></param>
        /// <param name="msg"></param>
        /// <param name="picout"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture GetGroupPic(CFaceItem cface, QQMsg msg,
                Stream picout, QQActionEventHandler listener)
        {
            var mod = GetModule<ChatModule>(QQModuleType.CHAT);
            return mod.GetGroupPic(cface, msg, picout, listener);
        }

        /// <summary>
        /// 上传离线图片
        /// </summary>
        /// <param name="user"></param>
        /// <param name="file"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture UploadOffPic(QQUser user, string file, QQActionEventHandler listener)
        {
            var mod = GetModule<ChatModule>(QQModuleType.CHAT);
            return mod.UploadOffPic(user, file, listener);
        }

        /// <summary>
        /// 上传好友图片
        /// </summary>
        /// <param name="file"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture UploadCustomPic(string file, QQActionEventHandler listener)
        {
            var mod = GetModule<ChatModule>(QQModuleType.CHAT);
            return mod.UploadCFace(file, listener);
        }

        /// <summary>
        /// 发送正在输入通知
        /// </summary>
        /// <param name="user"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture SendInputNotify(QQUser user, QQActionEventHandler listener)
        {
            var mod = GetModule<ChatModule>(QQModuleType.CHAT);
            return mod.SendInputNotify(user, listener);
        }

        /// <summary>
        /// 获取讨论组列表
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture GetDiscuzList(QQActionEventHandler listener)
        {
            var mod = GetModule<DiscuzModule>(QQModuleType.DISCUZ);
            return mod.GetDiscuzList(listener);
        }

        /// <summary>
        /// 获取讨论组信息
        /// </summary>
        /// <param name="discuz"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture GetDiscuzInfo(QQDiscuz discuz, QQActionEventHandler listener)
        {
            var mod = GetModule<DiscuzModule>(QQModuleType.DISCUZ);
            return mod.GetDiscuzInfo(discuz, listener);
        }

        /// <summary>
        /// 临时消息信道，用于发送群U2U会话消息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture GetSessionMsgSig(QQStranger user, QQActionEventHandler listener)
        {
            var mod = GetModule<ChatModule>(QQModuleType.CHAT);
            return mod.GetSessionMsgSig(user, listener);
        }

        /// <summary>
        /// 获取群成员状态
        /// </summary>
        /// <param name="group"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture GetGroupMemberStatus(QQGroup group, QQActionEventHandler listener)
        {
            var mod = GetModule<GroupModule>(QQModuleType.GROUP);
            return mod.GetMemberStatus(group, listener);
        }

        /// <summary>
        /// 刷新验证码
        /// </summary>
        /// <param name="verifyEvent"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture FreshVerify(QQNotifyEvent verifyEvent, QQActionEventHandler listener)
        {
            var mod = GetModule<LoginModule>(QQModuleType.LOGIN);
            return mod.GetCaptcha(Account.Uin, listener);
        }

        /// <summary>
        /// 更新群消息筛选
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture UpdateGroupMessageFilter(QQActionEventHandler listener)
        {
            var mod = GetModule<GroupModule>(QQModuleType.GROUP);
            return mod.UpdateGroupMessageFilter(listener);
        }

        /// <summary>
        /// 搜索群列表
        /// </summary>
        /// <param name="resultList"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture SearchGroupGetList(QQGroupSearchList resultList, QQActionEventHandler listener)
        {
            var mod = GetModule<GroupModule>(QQModuleType.GROUP);
            return mod.SearchGroup(resultList, listener);
        }

        /// <summary>
        /// 退出验证码输入
        /// </summary>
        /// <param name="verifyEvent"></param>
        public void CancelVerify(QQNotifyEvent verifyEvent)
        {
            var verify = (ImageVerify)verifyEvent.Target;
            verify.Future.Cancel();
        }

        /// <summary>
        /// 获取好友列表，但必须已经使用接口获取过
        /// </summary>
        /// <returns></returns>
        public List<QQBuddy> GetBuddyList()
        {
            return Store.GetBuddyList();
        }

        /// <summary>
        /// 获取群列表，但必须已经使用接口获取过
        /// </summary>
        /// <returns></returns>
        public List<QQGroup> GetGroupList()
        {
            return Store.GetGroupList();
        }

        /// <summary>
        /// 获取讨论组列表，但必须已经使用接口获取过
        /// </summary>
        /// <returns></returns>
        public List<QQDiscuz> GetDiscuzList()
        {
            return Store.GetDiscuzList();
        }

        /// <summary>
        /// 根据UIN获得好友
        /// </summary>
        /// <param name="uin"></param>
        /// <returns></returns>
        public QQBuddy GetBuddyByUin(long uin)
        {
            return Store.GetBuddyByUin(uin);
        }

        /// <summary>
        /// 获取是否在线
        /// </summary>
        /// <returns></returns>
        public bool IsOnline()
        {
            return Session.State == QQSessionState.ONLINE;
        }

        /// <summary>
        /// 获取是否正在登录的状态
        /// </summary>
        /// <returns></returns>
        public bool IsLogining()
        {
            return Session.State == QQSessionState.LOGINING;
        }

        public void AcceptBuddyRequest(string qq, QQActionEventHandler listener = null)
        {
            var mod = GetModule<BuddyModule>(QQModuleType.BUDDY);
            mod.AddBuddy(listener, qq);
        }

        /// <summary>
        /// 获取登录二维码
        /// </summary>
        /// <param name="qqActionListener"></param>
        public IQQActionFuture GetQRCode(QQActionEventHandler qqActionListener)
        {
            var login = GetModule<ProcModule>(QQModuleType.PROC);
            return login.GetQRCode(qqActionListener);
        }

        /// <summary>
        /// 检测登录二维码是否已经扫描
        /// </summary>
        /// <param name="qqActionListener"></param>
        //public QQActionFuture CheckQRCode(QQActionEventHandler qqActionListener)
        //{
        //    var module = GetModule<ProcModule>(QQModuleType.PROC);
        //    return module.CheckQRCode(qqActionListener);
        //}
    }
}
