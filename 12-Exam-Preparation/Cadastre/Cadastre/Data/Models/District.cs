namespace Cadastre.Data.Models;

using Enumerations;
using Shared;
using System.ComponentModel.DataAnnotations;
public class District
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(GlobalConstants.DistrictNameMaxLength)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(GlobalConstants.DistrictPostalCodeLength)]
    public string PostalCode { get; set; }

    [Required]
    public Region Region { get; set; }

    public virtual ICollection<Property> Properties { get; set; } = new HashSet<Property>();

    //District
    //• Id – integer, Primary Key
    //• Name – text with length[2, 80] (required)
    //• PostalCode – text with length 8. All postal codes must have the following structure:starting with two capital letters, followed by e dash '-', followed by five digits.Example: SF-10000 (required)
    //• Region – Region enum (SouthEast = 0, SouthWest, NorthEast, NorthWest) (required)
    //• Properties - collection of type Property
}

