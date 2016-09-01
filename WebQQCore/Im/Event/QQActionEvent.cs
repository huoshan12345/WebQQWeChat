using System;
using System.ComponentModel;
using System.Reflection;
using iQQ.Net.WebQQCore.Util.Extensions;

namespace iQQ.Net.WebQQCore.Im.Event
{
    [Description("QQ事件类型")]
    public enum QQActionEventType
    {
        [Description("成功")]
        EVT_OK,
        [Description("错误")]
        EVT_ERROR,
        [Description("写入")]
        EVT_WRITE,
        [Description("读取")]
        EVT_READ,
        [Description("取消")]
        EVT_CANCELED,
        [Description("重试")]
        EVT_RETRY,
    }

    public static class EnumExt
    {
        public static string GetDescription(this Enum en)
        {
            var temType = en.GetType();
            var memberInfos = temType.GetMember(en.ToString());
            if (memberInfos.Length > 0)
            {
                var desc = memberInfos[0].GetCustomAttribute<DescriptionAttribute>()?.Description ?? "";
                var parentDesc = temType.GetCustomAttribute<DescriptionAttribute>()?.Description ?? "";
                return $"{parentDesc}-{desc}";
            }
            return en.ToString();
        }
    }

    public class QQActionEvent : QQEvent
    {
        public QQActionEventType Type { get; }
        public object Target { get; }

        public QQActionEvent(QQActionEventType type, object target)
        {
            Type = type;
            Target = target;
        }

        public override string ToString()
        {
            return $"QQActionEvent [type={Type.GetFullDescription()}, target={Target}]";
        }
    }
}
