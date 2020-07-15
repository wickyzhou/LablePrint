using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Common
{
    public static class JsonHelper
    {
        public static T DeserializeObject<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static object DeserializeObject(string jsonString, Type type)
        {
            return JsonConvert.DeserializeObject(jsonString, type);
        }

        /// <summary>
        /// 对象序列化String
        /// 时间默认格式默认：yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeObject(object obj)
        {
            IsoDateTimeConverter timeCoverter = new IsoDateTimeConverter();
            timeCoverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented, timeCoverter);
        }

        /// <summary>
        /// 日期格式
        /// timeFormat：yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="timeFormat"></param>
        /// <returns></returns>
        public static string SerializeObject(object obj, string timeFormat)
        {
            IsoDateTimeConverter timeCoverter = new IsoDateTimeConverter();
            timeCoverter.DateTimeFormat = timeFormat;
            return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented, timeCoverter);
        }


        public static string ObjectToJson(object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            byte[] dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            return Encoding.UTF8.GetString(dataBytes);
        }
    }
}
