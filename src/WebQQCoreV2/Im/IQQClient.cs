using System.Collections.Generic;
using System.IO;
using HttpActionTools.Action;
using HttpActionTools.Actor;
using HttpActionTools.Core;
using HttpActionTools.Event;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Bean.Content;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Module;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore.Im
{
    public interface IQQClient : IQQContext
    {
        /// <summary>
        /// 获取客户端类型
        /// </summary>
        QQClientType ClientType { get; }

        void Init();

        void Destroy();

        void InitExtendQQModel(IQQModule qqModule);

        IActionLink LoginWithQRCode(ActionEventListener listener = null);

        ///// <summary>
        ///// 获取个人信息
        ///// </summary>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink GetSelfInfo(ActionEventListener listener);

        ///// <summary>
        ///// 登录
        ///// </summary>
        ///// <param name="status">状态</param>
        ///// <param name="listener">事件处理委托</param>
        ///// <returns></returns>
        //IActionLink Login(QQStatus status, ActionEventListener listener = null);

        ///// <summary>
        ///// 重新登录
        ///// </summary>
        ///// <param name="status">状态</param>
        ///// <param name="listener">事件处理委托</param>
        ///// <returns></returns>
        //IActionLink Relogin(QQStatus status, ActionEventListener listener = null);

        ///// <summary>
        ///// 登出
        ///// </summary>
        ///// <param name="listener">事件处理委托</param>
        ///// <returns></returns>
        //IActionLink Logout(ActionEventListener listener = null);

        ///// <summary>
        ///// 获得QQ基本信息，自己或者好友
        ///// </summary>
        ///// <param name="user"></param>
        ///// <param name="listener">事件处理委托</param>
        ///// <returns></returns>
        //IActionLink GetUserInfo(QQUser user, ActionEventListener listener = null);

        ///// <summary>
        ///// 改变状态
        ///// </summary>
        ///// <param name="status"></param>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink ChangeStatus(QQStatus status, ActionEventListener listener = null);

        ///// <summary>
        ///// 获取好友列表
        ///// </summary>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink GetBuddyList(ActionEventListener listener);

        ///// <summary>
        ///// 获取在线好友列表
        ///// </summary>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink GetOnlineList(ActionEventListener listener = null);

        ///// <summary>
        ///// 获取最近联系人列表
        ///// </summary>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink GetRecentList(ActionEventListener listener = null);

        ///// <summary>
        ///// 获取用户头像
        ///// </summary>
        ///// <param name="user"></param>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink GetUserFace(QQUser user, ActionEventListener listener = null);

        ///// <summary>
        ///// 获取用户签名
        ///// </summary>
        ///// <param name="user"></param>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink GetUserSign(QQUser user, ActionEventListener listener = null);

        ///// <summary>
        ///// 获取QQ号码
        ///// </summary>
        ///// <param name="user"></param>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink GetUserQQ(QQUser user, ActionEventListener listener = null);

        ///// <summary>
        ///// 获取等级
        ///// </summary>
        ///// <param name="user"></param>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink GetUserLevel(QQUser user, ActionEventListener listener = null);

        ///// <summary>
        ///// 获取陌生人信息
        ///// </summary>
        ///// <param name="user"></param>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink GetStrangerInfo(QQUser user, ActionEventListener listener = null);

        ///// <summary>
        ///// 获取群列表
        ///// </summary>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink GetGroupList(ActionEventListener listener);

        ///// <summary>
        ///// 获取群列表
        ///// </summary>
        ///// <param name="group"></param>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink GetGroupFace(QQGroup group, ActionEventListener listener = null);

        ///// <summary>
        ///// 获取群信息
        ///// </summary>
        ///// <param name="group"></param>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink GetGroupInfo(QQGroup group, ActionEventListener listener = null);

        ///// <summary>
        ///// 获取群号码
        ///// </summary>
        ///// <param name="group"></param>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink GetGroupGid(QQGroup group, ActionEventListener listener = null);

        ///// <summary>
        ///// 获取群成员状态
        ///// </summary>
        ///// <param name="group"></param>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink GetGroupMemberStatus(QQGroup group, ActionEventListener listener = null);

        ///// <summary>
        ///// 获取讨论组列表
        ///// </summary>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink GetDiscuzList(ActionEventListener listener);

        ///// <summary>
        ///// 获取讨论组信息
        ///// </summary>
        ///// <param name="discuz"></param>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink GetDiscuzInfo(QQDiscuz discuz, ActionEventListener listener = null);

        ///// <summary>
        ///// 临时消息信道，用于发送群U2U会话消息
        ///// </summary>
        ///// <param name="user"></param>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink GetSessionMsgSig(QQStranger user, ActionEventListener listener = null);

        ///// <summary>
        ///// 发送消息
        ///// </summary>
        ///// <param name="msg"></param>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink SendMsg(QQMsg msg, ActionEventListener listener = null);

        ///// <summary>
        ///// 发送抖屏
        ///// </summary>
        ///// <param name="user"></param>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink SendShake(QQUser user, ActionEventListener listener = null);

        ///// <summary>
        ///// 获取离线图片
        ///// </summary>
        ///// <param name="offpic"></param>
        ///// <param name="msg"></param>
        ///// <param name="picout"></param>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink GetOffPic(OffPicItem offpic, QQMsg msg, Stream picout, ActionEventListener listener = null);

        ///// <summary>
        ///// 获取聊天图片
        ///// </summary>
        ///// <param name="cface"></param>
        ///// <param name="msg"></param>
        ///// <param name="picout"></param>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink GetUserPic(CFaceItem cface, QQMsg msg, Stream picout, ActionEventListener listener = null);

        ///// <summary>
        ///// 获取群聊天图片
        ///// </summary>
        ///// <param name="cface"></param>
        ///// <param name="msg"></param>
        ///// <param name="picout"></param>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink GetGroupPic(CFaceItem cface, QQMsg msg, Stream picout, ActionEventListener listener = null);

        ///// <summary>
        ///// 上传离线图片
        ///// </summary>
        ///// <param name="user"></param>
        ///// <param name="file"></param>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink UploadOffPic(QQUser user, string file, ActionEventListener listener = null);

        ///// <summary>
        ///// 上传好友图片
        ///// </summary>
        ///// <param name="file"></param>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink UploadCustomPic(string file, ActionEventListener listener = null);

        ///// <summary>
        ///// 发送正在输入通知
        ///// </summary>
        ///// <param name="user"></param>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink SendInputNotify(QQUser user, ActionEventListener listener = null);

        ///// <summary>
        ///// 刷新验证码
        ///// </summary>
        ///// <param name="verifyEvent"></param>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink FreshVerify(QQNotifyEvent verifyEvent, ActionEventListener listener = null);

        ///// <summary>
        ///// 更新群消息筛选
        ///// </summary>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink UpdateGroupMessageFilter(ActionEventListener listener = null);

        ///// <summary>
        ///// 搜索群列表
        ///// </summary>
        ///// <param name="resultList"></param>
        ///// <param name="listener"></param>
        ///// <returns></returns>
        //IActionLink SearchGroupGetList(QQGroupSearchList resultList, ActionEventListener listener = null);

        ///// <summary>
        ///// 提交验证码
        ///// </summary>
        ///// <param name="code"></param>
        ///// <param name="verifyEvent"></param>
        //void SubmitVerify(string code, QQNotifyEvent verifyEvent);

        ///// <summary>
        ///// 退出验证码输入
        ///// </summary>
        ///// <param name="verifyEvent"></param>
        //void CancelVerify(QQNotifyEvent verifyEvent);

        ///// <summary>
        ///// 开始轮询消息，发送心跳包
        ///// </summary>
        //void BeginPollMsg();
        
        //void SetHttpProxy(ProxyType proxyType, string proxyHost, int proxyPort, string proxyAuthUser, string proxyAuthPassword);

        ///// <summary>
        ///// 根据UIN获得好友
        ///// </summary>
        ///// <param name="uin"></param>
        ///// <returns></returns>
        //QQBuddy GetBuddyByUin(long uin);


        ///// <summary>
        ///// 获取是否在线
        ///// </summary>
        ///// <returns></returns>
        //bool IsOnline();

        ///// <summary>
        ///// 获取是否正在登录的状态
        ///// </summary>
        ///// <returns></returns>
        //bool IsLogining();

        ///// <summary>
        /////  接受别人加你为好友的请求
        ///// </summary>
        ///// <param name="qq"></param>
        ///// <param name="listener"></param>
        //void AcceptBuddyRequest(string qq, ActionEventListener listener = null);

        ///// <summary>
        ///// 获取聊天机器人的回复
        ///// </summary>
        ///// <returns></returns>
        //IActionLink GetRobotReply(QQMsg input, RobotType robotType, ActionEventListener listener = null);

        ///// <summary>
        ///// 获取好友列表，但必须已经使用接口获取过
        ///// </summary>
        ///// <returns></returns>
        //List<QQBuddy> GetBuddyList();

        ///// <summary>
        ///// 获取群列表，但必须已经使用接口获取过
        ///// </summary>
        ///// <returns></returns>
        //List<QQGroup> GetGroupList();

        ///// <summary>
        ///// 获取讨论组列表，但必须已经使用接口获取过
        ///// </summary>
        ///// <returns></returns>
        //List<QQDiscuz> GetDiscuzList();
    }
}
