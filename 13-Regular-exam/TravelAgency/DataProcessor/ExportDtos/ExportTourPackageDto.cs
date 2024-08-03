using System.Xml.Serialization;

namespace TravelAgency.DataProcessor.ExportDtos
{
    [XmlType("TourPackage")]
    public class ExportTourPackageDto
    {
        [XmlElement("Name")]
        public string PackageName { get; set; }

        [XmlElement("Description")]
        public string PackageDescription { get; set; }

        [XmlElement("Price")]
        public decimal Price { get; set; }
    }
}
