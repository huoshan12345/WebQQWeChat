using System;

namespace iQQ.Net.WebQQCore.Im.Bean
{
    public enum QQClientTypeName
    {
        WebQQ,
        MobileQQ,
        PCQQ,
        PadQQ,
        Unknown
    }

    /// <summary>
    /// QQ客户端类型 可能有多个值对应同一种客户端的情况，这些值是否能进一步区分，还需做测试
    /// </summary>
    public sealed class QQClientType
    {
        /// <summary>PC版QQ: [1,2,3,4,5,6,10,0x1E4]</summary>
        public static readonly QQClientType PC = new QQClientType(QQClientTypeName.PCQQ);

        /// <summary>WEBQQ: [41]</summary>
        public static readonly QQClientType WEBQQ = new QQClientType(QQClientTypeName.WebQQ);

        /// <summary>手机QQ: [21,22,23,24]</summary>
        public static readonly QQClientType MOBILE = new QQClientType(QQClientTypeName.MobileQQ);

        /// <summary>平板QQ: [42] (Android还是IOS不知道是否可以区分)</summary>
        public static readonly QQClientType PAD = new QQClientType(QQClientTypeName.PadQQ);

        /// <summary> 其他值，待测试</summary>
        public static readonly QQClientType UNKNOWN = new QQClientType(QQClientTypeName.Unknown);

        public static readonly QQClientType[] QQClientAllTypes = { WEBQQ, MOBILE, PC, PAD, UNKNOWN };

        private QQClientType(QQClientTypeName type)
        {
            this.Type = type;
        }

        public string Value => Type.ToString();

        public QQClientTypeName Type { get; }

        public static QQClientType ValueOfRaw(string value)
        {
            QQClientTypeName type;
            if (Enum.TryParse(value,true, out type))
            {
                return ValueOfRaw(type);
            }
            return UNKNOWN;
        }

        public static QQClientType ValueOfRaw(QQClientTypeName value)
        {
            switch (value)
            {
                case QQClientTypeName.PCQQ: return PC;
                case QQClientTypeName.WebQQ: return WEBQQ;
                case QQClientTypeName.MobileQQ: return MOBILE;
                case QQClientTypeName.PadQQ: return PAD;
                default: return UNKNOWN;
            }
        }

        public static QQClientType ValueOfRaw(int i)
        {
            switch (i)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 10:
                case 0x1E4:
                return PC;
                case 41:
                return WEBQQ;
                case 21:
                case 22:
                case 23:
                case 24:
                return MOBILE;
                case 42:
                return PAD;
                default:
                return UNKNOWN;
            }
        }

    }
}
