using System;
using Newtonsoft.Json;

namespace WebQQ.Im.Bean.Friend
{
    public class FriendInfo
    {
        public int Face { get; set; }
        public string Occupation { get; set; }
        public string Phone { get; set; }
        public AllowType Allow { get; set; }
        public string College { get; set; }
        public long Uin { get; set; }
        public int Constel { get; set; }
        public int Blood { get; set; }
        public string Homepage { get; set; }

        [JsonProperty("stat")]
        public QQStatusType Status { get; set; }

        [JsonProperty("vip_info")]
        public int VipInfo { get; set; }

        public string Country { get; set; }
        public string City { get; set; }
        public string Personal { get; set; }
        public string Nick { get; set; }
        public int Shengxiao { get; set; }
        public string Email { get; set; }
        public string Province { get; set; }
        public string Gender { get; set; }
        public string Mobile { get; set; }

        private Birthday _birthday;
        public Birthday Birthday
        {
            get { return _birthday; }
            set
            {
                _birthday = value;
                if (_birthday.Year != 0 && _birthday.Month != 0 && _birthday.Day != 0)
                {
                    BirthdayDate = new DateTime(_birthday.Year, _birthday.Month, _birthday.Day);
                }
            }
        }
        public DateTime BirthdayDate { get; set; }
    }
}
