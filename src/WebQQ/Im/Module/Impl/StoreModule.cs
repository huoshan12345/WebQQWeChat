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

        /// <summary>
        /// 主键是Category的index
        /// </summary>
        public ConcurrentDictionary<long, Category> Categories { get; } = new ConcurrentDictionary<long, Category>();

        /// <summary>
        /// 主键是Group的Gid
        /// </summary>
        public ConcurrentDictionary<long, Group> Groups { get; } = new ConcurrentDictionary<long, Group>();
        
        /// <summary>
        /// key是好友的uin
        /// </summary>
        public ConcurrentDictionary<long, Friend> Friends { get; } = new ConcurrentDictionary<long, Friend>();

        public StoreModule(IQQContext context) : base(context)
        {
        }

        public void AddCategory(Category category)
        {
            Categories[category.Index] = category;
        }

        public void AddFriend(Friend friend)
        {
            if (Categories.ContainsKey(friend.CategoryIndex))
            {
                Categories[friend.CategoryIndex].AddFriend(friend);
            }
            else
            {
                AddCategory(new Category() { Index = friend.CategoryIndex });
                AddFriend(friend);
            }
            Friends[friend.Uin] = friend;
        }

        public Friend GetFriendByUin(long uin)
        {
            return Friends.GetOrDefault(uin);
        }

        public void AddGroup(Group group)
        {
            Groups[group.Gid] = group;
        }

        public Group GetGroupByGid(long gid)
        {
            return Groups.GetOrDefault(gid);
        }
    }
}
