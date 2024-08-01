using Artillery.Common;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Country")]
    public class ImportCountryDto
    {
        [Required]
        [MinLength(ValidationConstants.CountryNameMinLength)]
        [MaxLength(ValidationConstants.CountryNameMaxLength)]
        [XmlElement("CountryName")]
        public string CountryName { get; set; } = null!;

        [Required]
        [Range(ValidationConstants.CountryNameMinArmy, ValidationConstants.CountryNameMaxArmy)]
        [XmlElement("ArmySize")]
        public int ArmySize { get; set; }
    }
}
