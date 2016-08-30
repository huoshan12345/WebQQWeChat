using System;

namespace iQQ.Net.WebQQCore.Im.Event
{
    public abstract class QQActionEventArgs : EventArgs
    {

    }

    public class ProgressArgs : QQActionEventArgs
    {
        /** 当前进度 */
        public long Current { get; set; }
        /** 总的进度 */
        public long Total { get; set; }

        public override string ToString()
        {
            return "ProgressArgs [current=" + Current + ", total=" + Total + "]";
        }
    }

    public class CheckVerifyArgs : QQActionEventArgs
    {
        public int Result { get; set; }
        public string Code { get; set; }
        public long Uin { get; set; }
    }

    public enum QRCodeStatus
    {
        /// <summary>
        /// 扫码成功
        /// </summary>
        QRCODE_OK,

        /// <summary>
        /// 二维码未失效
        /// </summary>
        QRCODE_VALID,

        /// <summary>
        /// 二维码失效
        /// </summary>
        QRCODE_INVALID,
        
        /// <summary>
        /// 二维码认证中
        /// </summary>
        QRCODE_AUTH,  
    }

    public class CheckQRCodeArgs : QQActionEventArgs
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
