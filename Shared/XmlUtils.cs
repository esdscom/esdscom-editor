using System.IO;

namespace eSDSCom.Editor.Shared;

public static class XmlUtils
{
    public static string Serialize<T>(T obj)
    {
        StringWriter writer = new();
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        serializer.Serialize(writer, obj);
        return writer.ToString();
    }

    public static T Deserialize<T>(string xml)
    {
        T result = default;

        if (!string.IsNullOrEmpty(xml))
        {
            TextReader tr = new StringReader(xml);
            XmlReader reader = XmlReader.Create(tr);
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            if (serializer.CanDeserialize(reader))
                result = (T)serializer.Deserialize(reader);
        }
        return result;
    }

    public static XmlNode GetXmlNodeFromString(string xmlData)
    {
        XmlDocument doc = new();
        doc.LoadXml(xmlData);
        return doc.DocumentElement;
    }

    public static XmlDocument GetXmlDocFromString(string xmlData)
    {
        XmlDocument doc = new();
        doc.LoadXml(xmlData);
        return doc;
    }
}

