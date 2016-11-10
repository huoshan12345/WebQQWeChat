using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebQQ.Im.Bean
{
    public enum MessageType
    {
        Buddy, 		    //好友消息
        Group,		    // 群消息
        Discussion,		//讨论组消息
        Session         //临时会话消息
    }

    public class Message
    {
        public MessageType Type { get; set; }


    }
}
