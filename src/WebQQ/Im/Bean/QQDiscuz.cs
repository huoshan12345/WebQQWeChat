using System.Collections.Generic;
using System.Linq;

namespace WebQQ.Im.Bean
{
    /// <summary>
    /// QQ讨论组
    /// </summary>
    
    public class QQDiscuz
    {
        /// <summary>
        /// 讨论组ID，每次登陆都固定，视为没有变换
        /// </summary>
        public long Did { get; set; }
        /// <summary>
        /// 讨论组的名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 创建者的UIN
        /// </summary>
        public long Owner { get; set; }

        /// <summary>
        /// 讨论组成员
        /// </summary>
        public List<QQDiscuzMember> Members { get; set; } = new List<QQDiscuzMember>();

        public QQDiscuzMember GetMemberByUin(long uin)
        {
            return Members.FirstOrDefault(mem => mem.Uin == uin);
        }

        public void ClearStatus()
        {
            foreach (var mem in Members)
            {
                mem.Status = QQStatus.OFFLINE;
            }
        }
        public void AddMemeber(QQDiscuzMember user)
        {
            Members.Add(user);
        }

        public override int GetHashCode()
        {
            var prime = 31;
            var result = 1;
            result = prime * result + (int)((ulong)Did ^ ((ulong)Did >> 32));
            return result;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (obj == null)
                return false;
            if (GetType() != obj.GetType())
                return false;
            var other = (QQDiscuz)obj;
            if (Did != other.Did)
                return false;
            return true;
        }

        public override string ToString()
        {
            return "QQDiscuz [did=" + Did + ", name=" + Name + ", owner=" + Owner + "]";
        }
    }
}
