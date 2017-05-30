using Newtonsoft.Json;
using WebQQ.Im.Bean.Content;

namespace WebQQ.Im.Bean.Group
{
    public class GroupMessage : Message
    {
        [JsonIgnore]
        public GroupMember User { get; set; }

        [JsonIgnore]
        public QQGroup Group { get; set; }

        public GroupMessage()
        {
            Type = MessageType.Group;
        }

        public GroupMessage(QQGroup group, string text) : this()
        {
            Group = group;
            Contents.Add(new TextItem(text));
            Contents.Add(new FontItem());
        }
    }
}
