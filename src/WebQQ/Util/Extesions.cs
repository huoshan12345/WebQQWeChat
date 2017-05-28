using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using FclEx.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Core;
using WebQQ.Im.Event;
using WebQQ.Im.Module.Impl;

namespace WebQQ.Util
{
    public static class Extesions
    {
        public static T MapTo<T>(this object obj)
        {
            return Mapper.Map<T>(obj);
        }

        public static void MapTo(this object source, object dest)
        {
            Mapper.Map(source, dest);
        }

        public static T Get<T>(this QQNotifyEvent e)
        {
            return (T)e.Target;
        }

        public static Task FireNotifyAsync(this IQQContext context, QQNotifyEventType type, object target = null)
        {
            return context.FireNotifyAsync(QQNotifyEvent.CreateEvent(type, target));
        }

        public static bool IsOnline(this IQQContext qqClient)
        {
            var session = qqClient.GetModule<SessionModule>();
            return session.State == SessionState.Online;
        }

        public static bool IsOffline(this IQQContext qqClient)
        {
            var session = qqClient.GetModule<SessionModule>();
            return session.State == SessionState.Offline;
        }

        public static byte[] Base64StringToBytes(this string base64String) => Convert.FromBase64String(base64String);

        public static string ToBase64String(this byte[] bytes) => Convert.ToBase64String(bytes);

        public static string ToBase64String(this Bitmap bitmap)
        {
            using (var m = new MemoryStream())
            {
                bitmap.Save(m, ImageFormat.Jpeg);
                return m.ToArray().ToBase64String();
            }
        }

        public static Bitmap Base64StringToBitmap(this string base64String)
        {
            using (var m = new MemoryStream(Convert.FromBase64String(base64String)))
            {
                return (Bitmap)Image.FromStream(m);
            }
        }
    }
}
