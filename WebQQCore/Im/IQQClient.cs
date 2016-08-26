using System.Collections.Generic;
using System.IO;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Bean.Content;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Service;
using iQQ.Net.WebQQCore.Util;
using iQQ.Net.WebQQCore.Im.Core;

namespace iQQ.Net.WebQQCore.Im
{

    /// <summary>
    /// <para>QQ客户端接口</para> 
    /// <para>author：solosky</para> 
    /// </summary>
    public interface IQQClient
    {
        /// <summary>
        /// 获取账号信息
        /// </summary>
        QQAccount Account { get; }

        /// <summary>
        /// 获取客户端类型
        /// </summary>
        QQClientType ClientType { get; }

        void Destroy();

        QQActionFuture GetSelfInfo(QQActionEventHandler listener);

        void InitExtendQQModel(IQQModule qqModule);

        void GetQRCode(QQActionEventHandler qqActionListener);

        void CheckQRCode(QQActionEventHandler qqActionListener);

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="listener">事件处理委托</param>
        /// <returns></returns>
        QQActionFuture Login(QQStatus status, QQActionEventHandler listener = null);

        /// <summary>
        /// 重新登录
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="listener">事件处理委托</param>
        /// <returns></returns>
        QQActionFuture Relogin(QQStatus status, QQActionEventHandler listener = null);

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="listener">事件处理委托</param>
        /// <returns></returns>
        QQActionFuture Logout(QQActionEventHandler listener = null);

        /// <summary>
        /// 获得QQ基本信息，自己或者好友
        /// </summary>
        /// <param name="user"></param>
        /// <param name="listener">事件处理委托</param>
        /// <returns></returns>
        QQActionFuture GetUserInfo(QQUser user, QQActionEventHandler listener = null);

        /// <summary>
        /// 改变状态
        /// </summary>
        /// <param name="status"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture ChangeStatus(QQStatus status, QQActionEventHandler listener = null);
       
        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture GetBuddyList(QQActionEventHandler listener);

        /// <summary>
        /// 获取在线好友列表
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture GetOnlineList(QQActionEventHandler listener = null);

        /// <summary>
        /// 获取最近联系人列表
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture GetRecentList(QQActionEventHandler listener = null);

        /// <summary>
        /// 获取用户头像
        /// </summary>
        /// <param name="user"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture GetUserFace(QQUser user, QQActionEventHandler listener = null);

        /// <summary>
        /// 获取用户签名
        /// </summary>
        /// <param name="user"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture GetUserSign(QQUser user, QQActionEventHandler listener = null);

        /// <summary>
        /// 获取QQ号码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture GetUserQQ(QQUser user, QQActionEventHandler listener = null);

        /// <summary>
        /// 获取等级
        /// </summary>
        /// <param name="user"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture GetUserLevel(QQUser user, QQActionEventHandler listener = null);

        /// <summary>
        /// 获取陌生人信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture GetStrangerInfo(QQUser user, QQActionEventHandler listener = null);

        /// <summary>
        /// 获取群列表
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture GetGroupList(QQActionEventHandler listener);

        /// <summary>
        /// 获取群列表
        /// </summary>
        /// <param name="group"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture GetGroupFace(QQGroup group, QQActionEventHandler listener = null);

        /// <summary>
        /// 获取群信息
        /// </summary>
        /// <param name="group"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture GetGroupInfo(QQGroup group, QQActionEventHandler listener = null);

        /// <summary>
        /// 获取群号码
        /// </summary>
        /// <param name="group"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture GetGroupGid(QQGroup group, QQActionEventHandler listener = null);

        /// <summary>
        /// 获取群成员状态
        /// </summary>
        /// <param name="group"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture GetGroupMemberStatus(QQGroup group, QQActionEventHandler listener = null);

        /// <summary>
        /// 获取讨论组列表
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture GetDiscuzList(QQActionEventHandler listener);

        /// <summary>
        /// 获取讨论组信息
        /// </summary>
        /// <param name="discuz"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture GetDiscuzInfo(QQDiscuz discuz, QQActionEventHandler listener = null);

        /// <summary>
        /// 临时消息信道，用于发送群U2U会话消息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture GetSessionMsgSig(QQStranger user, QQActionEventHandler listener = null);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture SendMsg(QQMsg msg, QQActionEventHandler listener = null);

        /// <summary>
        /// 发送抖屏
        /// </summary>
        /// <param name="user"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture SendShake(QQUser user, QQActionEventHandler listener = null);

