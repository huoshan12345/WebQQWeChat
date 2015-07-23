using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Bean.Content
{
    [Serializable]
    public class FontItem : ContentItem
    {
        private string name = "宋体";

        private int size = 12;
        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        private bool bold;
        public bool Bold
        {
            get { return bold; }
            set { bold = value; }
        }

        private bool underline;
        public bool Underline
        {
            get { return underline; }
            set { underline = value; }
        }

        private bool italic;
        public bool Italic
        {
            get { return italic; }
            set { italic = value; }
        }

        private int color = 0;
        public int Color
        {
            get { return color; }
            set { color = value; }
        }

        public FontItem() { }

        public FontItem(string text)
        {
            FromJson(text);
        }

        public ContentItemType Type
        {
            get { return ContentItemType.FONT; }
        }

        public object ToJson()
        {
            // ["font",{"size":10,"color":"808080","style":[0,0,0],"name":"\u65B0\u5B8B\u4F53"}]
            try
            {
                JArray json = new JArray();
                json.Add("font");
                JObject font = new JObject();
                font.Add("size", size);
                font.Add("color", string.Format("%06x", color));
                JArray style = new JArray();
                style.Add(bold ? 1 : 0);
                style.Add(italic ? 1 : 0);
                style.Add(underline ? 1 : 0);
                font.Add("style", style);
                font.Add("name", name);
                json.Add(font);
                return json;
            }
            catch (JsonException e)
            {
                throw new QQException(QQErrorCode.JSON_ERROR, e);
            }
        }

        public void FromJson(string text)
        {
            try
            {
                JArray json = JArray.Parse(text);
                JObject font = json[1].ToObject<JObject>();
                size = font["size"].ToObject<int>();
                color = int.Parse(font["color"].ToString(), NumberStyles.HexNumber);

                JArray style = font["style"].ToObject<JArray>();
                bold = style[0].ToObject<int>() == 1 ? true : false;
                italic = style[1].ToObject<int>() == 1 ? true : false;
                underline = style[2].ToObject<int>() == 1 ? true : false;
                name = font["name"].ToString();
            }
            catch (JsonException e)
            {
                throw new QQException(QQErrorCode.JSON_ERROR, e);
            }
            catch (Exception e)
            {
                throw new QQException(QQErrorCode.UNKNOWN_ERROR, e);
            }
        }

        public string ToText()
        {
            return string.Empty;
        }
    }
}
