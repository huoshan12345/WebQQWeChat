using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Base;

namespace Domain.Entities
{
    public class AccountLogin : Entity
    {
        public int AccountId { get; set; }
        public DateTime LoginTime { get; set; }
    }
}
