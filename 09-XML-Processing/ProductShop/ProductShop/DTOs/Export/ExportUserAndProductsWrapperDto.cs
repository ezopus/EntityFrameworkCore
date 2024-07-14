using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{
    public class ExportUserAndProductsWrapperDto
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("users")]
        [XmlArrayItem("User")]
        public ExportUserAndProducts[] Users { get; set; } = null!;
    }
}
