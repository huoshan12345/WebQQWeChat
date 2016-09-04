namespace iQQ.Net.WebQQCore.Im.Bean.Content
{
    public interface IContentItem
    {
        ContentItemType Type { get; }
        object ToJson();
        void FromJson(string text);
        string ToText();
    }

    public sealed class ContentItemType
    {
        /** 字体 */
        public static readonly ContentItemType Font = new ContentItemType("font");
        public static readonly ContentItemType Text = new ContentItemType("text");/** 文字 */
        public static readonly ContentItemType Face = new ContentItemType("face");/** 表情 */
        public static readonly ContentItemType Offpic = new ContentItemType("offpic");/** 离线图片 */
        public static readonly ContentItemType Cface = new ContentItemType("cface");/** 群图片 */

        public string Name { get; set; }

        ContentItemType(string name)
        {
            Name = name;
        }

        public static ContentItemType ValueOfRaw(string txt)
        {
            switch (txt)
            {
                case "font": return Font;
                case "face": return Face;
                case "offpic": return Offpic;
                case "cface": return Cface;
                default: return Text;
            }
        }

    }
}
