using Newtonsoft.Json;

namespace WebQQ.Im.Bean.Discussion
{
    public class DiscussionMessage : Message
    {
        public DiscussionMessage()
        {
            Type = MessageType.Discussion;
        }

        [JsonIgnore]
        public QQDiscussion Discussion { get; set; }
    }
}
