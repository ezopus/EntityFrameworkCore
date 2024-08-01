using System.Xml.Serialization;

namespace Artillery.DataProcessor.ExportDto
{
    [XmlType("Gun")]
    public class ExportGunDto
    {
        [XmlAttribute("Manufacturer")] public string Manufacturer { get; set; }

        [XmlAttribute("GunType")] public string GunType { get; set; }

        [XmlAttribute("GunWeight")] public int GunWeight { get; set; }

        [XmlAttribute("BarrelLength")] public double BarrelLength { get; set; }

        [XmlArray("Countries")]
        [XmlArrayItem("Country")]
        public ExportCountryDto[] Countries { get; set; }
    }
}
