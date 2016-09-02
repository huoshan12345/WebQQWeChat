using System;
using System.Collections.Generic;
using System.Text;
using iQQ.Net.WebQQCore.Im.Bean.Content;
using iQQ.Net.WebQQCore.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Bean
{

    public enum QQMsgType
    {
        BUDDY_MSG, 		//好友消息
        GROUP_MSG,		// 群消息
        DISCUZ_MSG,		//讨论组消息
        SESSION_MSG	//临时会话消息
    }

    /// <summary>
    /// QQ消息
    /// </summary>
    // [Serializable]
    public class QQMsg
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 暂时不知什么含义
        /// </summary>
        public long Id2 { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public QQMsgType Type { get; set; }
        /// <summary>
        /// 消息接收方
        /// </summary>
        public QQUser To { get; set; }
        /// <summary>
        /// 消息发送方
        /// </summary>
        public QQUser From { get; set; }
        /// <summary>
        /// 所在群
        /// </summary>
        public QQGroup Group { get; set; }
        /// <summary>
        /// 讨论组
        /// </summary>
        public QQDiscuz Discuz { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 消息列表
        /// </summary>
        public List<IContentItem> ContentList { get; set; } = new List<IContentItem>();

        public string PackContentList()
        {
            // ["font",{"size":10,"color":"808080","style":[0,0,0],"name":"\u65B0\u5B8B\u4F53"}]
            var json = new JArray();
            foreach (var contentItem in ContentList)
            {
                json.Add(contentItem.ToJson());
            }
            return JsonConvert.SerializeObject(json);
        }


        //public void ParseContentList(JToken jToken)
        //{
        //    /*
        //        [
        //            [
        //                "font",
        //                {
        //                    "color": "000000",
        //                    "name": "微软雅黑",
        //                    "size": 10,
        //                    "style": [
        //                        0,
        //                        0,
        //                        0
        //                    ]
        //                }
        //            ],
        //            "d"
        //        ]
        //    */
        //    var array = jToken as JArray;
        //    if (array != null)
        //    {
        //        foreach (var t in array)
        //        {
        //            if (t is JArray)
        //            {
        //                var items = t.ToObject<JArray>();
        //                var contentItemType = ContentItemType.ValueOfRaw(items[0].ToString());
        //                var item = JsonConvert.SerializeObject(items);
        //                switch (contentItemType.Name.ToUpper())
        //                {
        //                    case "FACE": AddContentItem(new FaceItem(item)); break;
        //                    case "FONT": AddContentItem(new FontItem(item)); break;
        //                    case "CFACE": AddContentItem(new CFaceItem(item)); break;
        //                    case "OFFPIC": AddContentItem(new OffPicItem(item)); break;

        //                    default:
        //                    break;
        //                }
        //            }
        //            else
        //            {
        //                AddContentItem(new TextItem(JsonConvert.SerializeObject(t)));
        //            }
        //        }
        //    }
        //    else
        //    {
        //        throw new QQException(QQErrorCode.JSON_ERROR);
        //    }
        //}

        public void ParseContentList(string text)
        {
            try
            {
                var json = JArray.Parse(text);
                foreach (var t in json)
                {
                    if (t is JArray)
                    {
                        var items = t.ToObject<JArray>();
                        var contentItemType = ContentItemType.ValueOfRaw(items[0].ToString());
                        var item = JsonConvert.SerializeObject(items);
                        switch (contentItemType.Name.ToUpper())
                        {
                            case "FACE":
                            AddContentItem(new FaceItem(item));
                            break;

                            case "FONT":
                            AddContentItem(new FontItem(item));
                            break;

                            case "CFACE":
                            AddContentItem(new CFaceItem(item));
                            break;

                            case "OFFPIC":
                            AddContentItem(new OffPicItem(item));
                            break;

                            default:
                            break;
                        }
                    }
                    else
                    {
                        AddContentItem(new TextItem(JsonConvert.SerializeObject(t)));
                    }
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

        public void AddContentItem(IContentItem contentItem)
        {
            ContentList.Add(contentItem);
        }

        public void DeleteContentItem(IContentItem contentItem)
        {
            ContentList.Remove(contentItem);
        }

        public override string ToString()
        {
            return PackContentList();
        }

        public string GetText()
        {
            var buffer = new StringBuilder();
            foreach (var item in ContentList)
            {
                buffer.Append(item.ToText());
            }
            return buffer.ToString();
        }

        public void ClearContentItems()
        {
            ContentList.Clear();
        }
    }
}
