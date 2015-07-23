using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Bean.Content
{
    [Serializable]
    public class OffPicItem : ContentItem
    {
        private bool isSuccess;
        public bool IsSuccess
        {
            get { return isSuccess; }
            set { isSuccess = value; }
        }

        private string filePath;
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        private int fileSize;
        public int FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }

        public OffPicItem() { }

        public OffPicItem(string text)
        {
            FromJson(text);
        }

        public ContentItemType Type
        {
            get { return ContentItemType.OFFPIC; }
        }

        public object ToJson()
        {
            // [\"offpic\",\"/27d736df-2a59-4007-8701-7218bc70898d\",\"Beaver.bmp\",14173]
            JArray json = new JArray();
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
                JArray json = JArray.Parse(text);
                JObject pic = (JObject)json[1];
                IsSuccess = int.Parse(pic["success"].ToString()) == 1 ? true : false;
                FilePath = pic["file_path"].ToString();
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
            return "[图片]";
        }
    }
}
