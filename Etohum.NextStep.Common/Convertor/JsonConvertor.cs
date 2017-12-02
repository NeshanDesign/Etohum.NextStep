using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Etohum.NextStep.Common.Convertor
{
    public class JsonConvertor
    {
        public static T ReadObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Ignore });
        }

        public static T ReadObject<T>(StreamReader stream)
        {
            return JsonConvert.DeserializeObject<T>(stream.ReadToEnd(), new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Ignore });
        }

        public static List<T> RaedObjects<T>(string json)
        {
            return JsonConvert.DeserializeObject<List<T>>(json, new JsonSerializerSettings { });
        }

        public static string SerializeObject<T>(T obj, bool formatted = false)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Formatting = formatted ? Formatting.Indented : Formatting.None });
        }
    }
}
