using Newtonsoft.Json;

namespace WebQQ.Im.Bean.Group
{
    public class GroupMessage : Message
    {
        [JsonIgnore]
        public QQGroup Group { get; set; }

        public GroupMessage()
        {
            Type = MessageType.Group;
        }
    }
}
