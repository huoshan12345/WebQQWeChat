using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Core;
using FxUtility.Extensions;

namespace WebQQ.Im.Bean.Content
{

    public class FontItem : IContentItem
    {
        public string Color { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public int[] Style { get; set; }
        
        public bool IsBold() => Style?[0] != 0;         // 加粗
        public bool IsItalic() => Style?[1] != 0;       // 斜体
        public bool IsUnderline() => Style?[2] != 0;    // 下划线

        [JsonIgnore]
        public ContentItemType Type => ContentItemType.Font;

        public object ToJson()
        {
            return new JArray { Type.ToLowerString(), this };
        }

        public string GetText()
        {
            return string.Empty;
        }
    }

}
