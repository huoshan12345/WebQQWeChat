using System.ComponentModel;

namespace WebWeChat.Im.Actions.ActionResult
{
    public enum WatiForLoginResult
    {
        /// <summary>
        /// 手机已允许登录
        /// </summary>
        [Description("手机已允许登录")]
        Success = 200,

        /// <summary>
        /// 二维码失效
        /// </summary>
        [Description("二维码失效")]
        QRCodeInvalid = 408,

        /// <summary>
        /// 手机已扫码
        /// </summary>
        [Description("手机已扫码")]
        ScanCode = 201,

    }
}
