using System;

namespace iQQ.Net.WebQQCore.Im.Bean
{
    /// <summary>
    /// 群成员
    /// </summary>
    // [Serializable]
    public class QQGroupMember : QQStranger
    {
        public QQGroup Group { get; set; }

        public string Card { get; set; }
    }
}
