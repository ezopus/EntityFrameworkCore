using Invoices.Common;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType("Address")]
    public class ImportAddressDto
    {
        [XmlElement("StreetName")]
        [MinLength(GlobalConstants.StreetNameMinLength)]
        [MaxLength(GlobalConstants.StreetNameMaxLength)]
        [Required]
        public string StreetName { get; set; } = null!;

        [Required]
        [XmlElement("StreetNumber")]
        public int StreetNumber { get; set; }

        [Required]
        [XmlElement("PostCode")]
        public string PostCode { get; set; } = null!;

        [Required]
        [MinLength(GlobalConstants.CityMinLength)]
        [MaxLength(GlobalConstants.CityMaxLength)]
        [XmlElement("City")]
        public string CityName { get; set; } = null!;

        [Required]
        [MinLength(GlobalConstants.CountryMinLength)]
        [MaxLength(GlobalConstants.CountryMaxLength)]
        [XmlElement("Country")]
        public string CountryName { get; set; } = null!;
    }
}
