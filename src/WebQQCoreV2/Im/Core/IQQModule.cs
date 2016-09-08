namespace iQQ.Net.WebQQCore.Im.Core
{
    public enum QQModuleType
    {
        Proc,			//登陆和退出流程执行
        Login,			//核心模块，处理登录和退出的逻辑
        User,			//个人信息管理模块
        Buddy,			//好友管理模块
        Category,		//分组管理模块
        Group,			//群管理模块
        Discuz,			//讨论组模块
        Chat,			//聊天模块
        Email			//邮件模块
    }

    public interface IQQModule : IQQLifeCycle
    {
    }
}
