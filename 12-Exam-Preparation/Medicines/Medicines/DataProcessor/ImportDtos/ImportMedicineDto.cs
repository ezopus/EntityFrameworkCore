using Medicines.Shared;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType("Medicine")]
    public class ImportMedicineDto
    {
        [XmlAttribute("category")]
        [Range(ValidationConstants.MedicineCategoryMinRange, ValidationConstants.MedicineCategoryMaxRange)]
        public int Category { get; set; }

        [XmlElement("Name")]
        [MinLength(ValidationConstants.MedicineNameMinLength)]
        [MaxLength(ValidationConstants.MedicineNameMaxLength)]
        [Required]
        public string Name { get; set; } = null!;

        [XmlElement("Price")]
        [Range(ValidationConstants.MedicinePriceRangeMin, ValidationConstants.MedicinePriceRangeMax)]
        [Required]
        public double Price { get; set; }

        [XmlElement("ProductionDate")]
        [Required]
        public string ProductionDate { get; set; } = null!;

        [XmlElement("ExpiryDate")]
        [Required]
        public string ExpiryDate { get; set; } = null!;

        [XmlElement("Producer")]
        [MinLength(ValidationConstants.MedicineProducerMinLength)]
        [MaxLength(ValidationConstants.MedicineProducerMaxLength)]
        [Required]
        public string Producer { get; set; } = null!;


        //<Medicine category = "1" >

        //< Name > Ibuprofen </ Name >

        //< Price > 8.50 </ Price >

        //< ProductionDate > 2022 - 02 - 10 </ ProductionDate >

        //< ExpiryDate > 2025 - 02 - 10 </ ExpiryDate >

        //< Producer > ReliefMed Labs</Producer>
        //</Medicine>
    }
}
