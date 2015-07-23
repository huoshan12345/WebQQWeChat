using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Bean.Content
{
    [Serializable]
    public class FaceItem : ContentItem
    {
        /**	 * 表情的ID	 */
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public FaceItem() { }

        public FaceItem(string text)
        {
            FromJson(text);
        }

        public FaceItem(int id)
        {
            this.id = id;
        }

        public ContentItemType Type { get { return ContentItemType.FACE; } }

        public object ToJson()
        {
            JArray json = new JArray();
            json.Add("face");
            json.Add(id);
            return json;
        }
        public void FromJson(string text)
        {
            try
            {
                JArray json = JArray.Parse(text);
                id = int.Parse(json[1].ToString());
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
            return string.Format("[表情 {0}]", Id);
        }
    }

}
