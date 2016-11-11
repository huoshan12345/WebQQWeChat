using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebQQ.Im.Bean
{
    public class QQUser
    {        
        /// <summary>
        /// 头像
        /// </summary>
        public int Face { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public long Uin { get; set; }

        /// <summary>
        /// 个性签名
        /// </summary>
        [JsonProperty("lnick")]
        public string LongNick { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string Nick { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string MarkName { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 职业
        /// </summary>
        public string Occupation { get; set; }

        /// <summary>
        /// 对方加好友验证请求设置
        /// </summary>
        public AllowType Allow { get; set; }


        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 账户（目前是qq号）
        /// </summary>
        public long Account { get; set; }


        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 个人说明
        /// </summary>
        public string Personal { get; set; }

        /// <summary>
        /// 生肖
        /// </summary>
        public int Shengxiao { get; set; }

        /// <summary>
        /// 血型
        /// </summary>
        public int Blood { get; set; }

        /// <summary>
        /// 星座
        /// </summary>
        public int Constel { get; set; }


        public string Vfwebqq { get; set; }

        /// <summary>
        /// 个人主页
        /// </summary>
        public string Homepage { get; set; }

        /// <summary>
        /// VIP等级
        /// </summary>
        [JsonProperty("vip_info")]
        public int VipInfo { get; set; }

        /// <summary>
        /// 毕业院校
        /// </summary>
        public string College { get; set; }

        public DateTime BirthdayDate { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        private Birthday _birthday;
        public Birthday Birthday
        {
            get { return _birthday; }
            set
            {
                _birthday = value;
                BirthdayDate = new DateTime(_birthday.Year, _birthday.Month, _birthday.Day);
            }
        }
    }

    public class Birthday
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int Day { get; set; }
    }

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
