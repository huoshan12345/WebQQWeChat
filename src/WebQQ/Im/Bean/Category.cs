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
        private readonly ConcurrentDictionary<long, Friend> _friends = new ConcurrentDictionary<long, Friend>();

        public void AddFriend(Friend friend)
        {
            _friends[friend.Uin] = friend;
        }

        public IEnumerable<Friend> GetFriends => _friends.Values;
    }
}
