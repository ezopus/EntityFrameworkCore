using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Trucks.Common;

namespace Trucks.DataProcessor.ImportDto;

[XmlType("Truck")]
public class ImportTruckDto
{
    [Required]
    [XmlElement("RegistrationNumber")]
    [MinLength(ValidationConstants.TruckRegNumberLength)]
    [MaxLength(ValidationConstants.TruckRegNumberLength)]
    [RegularExpression(ValidationConstants.TruckRegistrationRegex)]
    public string RegistrationNumber { get; set; } = null!;

    [Required]
    [XmlElement("VinNumber")]
    [MinLength(ValidationConstants.TruckVinNumberLength)]
    [MaxLength(ValidationConstants.TruckVinNumberLength)]
    public string VinNumber { get; set; } = null!;

    [Range(ValidationConstants.TruckTankMinCapacity, ValidationConstants.TruckTankMaxCapacity)]
    public int TankCapacity { get; set; }

    [Range(ValidationConstants.TruckCargoMinCapacity, ValidationConstants.TruckCargoMaxCapacity)]
    public int CargoCapacity { get; set; }

    [Required]
    [Range(0, ValidationConstants.CategoryMaxRange)]
    public int CategoryType { get; set; }

    [Required]
    [Range(0, ValidationConstants.MakeMaxRange)]
    public int MakeType { get; set; }
}

