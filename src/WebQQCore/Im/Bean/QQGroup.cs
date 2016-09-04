using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;

namespace iQQ.Net.WebQQCore.Im.Bean
{
    
    public class QQGroup
    {
        public long Gid { get; set; }
        public long Gin { get; set; }
        public long Code { get; set; }
        public int Clazz { get; set; }
        public long Flag { get; set; }
        public int Level { get; set; }
        public int Mask { get; set; }
        public string Name { get; set; }
        public string Memo { get; set; }
        public string FingerMemo { get; set; }
        public DateTime CreateTime { get; set; }

        [JsonIgnore]
        public Image Face { get; set; }

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
            var other = obj as QQGroup;
            return Code == other?.Code;
        }

        public override string ToString()
        {
            return "QQGroup [gid=" + Gid + ", code=" + Code + ", name=" + Name + "]";
        }
    }
}
