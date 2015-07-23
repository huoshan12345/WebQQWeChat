using System;

namespace iQQ.Net.WebQQCore.Im.Bean
{
    /// <summary>
    /// 群成员
    /// </summary>
    [Serializable]
    public class QQGroupMember : QQStranger
    {
        public QQGroup Group { get; set; }

        private string card;
        public string Card
        {
            get { return card; }
            set { card = value; }
        }
    }
}
