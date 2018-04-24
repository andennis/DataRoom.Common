using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Common.Extensions
{
    public static class XmlExtensions
    {
        public static void SaveToXml(this object obj, string filePath, string custNamespace = null)
        {
            var xws = new XmlWriterSettings() { Indent = true, Encoding = Encoding.UTF8 };
            var serializer = new XmlSerializer(obj.GetType());
            using (var xmlWriter = XmlWriter.Create(filePath, xws))
            {
                if (custNamespace != null)
                {
                    var ns = new XmlSerializerNamespaces();
                    ns.Add("", custNamespace);
                    serializer.Serialize(xmlWriter, obj, ns);
                }
                else
                {
                    serializer.Serialize(xmlWriter, obj);
                }
            }
        }

        public static TResult LoadFromXml<TResult>(this string filePath, string custNamespace = null)
        {
            var doc = new XmlDocument();
            doc.Load(filePath);

            using (var read = new StringReader(doc.OuterXml))
            using (var reader = new XmlTextReader(read))
            {
                if (custNamespace != null)
                {
                    var serializer = new XmlSerializer(typeof (TResult), custNamespace);
                    return (TResult)serializer.Deserialize(reader);
                }
                else
                {
                    var serializer = new XmlSerializer(typeof(TResult));
                    return (TResult)serializer.Deserialize(reader);
                }
            }
        }
    }
}
