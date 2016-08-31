using System;
using System.Drawing;

namespace iQQ.Net.WebQQCore.Im.Event
{
    public abstract class QQNotifyEventArgs : EventArgs
    {

    }

    /** 需要用户识别验证码通知 登录，加好友，获取QQ号可能都需要验证码*/
    public class ImageVerify: QQNotifyEventArgs
    {
        public enum VerifyType
        {
            LOGIN, ADD_FRIEND, GET_UIN
        };

        /** 验证的类型，登陆，添加好友，获取qq号可能会出现验证码 */
        public VerifyType Type { get; set; }

        /** 验证码图片对象 **/
        public Image Image { get; set; }

        /** 需要验证的原因 */
        public string Reason { get; set; }

        /** future对象，在验证流程内部使用 */
        public IQQActionFuture Future { get; set; }

        /** 每一个验证码对应HTTP中cookie中名字为verifysession的值 */
        public string Vsession { get; set; }

        /** 验证码字符 */
        public string Vcode { get; set; }
    }


    /// <summary>
    /// 登录进度通知
    /// </summary>
    public enum LoginProgress
    {
        CHECK_VERIFY,
        UI_LOGIN,
        UI_LOGIN_VERIFY,
        CHANNEL_LOGIN,
    }

}
