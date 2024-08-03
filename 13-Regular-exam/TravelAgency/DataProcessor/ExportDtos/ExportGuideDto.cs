using System.Xml.Serialization;

namespace TravelAgency.DataProcessor.ExportDtos
{
    [XmlType("Guide")]
    public class ExportGuideDto
    {
        [XmlElement("FullName")]
        public string GuideName { get; set; }


        [XmlArray("TourPackages")]
        [XmlArrayItem("TourPackage")]
        public ExportTourPackageDto[] Packages { get; set; }
    }
}
