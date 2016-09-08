using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Bean.Content
{
    
    public class OffPicItem : IContentItem
    {
        public bool IsSuccess { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public int FileSize { get; set; }

        public OffPicItem() { }

        public OffPicItem(string text)
        {
            FromJson(text);
        }

        public ContentItemType Type => ContentItemType.Offpic;

        public object ToJson()
        {
            // [\"offpic\",\"/27d736df-2a59-4007-8701-7218bc70898d\",\"Beaver.bmp\",14173]
            var json = new JArray();
            json.Add("offpic");
            json.Add(FilePath);
            json.Add(FileName);
            json.Add(FileSize);
            return json;
        }

        public void FromJson(string text)
        {
            // ["offpic",{"success":1,"file_path":"/7acccf74-0fcd-4bbd-b885-03a5cc2f7507"}]
            try
            {
                var json = JArray.Parse(text);
                var pic = (JObject)json[1];
                IsSuccess = int.Parse(pic["success"].ToString()) == 1 ? true : false;
                FilePath = pic["file_path"].ToString();
            }
            catch (JsonException e)
            {
                throw new QQException(QQErrorCode.JsonError, e);
            }
            catch (Exception e)
            {
                throw new QQException(QQErrorCode.UnknownError, e);
            }
        }


        public string ToText()
        {
            return "[图片]";
        }
    }
}
