using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebQQ.Im.Bean.Content
{
    /// <summary>
    /// 群图片
    /// </summary>
    
    public class CFaceItem : IContentItem
    {
        public bool IsSuccess { get; set; }

        public long FileId { get; set; }

        public string FileName { get; set; }

        public string Key { get; set; }

        public string Server { get; set; }

        public CFaceItem() { }

        public CFaceItem(string text)
        {
            FromJson(text);
        }


        public ContentItemType Type => ContentItemType.Cface;

        public object ToJson()
        {
            // [\"cface\",\"group\",\"5F7E31F0001EF4310865F1FF4549B12B.jPg\"]
            var json = new JArray();
            json.Add("cface");
            json.Add("group");
            json.Add(FileName);
            return json;
        }
        public void FromJson(string text)
        {
            try
            {
                var json = JArray.Parse(text);
                if (json[1].Type == JTokenType.String)
                {
                    FileName = json[1].ToString();
                }
                else
                {
                    var pic = (JObject) json[1];
                    FileName = pic["name"].ToString();
                    FileId = long.Parse(pic["file_id"].ToString());
                    Key = pic["key"].ToString();
                    Server = pic["server"].ToString();
                }
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
            return "[群图片]";
        }
    }

}
