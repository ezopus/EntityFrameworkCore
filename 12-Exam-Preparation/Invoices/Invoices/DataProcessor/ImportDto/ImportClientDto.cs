using Invoices.Common;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType("Client")]
    public class ImportClientDto
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(GlobalConstants.ClientNameMinLength)]
        [MaxLength(GlobalConstants.ClientNameMaxLength)]
        public string Name { get; set; }

        [XmlElement("NumberVat")]
        [Required]
        [MinLength(GlobalConstants.ClientVATMinLength)]
        [MaxLength(GlobalConstants.ClientVATMaxLength)]
        public string NumberVat { get; set; } = null!;

        [XmlArray("Addresses")]
        [XmlArrayItem("Address")]
        public ImportAddressDto[]? ImportAddressDtos { get; set; }
    }
}
