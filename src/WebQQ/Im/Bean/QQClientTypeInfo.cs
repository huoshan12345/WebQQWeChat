using System;

namespace WebQQ.Im.Bean
{
    public enum QQClientType
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
    public static class QQClientTypeInfo
    {
        public static QQClientType ValueOfRaw(string value)
        {
            QQClientType type;
            if (Enum.TryParse(value, true, out type))
            {
                return type;
            }
            return QQClientType.Unknown;
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
                return QQClientType.PcQQ;

                case 41:
                return QQClientType.WebQQ;

                case 21:
                case 22:
                case 23:
                case 24:
                return QQClientType.MobileQQ;

                case 42:
                return QQClientType.PadQQ;

                default:
                return QQClientType.Unknown;
            }
        }

    }
}
