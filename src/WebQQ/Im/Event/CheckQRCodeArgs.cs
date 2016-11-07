using System;
using System.ComponentModel;

namespace WebQQ.Im.Event
{
    public enum QRCodeStatus
    {
        /// <summary>
        /// 扫码成功
        /// </summary>
        OK,

        /// <summary>
        /// 二维码未失效
        /// </summary>
        [Description("未失效")]
        Valid,

        /// <summary>
        /// 二维码失效
        /// </summary>
        [Description("已失效")]
        Invalid,

        /// <summary>
        /// 二维码认证中
        /// </summary>
        [Description("认证中")]
        Auth,
    }

    public class CheckQRCodeArgs : EventArgs
    {
        public QRCodeStatus Status { get; private set; }

        public string Msg { get; private set; }

        public CheckQRCodeArgs(QRCodeStatus status, string msg)
        {
            Status = status;
            Msg = msg;
        }
    }
}
