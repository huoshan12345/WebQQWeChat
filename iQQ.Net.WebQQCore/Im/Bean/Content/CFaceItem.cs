using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Bean.Content
{
    /// <summary>
    /// 群图片
    /// </summary>
    [Serializable]
    public class CFaceItem : ContentItem
    {
        private bool isSuccess;
        public bool IsSuccess
        {
            get { return isSuccess; }
            set { isSuccess = value; }
        }

        private long fileId;
        public long FileId
        {
            get { return fileId; }
            set { fileId = value; }
        }

        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        private string key;
        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        private string server;
        public string Server
        {
            get { return server; }
            set { server = value; }
        }

        public CFaceItem() { }

        public CFaceItem(string text)
        {
            FromJson(text);
        }


        public ContentItemType Type { get { return ContentItemType.CFACE; } }

        public object ToJson()
        {
            // [\"cface\",\"group\",\"5F7E31F0001EF4310865F1FF4549B12B.jPg\"]
            JArray json = new JArray();
            json.Add("cface");
            json.Add("group");
            json.Add(fileName);
            return json;
        }
        public void FromJson(string text)
        {
            try
            {
                JArray json = JArray.Parse(text);
                if (json[1].Type == JTokenType.String)
                {
                    this.FileName = json[1].ToString();
                }
                else
                {
                    JObject pic = (JObject) json[1];
                    this.FileName = pic["name"].ToString();
                    this.FileId = long.Parse(pic["file_id"].ToString());
                    this.Key = pic["key"].ToString();
                    this.Server = pic["server"].ToString();
                }
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
            return "[群图片]";
        }
    }

}
