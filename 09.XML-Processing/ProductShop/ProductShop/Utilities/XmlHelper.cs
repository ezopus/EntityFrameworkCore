using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Utilities
{
    public class XmlHelper
    {
        //XmlSerializer - analogue to JsonConvert
        //Provides us with serialize, deserialize methods
        public T Deserialize<T>(string inputXml, string rootName)
        {
            //first we need a root for XML serializer to work
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);

            //create new serializer, this serializes + deserializes
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRoot);

            //create stream reader necessary for deserializing
            using StringReader reader = new StringReader(inputXml);

            //finally we can use Deserialize() and we need to cast it to specific type
            T dto = (T)xmlSerializer.Deserialize(reader);

            return dto;
        }

        public string Serialize<T>(T obj, string rootName)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRoot);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            using StringWriter writer = new StringWriter(sb);
            xmlSerializer.Serialize(writer, obj, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
