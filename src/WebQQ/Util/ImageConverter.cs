using System;
using ImageSharp;
using Newtonsoft.Json;
using WebQQ.Util;

namespace WebQQ.Util
{
    public class ImageConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ImageSharp.Image<Rgba32>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ((string) reader.Value).Base64StringToImage();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var bmp = (ImageSharp.Image<Rgba32>)value;
            writer.WriteValue(bmp.ToRawBase64String());
        }
    }
}
