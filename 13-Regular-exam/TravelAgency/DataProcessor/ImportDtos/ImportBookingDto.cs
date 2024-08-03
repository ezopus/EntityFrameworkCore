using System.ComponentModel.DataAnnotations;
using TravelAgency.Common;

namespace TravelAgency.DataProcessor.ImportDtos
{
    public class ImportBookingDto
    {
        [Required]
        public string BookingDate { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.CustomerNameMinLength)]
        [MaxLength(ValidationConstants.CustomerNameMaxLength)]
        public string CustomerName { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.TourPackageNameMinLength)]
        [MaxLength(ValidationConstants.TourPackageNameMaxLength)]
        public string TourPackageName { get; set; }
    }
}
