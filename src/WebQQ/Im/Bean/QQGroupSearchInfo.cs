namespace WebQQ.Im.Bean
{
    /// <summary>
    /// 用于存储搜索群的结果
    /// </summary>
    
    public class QQGroupSearchInfo
    {
        /// <summary>
        /// 群名;
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 群ID
        /// </summary>
        public long GroupId { get; set; }
        /// <summary>
        /// 群别名ID,用于协议部分
        /// </summary>
        public long GroupAliseId { get; set; }
        /// <summary>
        /// 群等级
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 所有者QQ号
        /// </summary>
        public long OwerId { get; set; }
        /// <summary>
        /// 创建的日期(时间戳)
        /// </summary>
        public long CreateTimeStamp { get; set; }
    }
}
