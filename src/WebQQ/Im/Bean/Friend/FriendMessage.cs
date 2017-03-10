using Newtonsoft.Json;

namespace WebQQ.Im.Bean.Friend
{
    public class FriendMessage : Message
    {
        public FriendMessage()
        {
            Type = MessageType.Friend;
        }

        [JsonIgnore]
        public QQFriend Friend { get; set; }
    }
}
