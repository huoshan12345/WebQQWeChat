using System.Collections.Concurrent;
using Newtonsoft.Json;
using WebQQ.Im.Bean.Group;

namespace WebQQ.Im.Bean.Discussion
{
    /// <summary>
    /// QQ讨论组
    /// </summary>
    
    public class QQDiscussion
    {
        /// <summary>
        /// 讨论组ID，每次登陆都固定，视为没有变换
        /// </summary>
        public long Did { get; set; }
        /// <summary>
        /// 讨论组的名字
        /// </summary>
        public string Name { get; set; }

        [JsonIgnore]
        public ConcurrentDictionary<long, DiscussionMember> Members { get; } = new ConcurrentDictionary<long, DiscussionMember>();
    }
}
