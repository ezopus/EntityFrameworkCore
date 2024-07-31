using Cadastre.Shared;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ImportDtos;

public class ImportPropertyDto
{
    [XmlElement("PropertyIdentifier")]
    [MinLength(GlobalConstants.PropertyIdentifierMinLength)]
    [MaxLength(GlobalConstants.PropertyIdentifierMaxLength)]
    [Required]
    public string PropertyIdentifier { get; set; } = null!;

    [XmlElement("Area")]
    [Range(0, int.MaxValue)]
    [Required]
    public int Area { get; set; }

    [XmlElement("Address")]
    [MinLength(GlobalConstants.PropertyAddressMinLength)]
    [MaxLength(GlobalConstants.PropertyAddressMaxLength)]
    [Required]
    public string Address { get; set; } = null!;

    [XmlElement("DateOfAcquisition")]
    [Required]
    public string DateOfAcquisition { get; set; } = null!;

    [XmlElement("Details")]
    [MinLength(GlobalConstants.PropertyDetailsMinLength)]
    [MaxLength(GlobalConstants.PropertyDetailsMaxLength)]
    public string? Details { get; set; }

    //<Property>
    //<PropertyIdentifier>SF-10000.001.001.001</PropertyIdentifier>
    //<Area>71</Area>
    //<Details>One-bedroom apartment</Details>
    //<Address>Apartment 5, 23 Silverado Street, Sofia</Address>
    //<DateOfAcquisition>15/03/2022</DateOfAcquisition>
    //</Property>
}

