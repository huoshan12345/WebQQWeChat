using System.Collections.Concurrent;
using Newtonsoft.Json;
using FclEx.Extensions;

namespace WebQQ.Im.Bean.Group
{

    public class QQGroup : GroupInfo
    {
        /// <summary>
        /// 备注
        /// </summary>
        public string MarkName { get; set; }

        [JsonIgnore]
        public ConcurrentDictionary<long, GroupMember> Members { get; } = new ConcurrentDictionary<long, GroupMember>();

        public string ShowName => MarkName.IsNullOrEmpty() ? Name : MarkName;

        public override string ToString()
        {
            return "QQGroup [gid=" + Gid + ", code=" + Code + ", name=" + Name + "]";
        }
    }
}