        /// <summary>
        /// 获取离线图片
        /// </summary>
        /// <param name="offpic"></param>
        /// <param name="msg"></param>
        /// <param name="picout"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture GetOffPic(OffPicItem offpic, QQMsg msg, Stream picout, QQActionEventHandler listener = null);

        /// <summary>
        /// 获取聊天图片
        /// </summary>
        /// <param name="cface"></param>
        /// <param name="msg"></param>
        /// <param name="picout"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture GetUserPic(CFaceItem cface, QQMsg msg, Stream picout, QQActionEventHandler listener = null);

        /// <summary>
        /// 获取群聊天图片
        /// </summary>
        /// <param name="cface"></param>
        /// <param name="msg"></param>
        /// <param name="picout"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture GetGroupPic(CFaceItem cface, QQMsg msg, Stream picout, QQActionEventHandler listener = null);

        /// <summary>
        /// 上传离线图片
        /// </summary>
        /// <param name="user"></param>
        /// <param name="file"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture UploadOffPic(QQUser user, string file, QQActionEventHandler listener = null);

        /// <summary>
        /// 上传好友图片
        /// </summary>
        /// <param name="file"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture UploadCustomPic(string file, QQActionEventHandler listener = null);

        /// <summary>
        /// 发送正在输入通知
        /// </summary>
        /// <param name="user"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture SendInputNotify(QQUser user, QQActionEventHandler listener = null);

        /// <summary>
        /// 刷新验证码
        /// </summary>
        /// <param name="verifyEvent"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture FreshVerify(QQNotifyEvent verifyEvent, QQActionEventHandler listener = null);

        /// <summary>
        /// 更新群消息筛选
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture UpdateGroupMessageFilter(QQActionEventHandler listener = null);

        /// <summary>
        /// 搜索群列表
        /// </summary>
        /// <param name="resultList"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        QQActionFuture SearchGroupGetList(QQGroupSearchList resultList, QQActionEventHandler listener = null);

        /// <summary>
        /// 提交验证码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="verifyEvent"></param>
        void SubmitVerify(string code, QQNotifyEvent verifyEvent);

        /// <summary>
        /// 退出验证码输入
        /// </summary>
        /// <param name="verifyEvent"></param>
        void CancelVerify(QQNotifyEvent verifyEvent);

        /// <summary>
        /// 开始轮询消息，发送心跳包
        /// </summary>
        void BeginPollMsg();
        /**
         * <p>setHttpUserAgent.</p>
         *
         * @param userAgent a {@link java.lang.String} object.
         */
        void SetHttpUserAgent(string userAgent);
        /**
         * <p>SetHttpProxy.</p>
         *
         * @param proxyType a {@link iqq.im.service.IHttpService.ProxyType} object.
         * @param proxyHost a {@link java.lang.String} object.
         * @param proxyPort a int.
         * @param proxyAuthUser a {@link java.lang.String} object.
         * @param proxyAuthPassword a {@link java.lang.String} object.
         */
        void SetHttpProxy(ProxyType proxyType, string proxyHost, int proxyPort, string proxyAuthUser, string proxyAuthPassword);
        
        /// <summary>
        /// 根据UIN获得好友
        /// </summary>
        /// <param name="uin"></param>
        /// <returns></returns>
        QQBuddy GetBuddyByUin(long uin);


        /// <summary>
        /// 获取是否在线
        /// </summary>
        /// <returns></returns>
        bool IsOnline();

        /// <summary>
        /// 获取是否正在登录的状态
        /// </summary>
        /// <returns></returns>
        bool IsLogining();

        /// <summary>
        ///  接受别人加你为好友的请求
        /// </summary>
        /// <param name="qq"></param>
        /// <param name="listener"></param>
        void AcceptBuddyRequest(string qq, QQActionEventHandler listener = null);

        /// <summary>
        /// 获取聊天机器人的回复
        /// </summary>
        /// <returns></returns>
        QQActionFuture GetRobotReply(QQMsg input, RobotType robotType, QQActionEventHandler listener = null);

        /// <summary>
        /// 获取好友列表，但必须已经使用接口获取过
        /// </summary>
        /// <returns></returns>
        List<QQBuddy> GetBuddyList();

        /// <summary>
        /// 获取群列表，但必须已经使用接口获取过
        /// </summary>
        /// <returns></returns>
        List<QQGroup> GetGroupList();

        /// <summary>
        /// 获取讨论组列表，但必须已经使用接口获取过
        /// </summary>
        /// <returns></returns>
        List<QQDiscuz> GetDiscuzList();
    }
}
