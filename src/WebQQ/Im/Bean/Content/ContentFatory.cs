using System;
using System.Collections.Generic;
using FclEx.Extesions;
using Newtonsoft.Json.Linq;
using WebQQ.Util;

namespace WebQQ.Im.Bean.Content
{
    public static class ContentFatory
    {
        public static IContentItem CreateContentItem(JToken token)
        {
            var array = token as JArray;
            if (array != null)
            {
                var type = array[0].ToEnum<ContentItemType>();
                switch (type)
                {
                    case ContentItemType.Font: return array[1].ToObject<FontItem>();
                    case ContentItemType.Face: return new FaceItem(array[1].ToInt());
                    default: throw new ArgumentOutOfRangeException();
                }
            }
            else return new TextItem(token.ToString());
        }

        public static List<IContentItem> ParseContents(JArray token)
        {
            var list = new List<IContentItem>(token.Count);
            foreach (var t in token)
            {
                list.Add(CreateContentItem(t));
            }
            return list;
        }
    }
}
