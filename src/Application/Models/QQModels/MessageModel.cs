using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WebQQ.Im.Bean;
using WebQQ.Im.Bean.Friend;

namespace Application.Models.QQModels
{
    public class MessageModel
    {
        [Required]
        public MessageType Type { get; set; }

        [Required]
        public string Text { get; set; }

        public string UserName { get; set; }
        public long UserUin { get; set; }
    }
}
