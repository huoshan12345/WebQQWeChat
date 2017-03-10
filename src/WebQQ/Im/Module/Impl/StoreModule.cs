using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using FclEx.Extensions;
using WebQQ.Im.Bean;
using WebQQ.Im.Bean.Content;
using WebQQ.Im.Bean.Discussion;
using WebQQ.Im.Bean.Friend;
using WebQQ.Im.Bean.Group;
using WebQQ.Im.Core;

namespace WebQQ.Im.Module.Impl
{

    /// <summary>
    /// 存储QQ相关的数据 如好友列表，分组列表，群列表，在线好友等
    /// </summary>
    public class StoreModule : QQModule
    {
        /// <summary>
        /// 好友分组
        /// 主键是Category的index
        /// </summary>
        public ConcurrentDictionary<long, Category> CategoryDic { get; } = new ConcurrentDictionary<long, Category>();

        /// <summary>
        /// 群
        /// 主键是Group的Code
        /// </summary>
        public ConcurrentDictionary<long, QQGroup> GroupDic { get; } = new ConcurrentDictionary<long, QQGroup>();

        /// <summary>
        /// 讨论组
        /// 主键是Discuz的Did
        /// </summary>
        public ConcurrentDictionary<long, QQDiscussion> DiscussionDic { get; } = new ConcurrentDictionary<long, QQDiscussion>();

        /// <summary>
        /// 好友
        /// key是好友的uin
        /// </summary>
        public ConcurrentDictionary<long, QQFriend> FriendDic { get; } = new ConcurrentDictionary<long, QQFriend>();

        public StoreModule(IQQContext context) : base(context)
        {
        }

        public void AddCategory(Category category)
        {
            CategoryDic[category.Index] = category;
        }

        public void AddFriend(QQFriend friend)
        {
            if (CategoryDic.ContainsKey(friend.CategoryIndex))
            {
                CategoryDic[friend.CategoryIndex].AddFriend(friend);
            }
            else
            {
                AddCategory(new Category() { Index = friend.CategoryIndex });
                AddFriend(friend);
            }
            FriendDic[friend.Uin] = friend;
        }

        public QQFriend GetFriendByUin(long uin)
        {
            return FriendDic.GetOrDefault(uin);
        }

        public QQFriend GetOrAddFriendByUin(long uin)
        {
            var f = FriendDic.GetOrDefault(uin);
            if (f == null)
            {
                f = new QQFriend() {Uin = uin};
                AddFriend(f);
            }
            return f;
        }

        public QQFriend GetOrAddFriendByUin(long uin, Func<long, QQFriend> func)
        {
            var f = FriendDic.GetOrDefault(uin);
            if (f == null)
            {
                f = func(uin);
                AddFriend(f);
            }
            return f;
        }

        public void AddGroup(QQGroup group)
        {
            GroupDic[group.Gid] = group;
        }

        public QQGroup GetGroupByGid(long gid)
        {
            return GroupDic.GetOrDefault(gid);
        }

        public QQGroup GetOrAddGroupByGid(long gid)
        {
            var g = GetGroupByGid(gid);
            if (g == null)
            {
                g = new QQGroup() { Gid = gid };
                AddGroup(g);
            }
            return g;
        }

        public QQGroup GetOrAddGroupByGid(long gid, Func<long, QQGroup> func)
        {
            var g = GetGroupByGid(gid);
            if (g == null)
            {
                g = func(gid);
                AddGroup(g);
            }
            return g;
        }

        public void AddDiscussion(QQDiscussion discussion)
        {
            DiscussionDic[discussion.Did] = discussion;
        }

        public QQDiscussion GetDiscuzByDid(long did)
        {
            return DiscussionDic.GetOrDefault(did);
        }
    }
}
