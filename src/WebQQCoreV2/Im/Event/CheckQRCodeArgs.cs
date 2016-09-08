using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iQQ.Net.WebQQCore.Im.Event
{
    public enum QRCodeStatus
    {
        /// <summary>
        /// 扫码成功
        /// </summary>
        QRCodeOK,

        /// <summary>
        /// 二维码未失效
        /// </summary>
        QRCodeValid,

        /// <summary>
        /// 二维码失效
        /// </summary>
        QRCodeInvalid,

        /// <summary>
        /// 二维码认证中
        /// </summary>
        QRCodeAuth,
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
