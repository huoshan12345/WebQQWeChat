using Newtonsoft.Json;
using WebQQ.Im.Bean.Content;

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

        public FriendMessage(QQFriend friend, string text) : this()
        {
            Friend = friend;
            Contents.Add(new TextItem(text));
            Contents.Add(new FontItem());
        }
    }
}
