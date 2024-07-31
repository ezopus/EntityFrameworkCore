using Cadastre.Shared;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ImportDtos;

[XmlType("District")]
public class ImportDistrictDto
{
    [XmlAttribute("Region")]
    [Required]
    public string Region { get; set; } = null!;

    [XmlElement("Name")]
    [MinLength(GlobalConstants.DistrictNameMinLength)]
    [MaxLength(GlobalConstants.DistrictNameMaxLength)]
    [Required]
    public string Name { get; set; } = null!;

    [XmlElement("PostalCode")]
    [MinLength(GlobalConstants.DistrictPostalCodeLength)]
    [MaxLength(GlobalConstants.DistrictPostalCodeLength)]
    [RegularExpression(GlobalConstants.DistrictPostalCodeRegex)]
    [Required]
    public string PostalCode { get; set; } = null!;

    [XmlArray("Properties")]
    [XmlArrayItem("Property")]
    public ImportPropertyDto[] Properties { get; set; }

    //<District Region = "SouthWest" >

    //< Name > Sofia </ Name >

    //< PostalCode > SF - 10000 </ PostalCode >

    //< Properties >

    //< Property >

}

