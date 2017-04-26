using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Base;

namespace Domain.Entities
{
    public enum EnumAccountType
    {
        Twitter = 1,
        Facebook,
        Instagram,
        QQ,
        Naver,
        Melon,
        WeChat
    }

    public class AccountInfo : AggregateRoot
    {
        public int OwnerId { get; set; }
        public string Username { get; set; }
        public EnumAccountType AccountType { get; set; }
    }
}
