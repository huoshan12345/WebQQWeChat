using System;

namespace iQQ.Net.WebQQCore.Im.Bean.Content
{
    [Serializable]
    public class TextItem : ContentItem
    {
        private string content;
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        public TextItem() { }

        public TextItem(string text)
        {
            FromJson(text);
        }

        public ContentItemType Type
        {
            get { return ContentItemType.TEXT; }
        }

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
