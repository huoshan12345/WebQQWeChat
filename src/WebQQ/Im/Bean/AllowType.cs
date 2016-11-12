using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebQQ.Im.Bean
{
    /// <summary>
    /// 对方设置的加好友策略
    /// </summary>
    public enum AllowType
    {
        /// <summary>允许所有人添加</summary>
        AllowAll = 0, // 0

        /// <summary>需要验证信息</summary>
        NeedConfirm, // 1

        /// <summary>拒绝任何人加好友</summary>
        RefuseAll, // 2

        /// <summary>需要回答问题</summary>
        NeedAnswer, // 3

        /// <summary>需要验证和回答问题</summary>
        NeedAnswerAndConfirm, // 4
    }
}
