using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WebQQ.Im.Bean.Friend;

namespace WebQQ.Im.Bean
{

    /// <summary>
    /// QQ分组
    /// </summary>
    public class Category
    {
        public int Index { get; set; }

        public int Sort { get; set; }

        public string Name { get; set; }

        // key是好友的uin
        public ConcurrentDictionary<long, QQFriend> Friends { get; }= new ConcurrentDictionary<long, QQFriend>();

        public void AddFriend(QQFriend friend)
        {
            Friends[friend.Uin] = friend;
        }
    }
}
