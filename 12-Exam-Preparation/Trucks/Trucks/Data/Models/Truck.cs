namespace Trucks.Data.Models;

using Common;
using Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Truck
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ValidationConstants.TruckRegNumberLength)]
    public string RegistrationNumber { get; set; } = null!;

    [Required]
    [MaxLength(ValidationConstants.TruckVinNumberLength)]
    public string VinNumber { get; set; } = null!;

    public int TankCapacity { get; set; }

    public int CargoCapacity { get; set; }

    [Required]
    public CategoryType CategoryType { get; set; }

    [Required]
    public MakeType MakeType { get; set; }

    [Required]
    [ForeignKey(nameof(Despatcher))]
    public int DespatcherId { get; set; }

    public Despatcher Despatcher { get; set; } = null!;

    public virtual ICollection<ClientTruck> ClientsTrucks { get; set; } = new HashSet<ClientTruck>();
}

