using System.Xml.Serialization;

namespace CarDealer.DTOs.Import
{
    [XmlType("Customer")]
    public class ImportCustomerDto
    {
        [XmlElement("name")]
        public string Name { get; set; } = null!;

        //Good practice:
        //Always read enums and dates as string, never let JsonConvert or XmlSerializer to parse
        [XmlElement("birthDate")]
        public string BirthDate { get; set; }

        [XmlElement("isYoungDriver")]
        public bool IsYoungDriver { get; set; }
    }
}
