using System;
using AutoMapper;
using FclEx.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebQQ.Im.Event;

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
    }
}
