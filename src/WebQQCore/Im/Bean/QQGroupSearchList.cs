using System.Collections.Generic;

namespace iQQ.Net.WebQQCore.Im.Bean
{
    public sealed class SearchType
    {
        public static readonly SearchType QQGROUPSEARCH_TEXT; // 0表示QQ号码
        public static readonly SearchType QQGROUPSEARCH_KEY;// 1表示关键字
    }

    public class QQGroupSearchList
    {
        public SearchType SearchType { get; set; } = SearchType.QQGROUPSEARCH_TEXT;

        /// <summary>
        /// 页数
        /// </summary>
        public int PageNum { get; set; }

        /// <summary>
        /// 当前页数
        /// </summary>
        public int CurrentPage { get; set; } = 1;

        /// <summary>
        /// 一页的信息量
        /// </summary>
        public int PageSize { get; set; } = 10;
        /// <summary>
        /// 搜索的关键字
        /// </summary>
        public string KeyStr { get; set; }

        /// <summary>
        /// 是否要验证码
        /// </summary>
        public bool NeedVfcode { get; set; } = false;
        /// <summary>
        /// 验证码数据
        /// </summary>
        public string VfCode { get; set; }
        /// <summary>
        /// 搜索出的群集合
        /// </summary>
        public List<QQGroupSearchInfo> GroupsResult { get; set; } = new List<QQGroupSearchInfo>();
    }
}
