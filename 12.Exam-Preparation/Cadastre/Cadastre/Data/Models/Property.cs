using Cadastre.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cadastre.Data.Models;

public class Property
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(GlobalConstants.PropertyIdentifierMaxLength)]
    public string PropertyIdentifier { get; set; } = null!;

    [Required]
    public int Area { get; set; }

    [MaxLength(GlobalConstants.PropertyDetailsMaxLength)]
    public string? Details { get; set; }

    [Required]
    [MaxLength(GlobalConstants.PropertyAddressMaxLength)]
    public string Address { get; set; } = null!;

    [Required]
    public DateTime DateOfAcquisition { get; set; }

    [ForeignKey(nameof(District))]
    [Required]
    public int DistrictId { get; set; }
    public virtual District District { get; set; }

    public virtual ICollection<PropertyCitizen> PropertiesCitizens { get; set; }
        = new HashSet<PropertyCitizen>();


    //Property
    //• Id – integer, Primary Key
    //• PropertyIdentifier – text with length[16, 20] (required)
    //• Area – int not negative(required)
    //    • Details - text with length[5, 500] (not required)
    //• Address – text with length[5, 200] (required)
    //• DateOfAcquisition – DateTime(required)
    //    • DistrictId – integer, foreign key(required)
    //    • District – District
    //• PropertiesCitizens - collection of type PropertyCitizen
}

