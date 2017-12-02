using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Etohum.NextStep.Common.Convertor
{
    public static class XmlConvertor
    {
        public static T GetObject<T>(string path)
        {
            T result;
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var reader = new XmlTextReader(path))
            {
                result = (T)xmlSerializer.Deserialize(reader);
            }

            return result;
        }

        public static T GetObject<T>(Stream stream)
        {
            return GetObject<T>(new StreamReader(stream));
        }
        public static T GetObject<T>(StreamReader stream)
        {
            var text = stream.ReadToEnd();

            stream.Close();
            stream.Dispose();

            var textStream = new StringReader(text);
            T result;
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var reader = new XmlTextReader(textStream))
            {
                result = (T)xmlSerializer.Deserialize(reader);
            }
            return result;
        }

    }


}
