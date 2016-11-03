using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WebWeChat.Im.Action.ActionResult
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
