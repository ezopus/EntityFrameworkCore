using System.ComponentModel.DataAnnotations;
using Trucks.Common;

namespace Trucks.Data.Models;
public class Despatcher
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ValidationConstants.DespatcherNameMaxLength)]
    public string Name { get; set; } = null!;

    public string? Position { get; set; }

    public virtual ICollection<Truck> Trucks { get; set; } = new HashSet<Truck>();
}

