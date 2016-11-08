using System;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebQQ.Util;

namespace WebQQ.Im.Bean
{

    /// <summary>
    /// QQ普通用户，保存了所有用户的基本信息
    /// </summary>
    // 
    public class QQUser
    {
        /// <summary>
        /// 标识
        /// </summary>
        public long Uin { get; set; }

        /// <summary>
        /// QQ号
        /// </summary>
        public long QQ { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public QQStatusType Status { get; set; } = QQStatusType.Offline;

        /// <summary>
        /// 客户类型
        /// </summary>
        public QQClientType ClientType { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public QQLevel LevelInfo { get; set; } = new QQLevel();

        /// <summary>
        /// 登录时间
        /// </summary>
        public long LoginDate { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 个性签名
        /// </summary>
        public string Sign { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 毕业院校
        /// </summary>
        public string College { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public int RegTime { get; set; }

        /// <summary>
        /// 星座
        /// </summary>
        public int Constel { get; set; }

        /// <summary>
        /// 血型
        /// </summary>
        public int Blood { get; set; }

        /// <summary>
        /// 个人主页
        /// </summary>
        public string Homepage { get; set; }

        /// <summary>
        /// 统计
        /// </summary>
        public int Stat { get; set; }

        /// <summary>
        /// 是否为VIP
        /// </summary>
        public bool IsVip { get; set; }

        /// <summary>
        /// VIP等级
        /// </summary>
        public int VipLevel { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 个人说明
        /// </summary>
        public string Personal { get; set; }

        /// <summary>
        /// 职业
        /// </summary>
        public string Occupation { get; set; }

        /// <summary>
        /// 生肖
        /// </summary>
        public int ChineseZodiac { get; set; }

        public int Flag { get; set; }

        public int Cip { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public int Face { get; set; }

        /// <summary>
        /// 对方加好友验证请求设置
        /// </summary>
        public QQAllow Allow { get; set; }

        public override string ToString() => $"QQUser [qq={QQ}, nickname={Nickname}, status={Status}]";
    }
}
