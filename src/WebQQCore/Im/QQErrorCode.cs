using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iQQ.Net.WebQQCore.Im
{
    public enum QQErrorCode
    {
        /// <summary>
        /// 需要登录
        /// </summary>
        NeedToLogin,

        /// <summary>
        /// 登录凭证失效
        /// </summary>
        InvalidLoginAuth,

        /// <summary>
        /// 参数无效
        /// </summary>
        InvalidParameter,

        /// <summary>
        /// 非预期的相应
        /// </summary>
        UnexpectedResponse,

        /// <summary>
        /// 无效的用户
        /// </summary>
        InvalidUser,

        /// <summary>
        /// 密码错误
        /// </summary>
        WrongPassword,

        /// <summary>
        /// 验证码错误
        /// </summary>   
        WrongCaptcha,

        /// <summary>
        /// 需要验证
        /// </summary>  
        NeedCaptcha,         

        /// <summary>
        /// 网络错误
        /// </summary> 
        IOError,         

        /// <summary>
        /// 网络超时
        /// </summary>
        IOTimeout,

        /// <summary>
        /// 用户没有找到
        /// </summary>
        UserNotFound,

        /// <summary>
        /// 回答验证问题错误
        /// </summary>     
        WrongAnswer,

        /// <summary>
        /// 用户拒绝添加好友
        /// </summary>           
        UserRefuseAdd,

        /// <summary>
        /// 无法解析的结果
        /// </summary>  
        InvalidResponse,

        /// <summary>
        /// 错误的状态码
        /// </summary>       
        ErrorHttpStatus,

        /// <summary>
        /// 初始化错误
        /// </summary>
        InitError,

        /// <summary>
        /// 用户取消操作
        /// </summary>        
        Canceled,               // 用户取消操作
        UnableCancel,          // 无法取消
        JsonError,             // JSON解析出错
        UnknownError,          // 未知的错误
        WaitInteruppted,       // 等待事件被中断
        WaitTimeout,           // 等待超时
        CookieError,           // COOKIE超时

        /// <summary>
        /// 发送消息失败
        /// </summary>
        SnedMsgError,
    }

}
