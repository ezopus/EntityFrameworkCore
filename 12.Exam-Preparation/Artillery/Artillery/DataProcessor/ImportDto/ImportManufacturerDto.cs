using Artillery.Common;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Manufacturer")]
    public class ImportManufacturerDto
    {
        [Required]
        [XmlElement("ManufacturerName")]
        [MinLength(ValidationConstants.ManufacturerNameMinLength)]
        [MaxLength(ValidationConstants.ManufacturerNameMaxLength)]
        public string ManufacturerName { get; set; } = null!;

        [Required]
        [XmlElement("Founded")]
        [MinLength(ValidationConstants.ManufacturerMinFounded)]
        [MaxLength(ValidationConstants.ManufacturerMaxFounded)]
        public string Founded { get; set; } = null!;
    }
}
