using System.Collections.Generic;
using System.Linq;

namespace WebQQ.Im.Bean
{
    /// <summary>
    /// QQ讨论组
    /// </summary>
    
    public class Discussion
    {
        /// <summary>
        /// 讨论组ID，每次登陆都固定，视为没有变换
        /// </summary>
        public long Did { get; set; }
        /// <summary>
        /// 讨论组的名字
        /// </summary>
        public string Name { get; set; }

        ///// <summary>
        ///// 创建者的UIN
        ///// </summary>
        //public long Owner { get; set; }
    }
}
