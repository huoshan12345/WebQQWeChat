namespace WebQQ.Im.Bean
{
    /// <summary>
    /// 群成员
    /// </summary>
    // 
    public class QQGroupMember : QQStranger
    {
        public Group Group { get; set; }

        public string Card { get; set; }
    }
}
