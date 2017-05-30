using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using FclEx.Extensions;
using ImageSharp;
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
    }
}
