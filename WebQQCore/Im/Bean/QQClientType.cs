using System;

namespace iQQ.Net.WebQQCore.Im.Bean
{
    public enum QQClientTypeName
    {
        WebQQ,
        MobileQQ,
        PcQQ,
        PadQQ,
        Unknown
    }

    /// <summary>
    /// QQ客户端类型 可能有多个值对应同一种客户端的情况，这些值是否能进一步区分，还需做测试
    /// </summary>
    public sealed class QQClientType
    {
        /// <summary>PC版QQ: [1,2,3,4,5,6,10,0x1E4]</summary>
        public static readonly QQClientType Pc = new QQClientType(QQClientTypeName.PcQQ);

        /// <summary>WebQQ: [41]</summary>
        public static readonly QQClientType WebQQ = new QQClientType(QQClientTypeName.WebQQ);

        /// <summary>手机QQ: [21,22,23,24]</summary>
        public static readonly QQClientType Mobile = new QQClientType(QQClientTypeName.MobileQQ);

        /// <summary>平板QQ: [42] (Android还是IOS不知道是否可以区分)</summary>
        public static readonly QQClientType Pad = new QQClientType(QQClientTypeName.PadQQ);

        /// <summary> 其他值，待测试</summary>
        public static readonly QQClientType Unknown = new QQClientType(QQClientTypeName.Unknown);

        public static readonly QQClientType[] QQClientAllTypes = { WebQQ, Mobile, Pc, Pad, Unknown };

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
            return Unknown;
        }

        public static QQClientType ValueOfRaw(QQClientTypeName value)
        {
            switch (value)
            {
                case QQClientTypeName.PcQQ: return Pc;
                case QQClientTypeName.WebQQ: return WebQQ;
                case QQClientTypeName.MobileQQ: return Mobile;
                case QQClientTypeName.PadQQ: return Pad;
                default: return Unknown;
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
                return Pc;

                case 41:
                return WebQQ;

                case 21:
                case 22:
                case 23:
                case 24:
                return Mobile;

                case 42:
                return Pad;

                default:
                return Unknown;
            }
        }

    }
}
