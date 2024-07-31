using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{
    public class ExportUserAndProducts
    {
        [XmlElement("firstName")]
        public string? FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; } = null!;

        [XmlElement("age")]
        public int? Age { get; set; }

        [XmlElement("soldProducts")]
        public SoldProductDto[] Products { get; set; } = null!;


    }
}
