using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using TravelAgency.Common;

namespace TravelAgency.DataProcessor.ImportDtos
{
    [XmlType("Customer")]
    public class ImportCustomerDto
    {
        [Required]
        [XmlElement("FullName")]
        [MinLength(ValidationConstants.CustomerNameMinLength)]
        [MaxLength(ValidationConstants.CustomerNameMaxLength)]
        public string CustomerFullName { get; set; } = null!;

        [Required]
        [XmlAttribute("phoneNumber")]
        [MinLength(ValidationConstants.CustomerPhoneNumber)]
        [MaxLength(ValidationConstants.CustomerPhoneNumber)]
        [RegularExpression(ValidationConstants.CustomerPhoneNumberRegex)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [XmlElement("Email")]
        [MinLength(ValidationConstants.CustomerEmailMinLength)]
        [MaxLength(ValidationConstants.CustomerEmailMaxLength)]
        public string Email { get; set; } = null!;
    }
}
