namespace WebQQ.Im.Bean.Content
{
    public interface IContentItem
    {
        ContentItemType Type { get; }
        object ToJson();
        void FromJson(string text);
        string ToText();
    }


    public enum ContentItemType
    {
        Font,
        Face,
        Offpic,
        Cface,
        Text
    }

    public sealed class ContentItemTypeInfo
    {
        public static ContentItemType ValueOfRaw(string txt)
        {
            switch (txt.ToLower())
            {
                case "font": return ContentItemType.Font;
                case "face": return ContentItemType.Face;
                case "offpic": return ContentItemType.Offpic;
                case "cface": return ContentItemType.Cface;
                default: return ContentItemType.Text;
            }
        }

    }
}
