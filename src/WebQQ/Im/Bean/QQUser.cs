using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebQQ.Im.Bean
{
    public class QQUser : SelfInfo
    {
        public QQStatusType Status { get; set; }

        public override Birthday Birthday
        {
            get { return _birthday; }
            set
            {
                _birthday = value;
                BirthdayDate = new DateTime(_birthday.Year, _birthday.Month, _birthday.Day);
            }
        }

        public DateTime BirthdayDate { get; set; }
        private Birthday _birthday;
    }
}
