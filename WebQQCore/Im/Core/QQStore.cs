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
        private readonly Dictionary<long, QQBuddy> _buddyMap; // uin => QQBudy, 快速通过uin查找QQ好友
        private readonly Dictionary<long, QQStranger> _strangerMap; // uin => QQStranger, 快速通过uin查找临时会话用户
        private readonly Dictionary<long, QQCategory> _categoryMap; // index => QQCategory
        private readonly Dictionary<long, QQDiscuz> _discuzMap;		// did = > QQDiscuz
        private readonly Dictionary<long, QQGroup> _groupMap; // code => QQGroup, 快速通过群ID查找群
        private readonly List<IContentItem> _pictureItemList; // filename -> PicItem 上传图片列表

        /**
         * <p>Constructor for QQStore.</p>
         */
        public QQStore()
        {
            this._buddyMap = new Dictionary<long, QQBuddy>();
            this._strangerMap = new Dictionary<long, QQStranger>();
            this._categoryMap = new Dictionary<long, QQCategory>();
            this._groupMap = new Dictionary<long, QQGroup>();
            this._discuzMap = new Dictionary<long, QQDiscuz>();
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
            _buddyMap.Remove(buddy.Uin);
        }

        public void DeleteStranger(QQStranger stranger)
        {
            _strangerMap.Remove(stranger.Uin);
        }

        public void DeleteCategory(QQCategory category)
        {
            _categoryMap.Remove(category.Index);
        }

        public void DeleteGroup(QQGroup group)
        {
            _groupMap.Remove(group.Gin);
        }

        public void DeletePicItem(IContentItem picItem)
        {
            _pictureItemList.Remove(picItem);
        }

        public void DeleteDiscuz(QQDiscuz discuz)
        {
            _discuzMap.Remove(discuz.Did);
        }

        public QQBuddy GetBuddyByUin(long uin)
        {
            return _buddyMap.ContainsKey(uin) ? _buddyMap[uin] : null;
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
            return (from g in _groupMap where g.Value.Gid == id select g.Value).FirstOrDefault();
        }

        public QQGroup GetGroupByGin(long gin)
        {
            return (from g in _groupMap where g.Value.Gin == gin select g.Value).FirstOrDefault();
        }

        public QQDiscuz GetDiscuzByDid(long did)
        {
            return _discuzMap.ContainsKey(did) ? _discuzMap[did] : null;
        }

        public List<QQBuddy> GetBuddyList()
        {
            return _buddyMap.Values.ToList();
        }

        public List<QQStranger> GetStrangerList()
        {
            return _strangerMap.Values.ToList();
        }

        public List<QQCategory> GetCategoryList()
        {
            return _categoryMap.Values.ToList();
        }

        public List<QQGroup> GetGroupList()
        {
            return _groupMap.Values.ToList();
        }

        public List<QQDiscuz> GetDiscuzList()
        {
            return _discuzMap.Values.ToList();
        }

        public List<QQBuddy> GetOnlineBuddyList()
        {
            return (from buddy in _buddyMap where QQStatus.IsGeneralOnline(buddy.Value.Status) select buddy.Value).ToList();
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
