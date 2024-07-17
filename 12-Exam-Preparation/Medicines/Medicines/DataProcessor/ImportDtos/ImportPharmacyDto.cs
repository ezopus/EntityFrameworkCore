using Medicines.Shared;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType("Pharmacy")]
    public class ImportPharmacyDto
    {
        [XmlAttribute("non-stop")]
        [Required]
        public string IsNonStop { get; set; }

        [XmlElement("Name")]
        [Required]
        [MinLength(ValidationConstants.PharmacyNameMinLength)]
        [MaxLength(ValidationConstants.PharmacyNameMaxLength)]
        public string Name { get; set; } = null!;

        [XmlElement("PhoneNumber")]
        [Required]
        [MinLength(ValidationConstants.PhoneNumberLength)]
        [MaxLength(ValidationConstants.PhoneNumberLength)]
        [RegularExpression(ValidationConstants.PhoneNumberRegex)]
        public string PhoneNumber { get; set; } = null!;

        [XmlArray("Medicines")]
        [XmlArrayItem("Medicine")]
        [Required]
        public ImportMedicineDto[] Medicines { get; set; }

        //<Pharmacy non-stop="true">
        //<Name>Vitality</Name>
        //<PhoneNumber>(123) 456-7890</PhoneNumber>
        //<Medicines>
    }
}
