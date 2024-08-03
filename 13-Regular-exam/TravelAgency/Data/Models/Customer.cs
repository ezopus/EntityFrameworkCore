using System.ComponentModel.DataAnnotations;
using TravelAgency.Common;

namespace TravelAgency.Data.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.CustomerNameMaxLength)]
        public string FullName { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.CustomerEmailMaxLength)]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.CustomerPhoneNumber)]
        public string PhoneNumber { get; set; } = null!;

        public virtual ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
    }
}
