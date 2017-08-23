using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Core;
using FclEx.Extensions;
using WebQQ.Util;

namespace WebQQ.Im.Bean.Content
{
    public class FontItem : IContentItem
    {
        public string Name { get; set; } = "宋体";
        public int Size { get; set; } = 12;
        public int[] Style { get; set; } = { 0, 0, 0 };
        public string Color { get; set; } = 0.ToString("d6");

        public bool IsBold() => Style?[0] != 0;         // 加粗
        public bool IsItalic() => Style?[1] != 0;       // 斜体
        public bool IsUnderline() => Style?[2] != 0;    // 下划线

        [JsonIgnore]
        public ContentItemType Type => ContentItemType.Font;

        public object ToJson()
        {
            return new JArray { Type.ToString().ToLower(), JObject.FromObject(this, Helpers.JsonCamelSerializer) };
        }

        public string GetText()
        {
            return string.Empty;
        }
    }

}
