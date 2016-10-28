using System;

namespace WebQQ.Im.Bean
{
    /// <summary>
    /// QQ状态枚举
    /// </summary>
    public sealed class QQStatus
    {
        public static readonly QQStatus ONLINE = new QQStatus("online", 10);
        public static readonly QQStatus OFFLINE = new QQStatus("offline", 20);
        public static readonly QQStatus AWAY = new QQStatus("away", 30);
        public static readonly QQStatus HIDDEN = new QQStatus("hidden", 40);
        public static readonly QQStatus BUSY = new QQStatus("busy", 50);
        public static readonly QQStatus CALLME = new QQStatus("callme", 60);
        public static readonly QQStatus SILENT = new QQStatus("silent", 70);

        private static readonly QQStatus[] qqStatus = { ONLINE, OFFLINE, AWAY, HIDDEN, BUSY, CALLME, SILENT };

        public string Description
        {
            get
            {
                switch (Value)
                {
                    case "online": return "在线";
                    case "offline": return "离线";
                    case "away": return "离开";
                    case "hidden": return "隐身";
                    case "busy": return "忙碌";
                    case "callme": return "Q我吧";
                    case "silent": return "请勿打扰";
                    default: return "";
                }
            }
        }

        public string Value { get; }

        public int Status { get; }

        public override string ToString() => Value;

        private QQStatus(string value, int status)
        {
            Value = value;
            Status = status;
        }

        public static QQStatus ValueOfRaw(string txt)
        {
            foreach (var s in qqStatus)
            {
                if (s.Value.Equals(txt))
                {
                    return s;
                }
            }
            throw new ArgumentException("unknown QQStatus enum: " + txt);
        }

        public static QQStatus ValueOfRaw(int status)
        {
            foreach (var s in qqStatus)
            {
                if (s.Status == status)
                {
                    return s;
                }
            }
            throw new ArgumentException("unknown QQStatus enum: " + status);
        }

        public static bool IsGeneralOnline(QQStatus stat)
        {
            return (stat == ONLINE || stat == CALLME
                    || stat == AWAY || stat == SILENT
                    || stat == BUSY || stat == HIDDEN);
        }
    }
}
