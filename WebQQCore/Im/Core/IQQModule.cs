using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;

namespace iQQ.Net.WebQQCore.Im.Core
{
    public enum QQModuleType
    {
        PROC,			//登陆和退出流程执行
        LOGIN,			//核心模块，处理登录和退出的逻辑
        USER,			//个人信息管理模块
        BUDDY,			//好友管理模块
        CATEGORY,		//分组管理模块
        GROUP,			//群管理模块
        DISCUZ,			//讨论组模块
        CHAT,			//聊天模块
        EMAIL			//邮件模块
    }

    public interface IQQModule : IQQLifeCycle
    {
        IQQActionFuture PushHttpAction(IHttpAction action);

        QQModuleType GetModuleType();
    }
}
