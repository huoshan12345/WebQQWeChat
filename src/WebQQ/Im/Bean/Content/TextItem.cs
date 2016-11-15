using Newtonsoft.Json.Linq;

namespace WebQQ.Im.Bean.Content
{

    public class TextItem : IContentItem
    {
        public string Content { get; }

        public TextItem(string text)
        {
            Content = text;
        }

        public ContentItemType Type => ContentItemType.Text;

        public object ToJson()
        {
            return Content;
        }

        public string GetText()
        {
            return Content;
        }
    }
}
