using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace iQQ.Net.WebQQCore.Im.Bean
{
    [Serializable]
    public class QQGroup
    {
        public long Gid { get; set; }
        public long Gin { get; set; }
        public long Code { get; set; }
        public int Clazz { get; set; }
        public int Flag { get; set; }
        public int Level { get; set; }
        public int Mask { get; set; }
        public string Name { get; set; }
        public string Memo { get; set; }
        public string FingerMemo { get; set; }
        public DateTime CreateTime { get; set; }

        [NonSerialized]
        private Image face; // 头像
        public Image Face
        {
            get { return face; }
            set { face = value; }
        }

        public List<QQGroupMember> Members { get; set; } = new List<QQGroupMember>();

        public QQGroupMember GetMemberByUin(long uin)
        {
            return Members.FirstOrDefault(mem => mem.Uin == uin);
        }

        public override int GetHashCode()
        {
            return (int)this.Code;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || this == obj) return false;
            var g = obj as QQGroup;
            return g?.Code == this.Code;
        }

        public override string ToString()
        {
            return "QQGroup [gid=" + Gid + ", code=" + Code + ", name=" + Name + "]";
        }
    }
}
