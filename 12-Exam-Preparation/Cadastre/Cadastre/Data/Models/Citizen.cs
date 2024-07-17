namespace Cadastre.Data.Models;

using Enumerations;
using Shared;
using System.ComponentModel.DataAnnotations;
public class Citizen
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(GlobalConstants.CitizenFirstNameMaxLength)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(GlobalConstants.CitizenLastNameMaxLength)]
    public string LastName { get; set; } = null!;

    [Required]
    public DateTime BirthDate { get; set; }

    [Required]
    public MaritalStatus MaritalStatus { get; set; }

    [Required]
    public virtual ICollection<PropertyCitizen> PropertiesCitizens { get; set; }
        = new HashSet<PropertyCitizen>();



    //Citizen
    //• Id – integer, Primary Key
    //• FirstName – text with length[2, 30] (required)
    //• LastName – text with length[2, 30] (required)
    //• BirthDate – DateTime(required)
    //    • MaritalStatus - MaritalStatus enum (Unmarried = 0, Married, Divorced, Widowed) (required)
    //• PropertiesCitizens - collection of type PropertyCitizen
}

