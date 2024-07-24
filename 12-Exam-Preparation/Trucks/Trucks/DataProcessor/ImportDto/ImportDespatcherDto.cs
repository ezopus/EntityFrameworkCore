using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Trucks.Common;

namespace Trucks.DataProcessor.ImportDto;

[XmlType("Despatcher")]
public class ImportDespatcherDto
{
    [Required]
    [XmlElement("Name")]
    [MinLength(ValidationConstants.DespatcherNameMinLength)]
    [MaxLength(ValidationConstants.DespatcherNameMaxLength)]
    public string Name { get; set; } = null!;

    [Required]
    [XmlElement("Position")]
    public string? Position { get; set; }

    [XmlArray("Trucks")]
    [XmlArrayItem("Truck")]
    public ImportTruckDto[] TrucksDtos { get; set; }
}

