using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using FxUtility.Extensions;
using WebQQ.Im.Bean;
using WebQQ.Im.Bean.Content;
using WebQQ.Im.Core;

namespace WebQQ.Im.Module.Impl
{

    /// <summary>
    /// 存储QQ相关的数据 如好友列表，分组列表，群列表，在线好友等
    /// </summary>
    public class StoreModule : QQModule
    {
        // 主键是Category的index
        private readonly ConcurrentDictionary<long, Category> _categories = new ConcurrentDictionary<long, Category>();

        // key是好友的uin
        private readonly ConcurrentDictionary<long, Friend> _friends = new ConcurrentDictionary<long, Friend>();

        public StoreModule(IQQContext context) : base(context)
        {
        }

        public IEnumerable<Friend> GetFriends => _friends.Values;

        public void AddCategory(Category category)
        {
            _categories[category.Index] = category;
        }

        public void AddFriend(Friend friend)
        {
            if (_categories.ContainsKey(friend.CategoryIndex))
            {
                _categories[friend.CategoryIndex].AddFriend(friend);
            }
            else
            {
                AddCategory(new Category() { Index = friend.CategoryIndex });
                AddFriend(friend);
            }
            _friends[friend.Uin] = friend;
        }

        public Friend GetFriendByUin(long uin)
        {
            return _friends.GetOrDefault(uin);
        }
    }
}
