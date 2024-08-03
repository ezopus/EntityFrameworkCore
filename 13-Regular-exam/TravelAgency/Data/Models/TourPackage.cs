using System.ComponentModel.DataAnnotations;
using TravelAgency.Common;

namespace TravelAgency.Data.Models
{
    public class TourPackage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.TourPackageNameMaxLength)]
        public string PackageName { get; set; } = null!;

        [MaxLength(ValidationConstants.TourPackageDescriptionMaxLength)]
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();

        public virtual ICollection<TourPackageGuide> TourPackagesGuides { get; set; } = new HashSet<TourPackageGuide>();
    }
}
