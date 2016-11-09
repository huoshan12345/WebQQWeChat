using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

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
        public ConcurrentDictionary<long, Friend> Friends { get; }= new ConcurrentDictionary<long, Friend>();

        public void AddFriend(Friend friend)
        {
            Friends[friend.Uin] = friend;
        }
    }
}
