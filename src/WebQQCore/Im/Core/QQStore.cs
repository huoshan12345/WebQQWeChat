using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Bean.Content;
using iQQ.Net.WebQQCore.Util;
using iQQ.Net.WebQQCore.Util.Extensions;

namespace iQQ.Net.WebQQCore.Im.Core
{

    /// <summary>
    /// 存储QQ相关的数据 如好友列表，分组列表，群列表，在线好友等
    /// </summary>
    public class QQStore : IQQLifeCycle
    {
        private readonly ConcurrentDictionary<long, QQBuddy> _buddyMap; // uin => QQBudy, 快速通过uin查找QQ好友
        private readonly ConcurrentDictionary<long, QQStranger> _strangerMap; // uin => QQStranger, 快速通过uin查找临时会话用户
        private readonly ConcurrentDictionary<long, QQCategory> _categoryMap; // index => QQCategory
        private readonly ConcurrentDictionary<long, QQDiscuz> _discuzMap;		// did = > QQDiscuz
        private readonly ConcurrentDictionary<long, QQGroup> _groupMap; // code => QQGroup, 快速通过群ID查找群
        private readonly List<IContentItem> _pictureItemList; // filename -> PicItem 上传图片列表

        /**
         * <p>Constructor for QQStore.</p>
         */
        public QQStore()
        {
            this._buddyMap = new ConcurrentDictionary<long, QQBuddy>();
            this._strangerMap = new ConcurrentDictionary<long, QQStranger>();
            this._categoryMap = new ConcurrentDictionary<long, QQCategory>();
            this._groupMap = new ConcurrentDictionary<long, QQGroup>();
            this._discuzMap = new ConcurrentDictionary<long, QQDiscuz>();
            this._pictureItemList = new List<IContentItem>();
        }


        public void Init(IQQContext context) { }


        public void Destroy() { }


        public void AddBuddy(QQBuddy buddy)
        {
            _buddyMap[buddy.Uin] = buddy;
        }


        public void AddStranger(QQStranger stranger)
        {
            _strangerMap[stranger.Uin] = stranger;
        }


        public void AddCategory(QQCategory category)
        {
            _categoryMap[category.Index] = category;
        }

        public void AddGroup(QQGroup group)
        {
            _groupMap[group.Code] = group;
        }

        public void AddPicItem(IContentItem pictureItem)
        {
            _pictureItemList.Add(pictureItem);
        }

        public void AddDiscuz(QQDiscuz discuz)
        {
            _discuzMap[discuz.Did] = discuz;
        }

        public void DeleteBuddy(QQBuddy buddy)
        {
            _buddyMap.TryRemove(buddy.Uin, out buddy);
        }

        public void DeleteStranger(QQStranger stranger)
        {
            _strangerMap.TryRemove(stranger.Uin, out stranger);
        }

        public void DeleteCategory(QQCategory category)
        {
            _categoryMap.TryRemove(category.Index, out category);
        }

        public void DeleteGroup(QQGroup group)
        {
            _groupMap.TryRemove(group.Gin, out group);
        }

        public void DeletePicItem(IContentItem picItem)
        {
            _pictureItemList.Remove(picItem);
        }

        public void DeleteDiscuz(QQDiscuz discuz)
        {
            _discuzMap.TryRemove(discuz.Did, out discuz);
        }

        public QQBuddy GetBuddyByUin(long uin)
        {
            return _buddyMap.ContainsKey(uin) ? _buddyMap[uin] : null;
        }

        public QQBuddy GetBuddyByUinOrAdd(long uin)
        {
            return _buddyMap.GetOrAdd(uin, (key) => new QQBuddy {Uin = uin});
        }

        public QQStranger GetStrangerByUin(long uin)
        {
            return _strangerMap.ContainsKey(uin) ? _strangerMap[uin] : null;
        }

        public QQCategory GetCategoryByIndex(long index)
        {
            return _categoryMap.ContainsKey(index) ? _categoryMap[index] : null;
        }

        public QQGroup GetGroupByCode(long code)
        {
            return _groupMap.ContainsKey(code) ? _groupMap[code] : null;
        }

        public QQGroup GetGroupById(long id)
        {
            // return (from g in _groupMap where g.Value.Gid == id select g.Value).FirstOrDefault();
            return _groupMap.Where(g => g.Value.Gid == id).Select(g => g.Value).FirstOrDefault();
        }

        public QQGroup GetGroupByGin(long gin)
        {
            // return (from g in _groupMap where g.Value.Gin == gin select g.Value).FirstOrDefault();
            return _groupMap.Where(g => g.Value.Gin == gin).Select(g => g.Value).FirstOrDefault();
        }

        public QQDiscuz GetDiscuzByDid(long did)
        {
            return _discuzMap.ContainsKey(did) ? _discuzMap[did] : null;
        }

        public IEnumerable<QQBuddy> GetBuddyList()
        {
            return _buddyMap.Select(item => item.Value);
        }
        public int BuddyCount => _buddyMap.Count;

        public IEnumerable<QQStranger> GetStrangerList()
        {
            return _strangerMap.Select(item => item.Value);
        }
        public int StrangerCount => _strangerMap.Count;

        public IEnumerable<QQCategory> GetCategoryList()
        {
            return _categoryMap.Select(item => item.Value);
        }
        public int CategoryCount => _categoryMap.Count;

        public IEnumerable<QQGroup> GetGroupList()
        {
            return _groupMap.Select(item => item.Value);
        }
        public int GroupCount => _groupMap.Count;

        public IEnumerable<QQDiscuz> GetDiscuzList()
        {
            return _discuzMap.Select(item => item.Value);
        }
        public int DiscuzCount => _discuzMap.Count;

        public IEnumerable<QQBuddy> GetOnlineBuddyList()
        {
            return (from buddy in _buddyMap where QQStatus.IsGeneralOnline(buddy.Value.Status) select buddy.Value);
        }

        public List<IContentItem> GetPicItemList()
        {
            return _pictureItemList;
        }

        public int GetPicItemListSize()
        {
            return _pictureItemList.Count;
        }

        /// <summary>
        /// 查找临时会话用户 QQGroup/QQDiscuz/QQStranger
        /// </summary>
        /// <param name="uin"></param>
        /// <returns></returns>
        public QQUser SearchUserByUin(long uin)
        {
            var user = GetBuddyByUin(uin) ?? (QQUser)GetStrangerByUin(uin);
            if (user == null)
            {
                foreach (var group in _groupMap.Values)
                {
                    return group.Members.FirstOrDefault(u => u.Uin == uin);
                }
                foreach (var discuz in _discuzMap.Values)
                {
                    return discuz.Members.FirstOrDefault(u => u.Uin == uin);
                }
            }
            return user;
        }
    }
}
