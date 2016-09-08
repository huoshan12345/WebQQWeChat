using System;
using System.Collections.Generic;
using System.Linq;

namespace iQQ.Net.WebQQCore.Im.Bean
{

    /// <summary>
    /// QQ分组
    /// </summary>
    
    public class QQCategory
    {
        public int Index { get; set; }

        public int Sort { get; set; }

        public string Name { get; set; }

        public List<QQBuddy> BuddyList { get; set; } = new List<QQBuddy>();

        public QQBuddy GetQQBuddyByUin(int uin)
        {
            if (BuddyList.Count != 0 && uin != 0)
            {
                return BuddyList.FirstOrDefault(b => b.Uin == uin);
            }
            return null;
        }

        public int GetOnlineCount()
        {
            return BuddyList.Select(buddy => buddy.Status).Count(QQStatus.IsGeneralOnline);
        }

        public int GetBuddyCount() => BuddyList?.Count ?? 0;
    }
}
