using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Bean.Content
{
    [Serializable]
    public class FaceItem : IContentItem
    {
        /**	 * 表情的ID	 */
        public int Id { get; set; }

        public FaceItem() { }

        public FaceItem(string text)
        {
            FromJson(text);
        }

        public FaceItem(int id)
        {
            Id = id;
        }

        public ContentItemType Type => ContentItemType.Face;

        public object ToJson()
        {
            var json = new JArray();
            json.Add("face");
            json.Add(Id);
            return json;
        }
        public void FromJson(string text)
        {
            try
            {
                var json = JArray.Parse(text);
                Id = int.Parse(json[1].ToString());
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
            return $"[表情 {Id}]";
        }
    }

}
