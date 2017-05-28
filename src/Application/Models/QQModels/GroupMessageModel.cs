using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Models.QQModels
{
    public class GroupMessageModel
    {
        public long FromUin { get; set; }
        public int MsgID { get; set; }
        public int MsgType { get; set; }
        public long Time { get; set; }
        public long ToUin { get; set; }
        public long GroupCode { get; set; }
        public long SendUin { get; set; }
        public string Text { get; set; }
    }
}
