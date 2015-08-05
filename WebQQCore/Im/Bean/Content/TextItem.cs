using System;

namespace iQQ.Net.WebQQCore.Im.Bean.Content
{
    [Serializable]
    public class TextItem : IContentItem
    {
        public string Content { get; set; }

        public TextItem() { }

        public TextItem(string text)
        {
            FromJson(text);
        }

        public ContentItemType Type => ContentItemType.Text;

        public object ToJson()
        {
            return Content;
        }

        public void FromJson(string text)
        {
            Content = text;
        }

        public string ToText()
        {
            return Content;
        }
    }

}
