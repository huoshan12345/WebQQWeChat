namespace iQQ.Net.WebQQCore.Im.Bean.Content
{
    public interface ContentItem
    {
        ContentItemType Type { get; }
        object ToJson();
        void FromJson(string text);
        string ToText();
    }

    public sealed class ContentItemType
    {
        /** 字体 */
        public static readonly ContentItemType FONT = new ContentItemType("font");
        public static readonly ContentItemType TEXT = new ContentItemType("text");/** 文字 */
        public static readonly ContentItemType FACE = new ContentItemType("face");/** 表情 */
        public static readonly ContentItemType OFFPIC = new ContentItemType("offpic");/** 离线图片 */
        public static readonly ContentItemType CFACE = new ContentItemType("cface");/** 群图片 */

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        ContentItemType(string name)
        {
            this.name = name;
        }

        public static ContentItemType ValueOfRaw(string txt)
        {
            if (txt.Equals("font"))
            {
                return FONT;
            }
            else if (txt.Equals("face"))
            {
                return FACE;
            }
            else if (txt.Equals("offpic"))
            {
                return OFFPIC;
            }
            else if (txt.Equals("cface"))
            {
                return CFACE;
            }
            else
            {
                return TEXT;
            }
        }

    }
}
