using System;
using FclEx.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Core;

namespace WebQQ.Im.Bean.Content
{
    
    public class FaceItem : IContentItem
    {
        public int Id { get; set; }

        public FaceItem(int id)
        {
            Id = id;
        }

        public ContentItemType Type => ContentItemType.Face;

        public object ToJson()
        {
            return new JArray { Type.ToString().ToLower(), this };
        }

        public string GetText()
        {
            return $"[表情 {Id}]";
        }
    }

}
