using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebQQ.Im.Event;

namespace WebQQ.Util
{
    public class QQNotifyEventJsonConventer : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(QQNotifyEvent);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
