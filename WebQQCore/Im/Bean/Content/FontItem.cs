using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Bean.Content
{
    [Serializable]
    public class FontItem : IContentItem
    {
        private string name = "宋体";

        public int Size { get; set; } = 12;

        public bool Bold { get; set; }

        public bool Underline { get; set; }

        public bool Italic { get; set; }

        public int Color { get; set; } = 0;

        public FontItem() { }

        public FontItem(string text)
        {
            FromJson(text);
        }

        public ContentItemType Type => ContentItemType.Font;

        public object ToJson()
        {
            // ["font",{"size":10,"color":"808080","style":[0,0,0],"name":"\u65B0\u5B8B\u4F53"}]
            try
            {
                var json = new JArray();
                json.Add("font");
                var font = new JObject();
                font.Add("size", Size);
                font.Add("color", string.Format("%06x", Color));
                var style = new JArray();
                style.Add(Bold ? 1 : 0);
                style.Add(Italic ? 1 : 0);
                style.Add(Underline ? 1 : 0);
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
                var json = JArray.Parse(text);
                var font = json[1].ToObject<JObject>();
                Size = font["size"].ToObject<int>();
                Color = int.Parse(font["color"].ToString(), NumberStyles.HexNumber);

                var style = font["style"].ToObject<JArray>();
                Bold = style[0].ToObject<int>() == 1 ? true : false;
                Italic = style[1].ToObject<int>() == 1 ? true : false;
                Underline = style[2].ToObject<int>() == 1 ? true : false;
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
